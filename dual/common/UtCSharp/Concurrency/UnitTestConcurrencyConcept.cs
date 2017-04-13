using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test.Concurrency.TaskParallel
{
    [TestClass]
    public class UnitTestConcurrencyConcept
    {
        /*
Here is how Bob cooks dinner:

    He fills a pot of water, and boils it.
    He then puts pasta in the water.
    He drains the pasta when its done.
    He prepares the ingredients for his sauce.
    He puts all of the ingredients for his sauce in a saucepan.
    He cooks his sauce.
    He puts his sauce on his pasta.
    He eats dinner.

Bob has cooked entirely synchronously with no multithreading, asynchrony, or parallelism when cooking his dinner.

Here is how Jane cooks dinner:

    She fills a pot of water and starts boiling it.
    She prepares the ingredients for her sauce.
    She puts the pasta in the boiling water.
    She puts the ingredients in the saucepan.
    She drains her pasta.
    She puts the sauce on her pasta.
    She eats her dinner.

Jane leveraged asynchronous cooking (without any multithreading) to achieve parallelism when cooking her dinner.

Here is how Servy cooks dinner:

    He tells Bob to boil a pot of water, put in the pasta when ready, and serve the pasta.
    He tells Jane to prepare the ingredients for the sauce, cook it, and then serve it over the pasta when done.
    He waits for Bob and Jane to finish.
    He eats his dinner.

Servy leveraged multiple threads (workers) who each individually did their work synchronously, but who worked asynchronously with respect to each other to achieve parallelism.

Of course, this becomes all the more interesting if we consider, for example, whether our stove has two burners or just one. If our stove has two burners then our two threads, Bob and Jane, are both able to do their work without getting in each others way, much. They might bump shoulders a bit, or each try to grab something from the same cabinet every now and then, so they'll each be slowed down a bit, but not much. If they each need to share a single stove burner though then they won't actually be able to get much done at all whenever the other person is doing work. In that case, the work won't actually get done any faster than just having one person doing the cooking entirely synchronously, like Bob does when he's on his own. In this case we are cooking with multiple threads, but our cooking isn't parallelized. Not all multithreaded work is actually parallel work. This is what happens when you are running multiple threads on a machine with one CPU. You don't actually get work done any faster than just using one thread, because each thread is just taking turns doing work. (That doesn't mean multithreaded programs are pointless on one cores CPUs, they're not, it's just that the reason for using them isn't to improve speed.)

We can even consider how these cooks would do their work using the Task Parallel Library, to see what uses of the TPL correspond to each of these types of cooks:

So first we have bob, just writing normal non-TPL code and doing everything synchronously:
         */
#if false
        /// <summary>
        /// http://stackoverflow.com/questions/23833255/does-using-tasks-tpl-library-make-an-application-multithreaded
        /// </summary>
        public class Bob : ICook
        {
            public IMeal Cook()
            {
                Pasta pasta = PastaCookingOperations.MakePasta();
                Sauce sauce = PastaCookingOperations.MakeSauce();
                return PastaCookingOperations.Combine(pasta, sauce);
            }
        }

        public class Jane : ICook
        {
            public IMeal Cook()
            {
                Task<Pasta> pastaTask = PastaCookingOperations.MakePastaAsync();
                Task<Sauce> sauceTask = PastaCookingOperations.MakeSauceAsync();
                return PastaCookingOperations.Combine(pastaTask.Result, sauceTask.Result);
            }
        }

        public class Servy : ICook
        {
            public IMeal Cook()
            {
                var bobsWork = Task.Run(() => PastaCookingOperations.MakePasta());
                var janesWork = Task.Run(() => PastaCookingOperations.MakeSauce());
                return PastaCookingOperations.Combine(bobsWork.Result, janesWork.Result);
            }
        }
#endif


        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
