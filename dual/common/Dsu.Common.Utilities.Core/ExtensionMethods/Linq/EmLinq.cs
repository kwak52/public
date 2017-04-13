﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    /// <summary>
    /// Linq extension methods
    /// </summary>
    public static partial class EmLinq
    {
        // http://stackoverflow.com/questions/1883920/call-a-function-for-each-value-in-a-generic-c-sharp-collection
        /// <summary>
        /// Generic IEnumerable ForEach extension : typename T is optional.  deducible from source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static void Iter<T>(this IEnumerable<T> source, Action<T> action) => ForEach(source, action);
        public static void Iter<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in source)
                action(item, i++);
        }

        /// <summary>
        /// Lazy Foreach : Not evaluated until on demand
        /// </summary>
        public static IEnumerable<T> ForEachTee<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Non-Generic IEnumerable ForEach extension : typename T should be provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        /// <summary>
        /// Non-Generic IEnumerable ForEach extension : typename T should be provided.
        /// Lazy Foreach : Not evaluated until on demand
        /// </summary>
        public static IEnumerable ForEachTee<T>(this IEnumerable source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Non-Generic IEnumerable Select extension : typename T should be provided.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectEx<TSource, TResult>(this IEnumerable source, Func<TSource, TResult> selector)
        {
            foreach (TSource item in source)
                yield return selector(item);
        }

        /// <summary>
        /// TResult type 의 enumerable 중에서 TNotCheckType type 이 아닌 것들만 골라서 반환한다. (System.Linq.OfType 의 negation)
        /// <para/> System.Linq.SkipWhile() 구문과 같은 역할
        /// <para/> TNotCheck 는 TResult type 이어야 함.
        /// </summary>
        /// <typeparam name="TResult">enumerable 들의 base type.  동시에 반환될 enumerable 의 type</typeparam>
        /// <typeparam name="TNotCheckType">제외할 type</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> OfNotType<TResult, TNotCheckType>(this IEnumerable source) where TNotCheckType : TResult
        {
            if (source == null) throw new ArgumentNullException("source");
            return OfNotTypeIterator<TResult, TNotCheckType>(source);
        }


        private static IEnumerable<TResult> OfNotTypeIterator<TResult, TNotCheckType>(IEnumerable source)
        {
            foreach (object obj in source)
            {
                if (!(obj is TNotCheckType))
                    yield return (TResult)obj;
            }
        }

		/// <summary>
		/// Select Non null element from enumerable
		/// </summary>
		public static IEnumerable<TResult> OfNotNull<TResult>(this IEnumerable<TResult> source) where TResult : class 
		{
			if (source == null) throw new ArgumentNullException("source");
			foreach (var s in source)
			{
				if ( s != null )
					yield return s;
			}
		}

		// http://stackoverflow.com/questions/2471588/how-to-get-index-using-linq
		public static Nullable<int> FindIndex<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return index;
                index++;
            }

            return null;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var hash = new HashSet<T>();
            items.ForEach(i => hash.Add(i));
            return hash;
        }



        /// <summary>
        /// 두개의 set 이 동일한지 비교.  see SequenceEqual
        /// </summary>
        public static bool SetEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            HashSet<T> firstSet = first.ToHashSet();
            foreach (var e in second)
            {
                if (!firstSet.Contains(e))
                    return false;
            }

            HashSet<T> secondSet = second.ToHashSet();
            foreach (var e in first)
            {
                if (!secondSet.Contains(e))
                    return false;
            }

            return true;
        }


        public static bool RemoveTail(this IList list)
        {
            if (list.IsNullOrEmpty())
                return false;

            list.RemoveAt(list.Count - 1);
            return true;
        }

        public static T[] ConcatArrays<T>(params T[][] list)
        {
            var result = new T[list.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < list.Length; x++)
            {
                list[x].CopyTo(result, offset);
                offset += list[x].Length;
            }
            return result;
        }


		private static Tuple<bool, T> ExtractFirst<T>(this IEnumerable<T> seq)
        {
            using (var enumerator = seq.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    return Tuple.Create(true, enumerator.Current);      // => return new Tuple<bool, T>(..) 와 동일

                return Tuple.Create(false, default(T));
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/4354902/check-that-all-items-of-ienumerablet-has-the-same-value-using-linq
        /// </summary>
        public static bool AllEqual<T>(this IEnumerable<T> source, T target)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    // empty case
                    return true;
                }

                var comparer = EqualityComparer<T>.Default;

                do
                {
                    if (!comparer.Equals(target, enumerator.Current))
                        return false;
                } while (enumerator.MoveNext());

                return true;
            }
        }

        public static bool AllEqual<T>(this IEnumerable<T> source)
        {
            var pr = source.ExtractFirst();
            if (pr.Item1)
                return AllEqual(source, pr.Item2);

            // empty case
            return true;
        }

        public static T Tee<T>(this T input, Action action)
        {
            action();
            return input;
        }
        public static T Tee<T>(this T input, Action<T> action)
        {
            action(input);
            return input;
        }

		public static bool ForAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (var s in source)
			{
				if (!predicate(s))
					return false;
			}

			return true;
		}

        public static bool ForAll<T>(this IEnumerable<T> source, Func<T, int, bool> predicate)
        {
            int i = 0;
            foreach (var s in source)
            {
                if (!predicate(s, i++))
                    return false;
            }

            return true;
        }


        public static bool NoForAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (var s in source)
			{
				if (predicate(s))
					return false;
			}

			return true;
		}


		/// <summary>
		/// source 를 n 개씩 분할한 sequence 를 반환
		/// http://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq
		/// </summary>
		public static IEnumerable<IEnumerable<T>> SplitByN<T>(this IEnumerable<T> source, int n)
	    {
			return source
				.Select((x, i) => new { Index = i, Value = x })
				.GroupBy(x => x.Index / n)
				.Select(x => x.Select(v => v.Value))
				;
		}

        public static IEnumerable<T> EveryNth<T>(this IEnumerable<T> source, int n)
        {
            return source.Where((v, i) => i % n == 0);
        }


        public static IEnumerable<TResult> Zip2<TS1, TS2, TResult>(
			this IEnumerable<TS1> s1,
		    IEnumerable<TS2> s2,
			Func<TS1, TS2, TResult> resultSelector) => s1.Zip(s2, resultSelector);

	    public static IEnumerable<TResult> Zip3<TS1, TS2, TS3, TResult>(
			this IEnumerable<TS1> s1,
			IEnumerable<TS2> s2,
			IEnumerable<TS3> s3,
			Func<TS1, TS2, TS3, TResult> resultSelector)
		{
			using (var e1 = s1.GetEnumerator())
			using (var e2 = s2.GetEnumerator())
			using (var e3 = s3.GetEnumerator())
			{
				while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
					yield return resultSelector(e1.Current, e2.Current, e3.Current);
			}
	    }

		public static IEnumerable<TResult> Zip4<TS1, TS2, TS3, TS4, TResult>(
			this IEnumerable<TS1> s1,
			IEnumerable<TS2> s2,
			IEnumerable<TS3> s3,
			IEnumerable<TS4> s4,
			Func<TS1, TS2, TS3, TS4, TResult> resultSelector)
		{
			using (var e1 = s1.GetEnumerator())
			using (var e2 = s2.GetEnumerator())
			using (var e3 = s3.GetEnumerator())
			using (var e4 = s4.GetEnumerator())
			{
				while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
					yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
			}
		}


	    public static IEnumerable<int> MinMaxRange(int min, int hop, int max)
	    {
			if (hop <= 0)
				throw new ArgumentException("hop counter should be positive.");

		    for (int i = min; i <= max; i += hop)
			    yield return i;
	    }

	    public static IEnumerable<int> MinMaxRange(int min, int max) => Enumerable.Range(min, max - min);

	    public static IEnumerable<string> HexRange(int start, int count)
	    {
		    foreach (var h in Enumerable.Range(start, count))
		    {
			    yield return $"{h:X}";
		    }
	    }


        /// <summary>
        /// Enumerable 의 laziness 를 강제로 실행시켜 evaluation 시킴.
        /// </summary>
        public static IEnumerable<T> Realize<T>(this IEnumerable<T> seq)
        {
            //var count = seq.Count();      // count 만으로는, 즉석 evaluation 이 안되는 경우가 존재...???...
            var array = seq.ToArray();
            return array;
        }

	}

}
