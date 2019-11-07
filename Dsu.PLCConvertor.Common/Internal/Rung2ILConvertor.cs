using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Graph;
using Dsu.PLCConvertor.Common.Internal;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common
{
    /// <summary>
    /// Rung 을 LS산전 IL 로 변환
    /// </summary>
    internal partial class Rung2ILConvertor
    {
        ILog Logger = Global.Logger;
        Rung _rung;
        ConvertParams _convertParam;
        PLCVendor _targetType => _convertParam.TargetType;
        Rung2ILConvertor(Rung rung, ConvertParams cvtParam)
        {
            _rung = rung;
            _convertParam = cvtParam;
        }




        /// <summary>
        /// IL 변환 중에 방문한 nodes.  모든 node 를 cover 하지 않으면 오류가 있는 것으로 봐야 한다.
        /// </summary>
        HashSet<Point> _visitedNodes = new HashSet<Point>();
        
        /// <summary>
        /// IL 변환 중에 방문한 edges.
        /// </summary>
        HashSet<Edge<Point, Wire>> _visitedEdges = new HashSet<Edge<Point, Wire>>();

        /// <summary>
        /// IL 변환 중에 방문하게 될 edge 의 stack.
        /// 하나의 node 를 만나면, 2th ~ nth edge 를 stack 에 push 하고 첫번째 edge 를 따라간다.
        /// </summary>
        Stack<Edge<Point, Wire>> _edgeStack = new Stack<Edge<Point, Wire>>();

        /// <summary>
        /// 주어진 node 를 따라가면서 IL 을 생성한다.
        /// </summary>
        IEnumerable<string> FollowNode(Point node)
        {
            Logger?.Debug($"FollowNode({node.Name})");

            if (_visitedNodes.Contains(node))
                yield break;

            _visitedNodes.Add(node);

            IEnumerable<string> xs;

            bool followedMe = false;

            // 현재 node 처리
            switch (node)
            {
                case StartNode sn:
                    break;

                case EndNode en:
                    // endNode 로 들어오는 edge 중에 아직 처리되지 않은 것이 남았다면..
                    while (_rung.GetIncomingEdges(en).Any(e => !_visitedEdges.Contains(e)))
                    {
                        if (_edgeStack.Any())
                        {
                            xs = FollowEdge(_edgeStack.Pop()).ToArray();
                            foreach (var x in xs)
                                yield return x;
                        }
                        else
                        {
                            // articulation point 에 도달.  output 쪽으로 달려야 함.
                            xs = FollowNodeOutgoingEdges(en);
                            foreach (var x in xs)
                                yield return x;
                            break;
                        }
                    }
                    //xs = FollowNodeOutgoingEdges(en).ToArray();
                    //foreach (var x in xs)
                    //    yield return x;
                    break;

                case TRNode trn:
                    followedMe = true;
                    xs = FollowMe().ToArray();
                    foreach (var x in xs)
                        yield return x;

                    xs = FollowEdgeStack().ToArray();
                    foreach (var x in xs)
                        yield return x;
                    break;
                case DummyNode dn:
                    xs = FollowNodeOutgoingEdges(dn);
                    foreach (var x in xs)
                        yield return x;
                    break;

                case TerminalNode udt when udt.ILSentence.ILCommand is UserDefinedILCommand:
                    var udc = udt.ILSentence.ILCommand;
                    Console.WriteLine("");
                    break;

                // Output node 를 비롯한 출력단의 node.  output node 자신을 출력하고 stack 의 내용을 따라간다.
                case TerminalNode tln:
                    yield return tln.ToIL(_targetType);
                    xs = FollowEdgeStack();
                    foreach (var x in xs)
                        yield return x;
                    break;

                // 기본 data node
                default:
                    yield return node.ToIL(_targetType);
                    break;
            }

            if (! followedMe )
            {
                xs = FollowMe().ToArray();
                foreach (var x in xs)
                    yield return x;
            }
            IEnumerable<string> FollowMe()
            {
                // 현재 node 의 outgoing edge 를 따라가기
                var oes = _rung.GetOutgoingEdges(node).ToArray();
                if (oes.Length == 0)
                    yield break;
                else
                {
                    if (oes.Length > 1)
                        _edgeStack.PushMultiples(oes.Skip(1));

                    xs = FollowEdge(oes[0]).ToArray();
                    foreach (var x in xs)
                        yield return x;
                }
            }

            // Edge stack 에 존재하는 edge 를 따라가기
            IEnumerable<string> FollowEdgeStack()
            {
                while (_edgeStack.Any())
                {
                    foreach (var x in FollowEdge(_edgeStack.Pop()))
                        yield return x;
                }
            }

            IEnumerable<string> FollowNodeOutgoingEdges(Point nnode)
            {
                var oges = _rung.GetOutgoingEdges(nnode).ToArray();
                _edgeStack.PushMultiples(oges.Skip(1));
                return FollowEdge(oges[0]);
            }
        }


        /// <summary>
        /// 주어진 edge 를 따라가면서 IL 을 생성한다.
        /// </summary>
        IEnumerable<string> FollowEdge(Edge<Point, Wire> edge)
        {
            Logger?.Debug($"FollowEdge({edge.Data})");
            if (_visitedEdges.Contains(edge))
                yield break;

            _visitedEdges.Add(edge);

            var edata = edge.Data.Output;
            if (edata.NonNullAny())
                yield return edata;

            var xs = FollowNode(edge.End).ToArray();
            foreach ( var x in xs)
                yield return x;
        }

        /// <summary>
        /// 주어진 rung 구조를 IL 리스트로 변환한다.  변환이 시작되는 위치
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> Convert()
        {
            var terminal = _rung.Sinks.First();
            var arity = terminal.Arity;
            if (arity < 2)
                return ConvertNormalOutput();

            return ConvertFunctionOutput();
        }

        /// <summary>
        /// Arity 가 0 또는 1 인 경우.  OUT, TIM 등이 해당
        /// </summary>
        private IEnumerable<string> ConvertNormalOutput()
        {
            Debug.Assert(_rung.Sources.Count() == 1);
            var mnemonics = FollowNode(_rung.Sources.First()).ToArray();
            var unvisitedNodes = _rung.Nodes.Where(n => !_visitedNodes.Contains(n)).ToArray();
            if (unvisitedNodes.Any())
            {
                var msg = string.Join(", ", unvisitedNodes.Select(n => n.Name));
                Logger?.Error($"Total {unvisitedNodes.Length} points untranslated.\r\n{msg}");
                Debug.Assert(false);
            }

            return mnemonics;
        }
        private IEnumerable<string> ConvertFunctionOutput()
        {
            Debug.Assert(_rung.Sinks.Count() == 1);
            FunctionNode terminal = _rung.Sinks.First() as FunctionNode;
            var converted = terminal.Convert(_convertParam).ToArray();
            return converted;
        }

        public static string[] Convert(Rung rung, ConvertParams cvtParam) => new Rung2ILConvertor(rung, cvtParam).Convert().ToArray();

        public static string[] ConvertFromMnemonics(string mnemonics, ConvertParams cvtParam)
            => ConvertFromMnemonics(MnemonicInput.MultilineString2Array(mnemonics), cvtParam);
        public static string[] ConvertFromMnemonics(IEnumerable<string> mnemonics, ConvertParams cvtParam)
        {
            var rung = new Rung4Parsing(mnemonics, cvtParam);
            rung.CoRoutineRungParser().ToArray();
            if (rung.ErrorMessage.NonNullAny())
                return new[] { rung.ErrorMessage };

            return new Rung2ILConvertor(rung.ToRung(false), cvtParam).Convert().ToArray();
        }
    }



    internal static class PointExtension
    {
        public static string ToIL(this Point point, PLCVendor targetType) => ILSentence.Create(targetType, point.ILSentence).ToString();

        //public static IEnumerable<string> ToIL(this Point point, PLCVendor targetType)
        //{
        //    switch(targetType)
        //    {
        //        case PLCVendor.LSIS:
        //            yield return new LSILSentence(point.ILSentence).ToString();
        //            yield break;
        //        default:
        //            throw new NotImplementedException("");
        //    }
        //}
    }
}
