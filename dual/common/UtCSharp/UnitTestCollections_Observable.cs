using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestCollections_Observable
    {
        static void Data_CollectionChanged(object sender,
         NotifyCollectionChangedEventArgs e)
        {
            Trace.WriteLine("action: {0}", e.Action.ToString());
            if (e.OldItems != null)
            {
                Debug.WriteLine("starting index for old item(s): {0}",
                e.OldStartingIndex);
                Debug.WriteLine("old item(s):");
                foreach (var item in e.OldItems)
                {
                    Debug.WriteLine(item);
                }
            }
            if (e.NewItems != null)
            {
                Debug.WriteLine("starting index for new item(s): {0}",
                e.NewStartingIndex);
                Debug.WriteLine("new item(s): ");
                foreach (var item in e.NewItems)
                {
                    Debug.WriteLine(item);
                }
            }
            Debug.WriteLine("");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var data = new ObservableCollection<string>();
            data.CollectionChanged += Data_CollectionChanged;
            data.Add("One");
            data.Add("Two");
            data.Insert(1, "Three");
            data.Remove("One");
        }
    }
}
