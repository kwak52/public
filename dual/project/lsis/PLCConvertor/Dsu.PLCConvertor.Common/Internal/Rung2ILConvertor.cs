using Dsu.Common.Utilities.Core.ExtensionMethods;
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
    public class Rung2ILConvertor
    {
        ILog Logger = Global.Logger;
        Rung _rung;
        internal ConvertParams _convertParam;
        internal PLCVendor TargetType => _convertParam.TargetType;

        /// <summary>
        /// Rung 변환시 발생한 경고/알림 메시지
        /// </summary>
        List<string> _numberedMessages = new List<string>();


        /// <summary>
        /// Rung 변환에 필요한 추가적 사전 IL command set.  %ANDNOT 등의 처리시 필요
        /// </summary>
        public List<string> ProglogRungs = new List<string>();
        /// <summary>
        /// Rung 변환 후, 필요한 추가적 IL command set.
        /// </summary>
        public List<string> EpilogRungs = new List<string>();

        /// <summary>
        /// ONS : Temprorary address allocator
        /// </summary>
        internal Lazy<TemporaryAddressAllocator> TempAddressAllocator =
            new Lazy<TemporaryAddressAllocator>(() => TemporaryAddressAllocator.Dup());

        public IEnumerable<string> GetNumberedMessages() => _numberedMessages;

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


                // Output node 를 비롯한 출력단의 node.  output node 자신을 출력하고 stack 의 내용을 따라간다.
                case UserDefinedFunctionNode udf:
                case ITerminalNode tln:
                    xs = spitResult(node);
                    foreach (var x in xs)
                        yield return x;
                    break;

                // 기본 data node
                default:
                    yield return node.ToIL(this);
                    break;
            }

            if (! followedMe )
            {
                xs = FollowMe().ToArray();
                foreach (var x in xs)
                    yield return x;
            }



            IEnumerable<string> spitResult(Point point)
            {
                var udf = point as UserDefinedFunctionNode;
                var udc = point.ILSentence.ILCommand as UserDefinedILCommand;
                var udcTerminal = udc == null ? false : udc.IsTerminal;
                if (udf != null)
                {
                    if (udc.Message.NonNullAny())
                    {
                        var ss = _convertParam.SourceStartStep;
                        var ts = _convertParam.TargetStartStep;
                        //_numberedMessages.Add($"[{ss+1}] [{ts+1}] [{Cx2Xg5kOption.LabelHeader} {udc.Message}]");     // kkk: 메지지 추가
                        _numberedMessages.Add(udc.Message);     // kkk: 메지지 추가
                    }

                    udf.ILSentence = ILSentence.Create(TargetType, point.ILSentence);

                    var xs2 = udf.EnumeratePerInputs();
                    foreach (var x in xs2)
                        yield return x;
                }
                else
                    yield return point.ToIL(this);
                if (point is ITerminalNode || udcTerminal)
                {
                    var xs2 = FollowEdgeStack();
                    foreach (var x in xs2)
                        yield return x;
                }
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
            IEnumerable<string> xs = null;

            if (_rung.RungComment.NonNullAny())
            {
                xs = _rung.RungComment.Select(rc => ILSentence.Create(TargetType, rc).ToIL());
                foreach (var x in xs)
                    yield return x;
            }

            var terminal = _rung.Sinks.First();
            var arity = terminal.Arity;

            if (arity < 2)
                xs = ConvertNormalOutput();
            else
                xs = ConvertFunctionOutput();

            // ONS : rung 변환을 위해서 앞부분에 추가한 내용
            foreach (var x in ProglogRungs)
                yield return x;

            // 현재 변환 중인 rung contents
            foreach (var x in xs)
                yield return x;


            // ONS : rung 변환 후, 뒷부분에 추가한 내용
            foreach (var x in EpilogRungs)
                yield return x;
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
            TerminalFunctionNode terminal = _rung.Sinks.First() as TerminalFunctionNode;
            var converted = terminal.Convert(_convertParam).ToArray();
            return converted;
        }

        public static ConvertResult Convert(Rung rung, ConvertParams cvtParam)
        {
            var cvr = new Rung2ILConvertor(rung, cvtParam).Convert();
            return new ConvertResult(cvr);
        }

        public static ConvertResult ConvertFromMnemonics(string mnemonics, string rungComment, ConvertParams cvtParam)
            => ConvertFromMnemonics(MnemonicInput.MultilineString2Array(mnemonics), rungComment, cvtParam);

        /// <summary>
        /// Rung 단위의 mnemonics 을 변환
        /// </summary>
        /// <param name="mnemonics">Rung 단위의 mnemonic</param>
        /// <param name="rungComment">rung 단위의 comment</param>
        /// <param name="cvtParam">변환 parameters</param>
        /// <returns></returns>
        public static ConvertResult ConvertFromMnemonics(IEnumerable<string> mnemonics, string rungComment, ConvertParams cvtParam)
        {
            if (Cx2Xg5kOption.ForceRungSplit)
            {
                // rung 단위 변환을 끊을 때에 XGRUNGSTART 로 marking
                var res = convertFromMnemonics();
                var result =
                    new[] { Xg5k.XgRungStart }
                    .Concat(res.Results)
                    .ToArray();

                return new ConvertResult(result, res.Messages);
            }
            else
                return convertFromMnemonics();

            ConvertResult convertFromMnemonics()
            {
                var directlyConverted = tryConvertDirectly();
                if (directlyConverted != null)
                    return directlyConverted;

                var rung4p = new Rung4Parsing(mnemonics, rungComment, cvtParam);
                rung4p.CoRoutineRungParser().ToArray();

                var r2il = new Rung2ILConvertor(rung4p.ToRung(false), cvtParam);
                var result = r2il.Convert().ToArray();

                // kkk: 결과와 error message 를 모두 반환
                return new ConvertResult(result, r2il.GetNumberedMessages().Concat(rung4p.ErrorMessage.Select(em => em.Replace("\r\n", ":::")))); // kkk


                // Rung 생성 없이, 문자열 기준으로 변환
                ConvertResult tryConvertDirectly()
                {
                    var grps = mnemonics.GroupBy(m => m.StartsWith("'"));
                    var cmtsCmds =  // comment commands
                        grps
                        .Where(grp => grp.Key).SelectMany(grp => grp)
                        .Select(cmt => $"{Xg5k.RungCommentCommand}\t{cmt}")
                        .ToArray();

                    // comment 제외한 명령들
                    var ils = grps.Where(grp => ! grp.Key).SelectMany(grp => grp).ToArray();

                    var length = ils.Count();
                    if (length == 1)
                    {
                        var m = ils.First();
                        if (m.StartsWith("NOP("))
                            return new ConvertResult(cmtsCmds.Concat(new[] { "NOP" }));
                        else if (m.StartsWith("END("))
                            return new ConvertResult(cmtsCmds.Concat(new[] { "END" }));
                    }

                    return null;
                }
            }
        }
    }


    public class ConvertResult
    {
        public List<string> Results;
        public List<string> Messages;

        public CxtInfoProgram Program { get; set; }
        public CxtInfoSection Section { get; set; }
        public CxtInfoRung Rung { get; set; }

        public ConvertResult(IEnumerable<string> results, IEnumerable<string> messages)
        {
            Results = results.ToList();
            Messages = messages.ToList();
        }
        public ConvertResult(IEnumerable<string> results)
            : this(results, Enumerable.Empty<string>())
        {}
    }

    internal static class PointExtension
    {
        public static string ToIL(this IPoint point, Rung2ILConvertor r2iConverter)
        {
            var il = ILSentence.Create(r2iConverter, point.ILSentence);
            return il.ToIL();
        }
        //public static string ToIL(this IPoint point, PLCVendor targetType) => ILSentence.Create(targetType, point.ILSentence).ToString();

        //public static IEnumerable<string> ToIL(this Point point, PLCVendor targetType)
        //{
        //    switch(targetType)
        //    {
        //        case PLCVendor.LSIS:
        //            yield return new LSILSentence(point.ILSentence).ToString();
        //            yield break;
        //        default:
        //            throw new ConvertorException("");
        //    }
        //}
    }
}
