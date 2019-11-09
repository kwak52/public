using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
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
        public Dictionary<TRNode, SubRung> TRmap = new Dictionary<TRNode, SubRung>();

        /// <summary>
        /// Rung build 중에 작업하고 있는 현재의 sub rung
        /// </summary>
        public SubRung CurrentBuildingLD { get; private set; }
        /// <summary>
        /// CurrentBuildingLD
        /// </summary>
        SubRung _cbld => CurrentBuildingLD;

        /// <summary>
        /// 변환하기 위한 source PLC 의 instruction list text array
        /// </summary>
        string[] _mnemonics;

        /// <summary>
        /// 임시 rung 들을 모아 놓은 stack
        /// </summary>
        public Stack<SubRung> LadderStack;

        /// <summary>
        /// 현재 parsing 진행 중인 니모닉의 index
        /// </summary>
        public int CurrentMnemonicIndex { get; private set; } = -1;


        /// <summary>
        /// 현재 rung 이 생성하려는 목적 PLC type
        /// </summary>
        internal PLCVendor _targetType;

        /// <summary>
        /// 현재 rung 을 생성하는데 사용된 PLC type
        /// </summary>
        PLCVendor _sourceType;
        string _strMPush = "MPUSH";
        string _strMPop = "MPOP";
        string _strMLoad = "MLOAD";
        string _strLD = "LD";


        /// <summary>
        /// Parsing 실패하였을 경우의 message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        public Rung4Parsing(IEnumerable<string> mnemonics, string rungComment, ConvertParams cvtParam)
        {
            _mnemonics = mnemonics.ToArray();
            LadderStack = new Stack<SubRung>();
            _targetType = cvtParam.TargetType;
            _sourceType = cvtParam.SourceType;
            if (rungComment.NonNullAny())
                RungComment = ILSentence.CreateRungComments(cvtParam.SourceType, rungComment).ToList();

            _strMPush = IL.GetOperator(_targetType, Mnemonic.MPUSH);
            _strMPop = IL.GetOperator(_targetType, Mnemonic.MPOP);
            _strMLoad = IL.GetOperator(_targetType, Mnemonic.MLOAD);
            _strLD = "LD";
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
                try
                {
                    CurrentMnemonicIndex = i;
                    Logger?.Info($"IL: {m}");

                    var sentence = ILSentence.Create(_sourceType, m);
                    var arg0 = sentence.Args.IsNullOrEmpty() ? null : sentence.Args[0];
                    var arg0N = new Point(arg0) { ILSentence = sentence };
                    var arity = sentence.ILCommand.Arity;
                    var ilCommand = sentence.ILCommand;
                    var udc = ilCommand as UserDefinedILCommand;


                    switch (sentence.Mnemonic)
                    {
                        case Mnemonic.RUNG_COMMENT:
                            RungComment.Add(sentence);
                            break;
                        case Mnemonic.LOAD when arg0.StartsWith("TR"):
                            _cbld.LDTR(arg0);
                            break;

                        case Mnemonic.LOAD:
                        case Mnemonic.LOADNOT:
                            if (_cbld != null)
                                LadderStack.Push(_cbld);
                            CurrentBuildingLD = new SubRung(this, arg0N);
                            break;

                        case Mnemonic.ANDNOT:
                            Logger?.Warn("ANDNOT : assume AND");
                            _cbld.AND(arg0N, sentence);
                            break;
                        case Mnemonic.AND:
                            _cbld.AND(arg0N, sentence);
                            break;

                        case Mnemonic.ANDLD:
                            CurrentBuildingLD = LadderStack.Pop().ANDLD(_cbld);
                            break;

                        case Mnemonic.OR:
                        case Mnemonic.ORNOT:
                            _cbld.OR(arg0N, sentence);
                            break;
                        case Mnemonic.ORLD:
                            CurrentBuildingLD = LadderStack.Pop().ORLD(_cbld);
                            break;

                        case Mnemonic.OUT when arg0 != null && arg0.StartsWith("TR"):
                            _cbld.OUTTR(new TRNode(arg0, sentence), sentence);
                            break;

                        case Mnemonic.TON: // timer output
                        case Mnemonic.OUT:
                        case Mnemonic.CMP:
                            _cbld.OUT(new OutNode($"{sentence}", sentence), sentence);
                            break;

                        case Mnemonic.USERDEFINED when udc.IsTerminal:
                            _cbld.ConnectFunctionParameters(sentence, LadderStack);
                            break;

                        case Mnemonic.USERDEFINED when ! udc.IsTerminal:
                            _cbld.AND(arg0N, sentence);
                            break;

                        case Mnemonic.UNDEFINED:
                        default:
                            if (arity == 1)
                            {
                                Logger?.Warn($"Unknown IL with arity=1: {m}");
                                _cbld.OUT(new OutNode($"{sentence}", sentence), sentence);
                            }
                            else if (arity > 1)
                            {
                                _cbld.ConnectFunctionParameters(sentence, LadderStack);
                                Console.WriteLine("");
                            }
                            else
                                Logger?.Error($"Unknown IL: {m}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ex}");
                    ErrorMessage = $"Error while processing [{m}].   {ex.Message}";
                    yield break;
                }
                yield return i++;
            }

            try
            {
                PostProcessing();


                Debug.Assert(LadderStack.IsNullOrEmpty());
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error {ex.Message}";
                Logger.Error($"{ex}");
            }
        }


        /// <summary>
        /// IL code 생성을 편하게 하기 위해서 기본적으로 만들어진 rung 에 대해서 graph 구조를 일부 수정
        /// </summary>
        void PostProcessing()
        {
            // Outgoing edge 에 대해서 comment 추가
            _cbld.Nodes
                .OfType<AuxNode>()
                .Where(n => _cbld.GetOutgoingDegree(n) > 1)
                .Iter(n =>
                {
                    _cbld.GetOutgoingEdges(n)
                        .Iter((e, nth) =>
                        {
                            e.Data.Comment = $"[{nth}]{e.Data.Output}";
                        });
                });


            var mpush = _targetType.IsMPushModel();
            var trNodes =
                _cbld.Nodes
                    .OfType<TRNode>()
                    .ToArray();

            trNodes.Iter(n =>
            {
                var outg = _cbld.GetOutgoingEdges(n).ToArray();
                outg[0].Data.Comment += "MPUSH//222";

                if (mpush)
                {
                    // see: TR Buggy123
                    // TRNode (-> n) 이후에 AND node 가 따라오는 경우, 옴론과 LS 산전 IL 구조가 상이함
                    var secondStepOutTr =
                        outg
                            .Skip(1)
                            .Where(e => e.End is EndNode)   // outgoing edge 의 두번째부터 EndNode  로 가는 경우에
                            .SelectMany(e => _cbld.GetOutgoingEdges(e.End).Where(e2 => e2.End.ILSentence?.Command == "AND")) // EndNode 에서 AND 로 연결된 edge 들을 모두 모음
                            .ToArray()
                            ;
                    secondStepOutTr.Iter(e =>
                    {
                        _cbld.AddEdge(n, e.End);
                        _cbld.RemoveEdge(e.Start, e.End);
                    });


                    // TRNode 의 outgoing edge 를 {MPUSH, MLOAD, MPOP} 로  tagging
                    outg = _cbld.GetOutgoingEdges(n).ToArray();
                    outg[0].Data.Output = _strMPush;                // 1-st edge : MPUSH
                    outg[outg.Length - 1].Data.Output = _strMPop;   // last edge : MPOP
                    if (outg.Length > 2)
                        outg.Skip(1).Take(outg.Length - 2).Iter(m => m.Data.Output = _strMLoad);        // 중간 edge(2nd ~ 마지막전) : MLOAD
                }
                else
                {
                    // TRNode 의 outgoing edge 를 LD TRxxx 로  tagging
                    outg[0].Data.Output = $"{n.ILSentence}";
                    outg.Skip(1).Iter(m => m.Data.Output = $"{_strLD} {n.ILSentence.Args[0]}");
                }
            });

        }


        /// <summary>
        /// Rung 구축을 위한 단계로 사용된 Rung4Parsing 로부터 최종 Rung 을 생성해서 반환한다.
        /// Rung 구축 중간에 사용된 임시 node 들을 제거
        /// </summary>
        /// <returns></returns>
        public Rung ToRung(bool removeAuxNode=true)
        {
            var rung = new Rung(_mnemonics, RungComment);
            rung.MergeGraph(_cbld);
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
                        System.Linq.Enumerable.ToHashSet(
                            consecutiveAuxNodePairEdges
                            .Select(e => e.Start)
                            .Concat(consecutiveAuxNodePairEdges.Select(e => e.End)))                            
                            ;

                    // Rung 시작 node 이거나, AuxNode 이면서 incoming/outgoing edge 갯수가 모두 1 인 node 들을 삭제한다.
                    var targetAuxNodes =
                        rung.Nodes
                            .OfType<AuxNode>()
                            .Where(n =>
                            {
                                var nIn = rung.GetIncomingDegree(n);
                                var nOut = rung.GetOutgoingDegree(n);
                                return
                                    //nIn == 0 ||                     // start node
                                    (nIn == 1 && nOut == 1       // passing node
                                                                 //&& !(rung.GetIncomingNodes(n).First() is AuxNode)
                                                                 //&& !(rung.GetOutgoingNodes(n).First() is AuxNode)
                                                                 //&& !(rung.GetOutgoingNodes(n).First() is OutNode)
                                        )
                                    || isConsecutiveAuxNode(n)
                                    ;

                                bool isConsecutiveAuxNode(AuxNode an)
                                {
                                    if (rung.GetIncomingDegree(an) == 0)
                                        return false;

                                    var prev = rung.GetIncomingEdges(an).First().Start;
                                    return nIn == 1 && prev is AuxNode && consecutiveAuxNodes.Contains(an);
                                }
                            })
                            .ToArray()     // 컬렉션이 수정
                            ;

                    targetAuxNodes.Iter(n => rung.OmitPoint(n));

                    rung.Nodes
                        .OfType<AuxNode>()
                        .Where(n => rung.GetIncomingNodes(n).All(inc => !(inc is AuxNode)) && rung.GetTheOutgoingNode(n, true) is AuxNode)
                        .ToArray()
                        .Iter(n => rung.OmitPoint(n));
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

                    targetAuxNodes.Iter(n => rung.OmitPoint(n));
                }
            }

            return rung;
        }

    }
}
