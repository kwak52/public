using System;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.TaskParallel.Dataflow
{
#if false
    public interface IDataflowBlock
    {
        void Complete();
        void Fault(Exception error);
        Task Completion { get; }
    }

    public interface ISourceBlock<out TOutput> : IDataflowBlock
    {
        bool TryReceive(out TOutput item, Predicate<TOutput> filter);
        bool TryReceiveAll(out IList<TOutput> items);
        IDisposable LinkTo(ITargetBlock<TOutput> target, bool unlinkAfterOne);
        bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
        TOutput ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed);
        void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
    }
    public interface ITargetBlock<in TInput> : IDataflowBlock
    {
        DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept);
    }

    public interface IPropagatorBlock<in TInput, out TOutput> : ITargetBlock<TInput>, ISourceBlock<TOutput>
    {        
    }

    public interface IReceivableSourceBlock<TOutput> : ISourceBlock<TOutput>
    {
        bool TryReceive(out TOutput item, Predicate<TOutput> filter);
        bool TryReceiveAll(out IList<TOutput> items);
    }
#endif
    [TestClass]
    public class UnitTestTDFInterface
    {
        [TestMethod]
        public void TestMethodTDF()
        {
            //IReceivableSourceBlock<string>

            //IDataflowBlock dataflow = null;
            //ISourceBlock<int> source = null;
            //ITargetBlock<int> target = null;

            var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
            var subtractBlock = new TransformBlock<int, int>(item => item - 2);
            IDisposable link = multiplyBlock.LinkTo(subtractBlock);
            multiplyBlock.Post(1);
            multiplyBlock.Post(2);
            link.Dispose();
            
            
            // TransformBlock
            // TransformManyBlock
            // ActionBlock
        }
    }
}
