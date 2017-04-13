using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestCovariance
    {
        interface INode { }
        class Node : INode { }
        class NodeLeaf : Node { }

        [TestMethod]
        public void TestMethodCovariance()
        {
            INode inode = null;
            Node node = null;
            inode = node;       // Compiles OK

            {
                IEnumerable<INode> inodes = null;
                IEnumerable<Node> nodes = new List<Node>();
                inodes = nodes;     // Compiles OK : IEnumerable<T> is covariant
            }


            {
                List<INode> inodes = null;
                List<Node> nodes = new List<Node>();
                //inodes = nodes;     // Compile Error!       List<T> is not covariant

                inodes = nodes.Cast<INode>().ToList();          // Require explict casting if covaraince not supported.
            }
        }


        class NodeStem : Node { }

        /// <summary>
        /// pp. 160.  A Programmer's Guide to C# 5.0 4-th edition.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArrayTypeMismatchException))]
        public void TestMethodCovarianceArray()
        {
            Node[] nodes = new Node[2];
            nodes[0] = new NodeLeaf();      // Compile OK
            nodes[1] = new NodeStem();      // Compile OK

            NodeLeaf[] leaves = new NodeLeaf[2];
            nodes = leaves;
            // now, nodes are array of leaves
            nodes[0] = new NodeLeaf();      // Compile OK
            nodes[1] = new NodeStem();      // Compile OK, but Runtime ERROR : ArrayTypeMismatchException

        }



        class Animal
        {
            private static int _count = 0;
            public int Id = _count++;
        }
        class Cat : Animal { }
        class Kitty : Cat { }
        class Dog : Animal { }

        [TestMethod]
        public void TestMethodCat()
        {
            Cat[] cats = new Cat[10];
            Dog[] dogs = new Dog[10];
            Animal[] animals = new Animal[10];

            Dog[] dss;
            Cat[] css;
            Animal[] ass;
            //css = animals;      // ILLEGAL
            ass = cats;        // OK
            ass[0] = new Cat();
            //css[0] = new Animal();  // ILLEGAL

            /*
             * Compiles OK, but... Runtime Exception : 
             * ArrayTypeMismatchException occurred.  배열과 호환되지 않는 형식으로 요소를 액세스하려고 했습니다.
             */
            ass[1] = new Dog();

            Trace.WriteLine("");
        }

        [TestMethod]
        public void TestMethodArrayConstructor()
        {
            Animal[] ass = new Cat[10];
            ass[0] = new Cat();
            ass[1] = new Dog();

            //Cat[] cats = new Animal[10];
        }



        private void DoCatAction(Action<Cat> catAction)
        {
            catAction.Invoke(new Cat());
        }
        /// <summary>
        /// Contravariance
        /// </summary>
        [TestMethod]
        public void TestMethodAction()
        {
            Action<Animal> animalAction = animal =>
            {
                Trace.WriteLine("I am animal " + animal.Id);
            };
            Action<Kitty> kittyAction = kitty =>
            {
                Trace.WriteLine("I am kitty " + kitty.Id);
            };

            DoCatAction(animalAction);

            /*
             * ILLEGAL : error CS1503: Argument 1: cannot convert from 'System.Action<CSharp.Test.UnitTestCovariance.Kitty>' to 'System.Action<CSharp.Test.UnitTestCovariance.Cat>'
             */
            //DoCatAction(kittyAction);
            
            Trace.WriteLine("");
        }
    }
}
