using System.Collections.Generic;
using System.Linq;

namespace Dsu.Common.Utilities.Core.ExtensionMethods
{
    public static class EmConsecutive
    {
        private static bool _isConsecutive(dynamic x, dynamic y) => (x == y) || (x == y - 1);

        /// <summary>
        /// http://sagarkhyaju.blogspot.kr/2014/01/split-array-with-linq-to-group.html
        /// </summary>
        public static IEnumerable<List<T>> ToConsecutiveGroups<T>(this IEnumerable<T> source)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (iterator.MoveNext())
                {
                    T current = iterator.Current;
                    List<T> group = new List<T> { current };

                    while (iterator.MoveNext())
                    {
                        T next = iterator.Current;
                        if (!_isConsecutive(current, next))
                        {
                            yield return group;
                            group = new List<T>();
                        }

                        current = next;
                        group.Add(current);
                    }

                    if (group.Any())
                        yield return group;
                }
            }
        }

        //[TestMethod]
        //private void TestConsecutive()
        //{
        //    var listOfInt = new List<int> { 1, 2, 3, 4, 7, 8, 12, 13, 14 };
        //    var result = listOfInt.ToConsecutiveGroups();
        //    ==>  { {1, 2, 3, 4}, {7, 8}, {12, 13, 14} }
        //}

    }
}
