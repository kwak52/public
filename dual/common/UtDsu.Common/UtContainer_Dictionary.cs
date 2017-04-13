using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtContainerDictionary
    {
        [TestMethod]
        public void TestMethodContainer_Dictionary()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>()
            {
                {1, "One"},
                {2, "Two"},
                {3, "Three"},
                {4, "Four"},
                {5, "Five"},
                {6, "Six"},
            };

            var evens = dict.Where(x => x.Key%2 == 0);
            List<int> evenKeys = evens.Select(x => x.Key).ToList();
            List<string> evenValues = evens.Select(x => x.Value).ToList();

            Assert.AreEqual(evenKeys.Count, 3);
            Assert.AreEqual(evenValues.Count, 3);
            Assert.IsTrue(evenKeys.Contains(2) && evenKeys.Contains(4) && evenKeys.Contains(6));
            Assert.IsTrue(evenValues.Contains("Two") && evenValues.Contains("Four") && evenValues.Contains("Six"));
        }

        [TestMethod]
        public void TestMethodContainer_DictionaryLinq()
        {
            //initialize a dictionary with keys and values.    
            Dictionary<int, string> plants = new Dictionary<int, string>()
            {
                {1, "Speckled Alder"},
                {2, "Apple of Sodom"},
                {3, "Hairy Bit"},
                {4, "Pennsylvania Blackberry"},
                {5, "Apple of Sodom"},
                {6, "Water Birch"},
                {7, "Meadow Cabbage"},
                {8, "Water Birch"}
            };

            DEBUG.WriteLine("dictionary elements........");

            //loop dictionary all elements   
            foreach (KeyValuePair<int, string> pair in plants)
            {
                DEBUG.WriteLine(pair.Key + "....." + pair.Value);
            }

            //find dictionary duplicate values.  
            var duplicateValues = plants.GroupBy(x => x.Value).Where(x => x.Count() > 1);

            DEBUG.WriteLine("dictionary duplicate values..........");

            //loop dictionary duplicate values only            
            foreach (var item in duplicateValues)
            {
                DEBUG.WriteLine(item.Key);
            }
        }

    }
}
