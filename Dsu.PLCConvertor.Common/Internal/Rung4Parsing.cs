using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 기능 중, 실제 rung 을 구성하고 build 하기 위한 구조.  build 가 끝나면 Rung 구조로 다시 반환(ToRung())
    /// </summary>
    internal class Rung4Parsing : Rung
    {
        public Dictionary<AuxNode, SubRung> TRmap = new Dictionary<AuxNode, SubRung>();

        /// <summary>
        /// Rung build 중에 작업하고 있는 현재의 sub rung
        /// </summary>
        public SubRung CurrentBuildingLD { get; private set; }

        string[] _mnemonics;

        /// <summary>
        /// 임시 rung 들을 모아 놓은 stack
        /// </summary>
        public Stack<SubRung> LadderStack;

        /// <summary>
        /// 현재 parsing 진행 중인 니모닉의 index
        /// </summary>
        public int CurrentMnemonicIndex { get; private set; } = -1;
        public Rung4Parsing(IEnumerable<string> mnemonics)
        {
            _mnemonics = mnemonics.ToArray();
            LadderStack = new Stack<SubRung>();
        }

        /// <summary>
        /// Co-Routine 을 이용한 단계별 parsing.  반환값은 무시
        /// </summary>
        // 다음과 같이 사용한다.
        // _parsingStages = _rung4Parsing.CoRoutineRungParser().GetEnumerator();
        // _parsingStages.MoveNext();  // 한 스텝씩 parsing
        // 마지막까지 parsing 하려면 _rung4Parsing.CoRoutineRungParser().ToArray(); 등의 방식으로 generation
        public IEnumerable<int> CoRoutineRungParser()
        {
            int i = 0;
            foreach (var m in _mnemonics)
            {
                CurrentMnemonicIndex = i;
                Logger?.Info($"IL: {m}");

                var sentence = new ILSentence(m);
                var arg0 = sentence.Args.IsNullOrEmpty() ? null : sentence.Args[0];
                var arg0N = new Point(arg0) { ILSentence = sentence };
                switch (sentence.Command)
                {
                    case "LD" when arg0.StartsWith("TR"):
                        CurrentBuildingLD.LDTR(arg0);
                        break;

                    case "LD":
                    case "LDNOT":
                        if (CurrentBuildingLD != null)
                            LadderStack.Push(CurrentBuildingLD);
                        CurrentBuildingLD = new SubRung(this, arg0N);
                        break;

                    case "ANDNOT":
                        Logger?.Warn("ANDNOT : assume AND");
                        CurrentBuildingLD.AND(arg0N, sentence);
                        break;
                    case "AND":
                        CurrentBuildingLD.AND(arg0N, sentence);
                        break;

                    case "ANDLD":
                        CurrentBuildingLD = LadderStack.Pop().ANDLD(CurrentBuildingLD);
                        break;

                    case "OR":
                    case "ORNOT":
                        CurrentBuildingLD.OR(arg0N, sentence);
                        break;
                    case "ORLD":
                        CurrentBuildingLD = LadderStack.Pop().ORLD(CurrentBuildingLD);
                        break;

                    case "OUT" when arg0 != null && arg0.StartsWith("TR"):
                        CurrentBuildingLD.OUTTR(new TRNode(arg0), sentence);
                        break;
                    case "OUT":
                        CurrentBuildingLD.OUT(new TerminalNode(arg0), sentence);
                        break;
                    default:
                        Logger?.Error($"Unknown IL: {m}");
                        break;
                }

                yield return i++;
            }

            Debug.Assert(LadderStack.IsNullOrEmpty());
            Console.WriteLine("");
        }

        /// <summary>
        /// Rung 구축을 위한 단계로 사용된 Rung4Parsing 로부터 최종 Rung 을 생성해서 반환한다.
        /// Rung 구축 중간에 사용된 임시 node 들을 제거
        /// </summary>
        /// <returns></returns>
        public Rung ToRung(bool removeAuxNode=true)
        {
            var rung = new Rung(_mnemonics);
            rung.MergeGraph(CurrentBuildingLD);
            if (removeAuxNode)
            {
                {
                    // AuxNode - AuxNode 간의 연속 연결을 찾는다.
                    var consecutiveAuxNodePairEdges =
                        rung.Edges
                            .Where(e => e.Start is AuxNode && e.End is AuxNode)
                            .Where(e => !(e.Start is TRNode) && !(e.End is TRNode))
                            ;
                    var consecutiveAuxNodes =
                        consecutiveAuxNodePairEdges
                            .Select(e => e.Start)
                            .Concat(consecutiveAuxNodePairEdges.Select(e => e.End))
                            .ToHashSet()
                            ;

                    // Rung 시작 node 이거나, AuxNode 이면서 incoming/outgoing edge 갯수가 모두 1 인 node 들을 삭제한다.
                    var targetAuxNodes =
                        rung.Nodes
                            .OfType<AuxNode>()
                            .Where(n =>
                            {
                                var nIn = rung.GetIncomingDegree(n);
                                var nOut = rung.GetOutgoingDegree(n);
                                return nIn == 0                     // start node
                                    || (nIn == 1 && nOut == 1 && !(rung.GetOutgoingNodes(n).First() is TerminalNode))      // passing node
                                    || isConsecutiveAuxNode(n)
                                    ;

                                bool isConsecutiveAuxNode(AuxNode an)
                                {
                                    var prev = rung.GetIncomingEdges(an).First().Start;
                                    return nIn == 1 && prev is AuxNode && consecutiveAuxNodes.Contains(an);
                                }
                            })
                            .ToArray();

                    targetAuxNodes.Iter(n => rung.OmitNode(n));
                }

                {
                    // incoming edge 가 하나인 TR node 의 이전 node 가 AuxNode 인 경우, 이전 node 를 삭제한다.
                    var targetAuxNodes =
                        rung.Nodes
                            .OfType<TRNode>()
                            .Where(n => rung.GetIncomingDegree(n) == 1)
                            .Where(n => rung.GetIncomingNodes(n).First() is AuxNode)
                            .Select(n => rung.GetIncomingNodes(n).First())
                            .ToArray();

                    targetAuxNodes.Iter(n => rung.OmitNode(n));
                }
            }

            return rung;
        }
    }
}
