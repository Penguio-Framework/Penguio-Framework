using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine
{
    public static class EnumerableExtensions
    {
        public static T Last<T>(this IEnumerable<T> items)
        {
            var last = default(T);
            foreach (var item in items)
            {
                last = item;
            }
            return last;
        }

        public static T First<T>(this IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                return item;
            }
            return default(T);
        }
        public static List<T> OrderBy<T>(this IEnumerable<T> source, Func<T, int> predicate)
        {
            var items = source.ToArray();
            var keys = items.Select(predicate).ToArray();
//            Array.Sort(keys, items);
            return new List<T>(items);
        }
        public static List<T> OrderByDescending<T>(this IEnumerable<T> source, Func<T, int> predicate)
        {
            var items = source.ToArray();
            var keys = items.Select(a => -predicate(a)).ToArray();
//            Array.Sort(keys, items);
            return new List<T>(items);
        }

        public static List<GroupByItem<T, T2>> GroupBy<T, T2>(this IEnumerable<T> items, Func<T, T2> predicate)
        {
            var ts = new Dictionary<T2, List<T>>();

            foreach (var item in items)
            {
                var j = predicate(item);
                if (!ts.ContainsKey(j))
                {
                    ts.Add(j, new List<T>());
                }
                ts[j].Add(item);
            }
            var ritems = new List<GroupByItem<T, T2>>();

            foreach (var t in ts)
            {
                ritems.Add(new GroupByItem<T, T2>(t.Key, t.Value));
            }
            return ritems;
        }

        public class GroupByItem<T, T2> : IEnumerable<T>
        {
            protected internal GroupByItem(T2 key, List<T> values)
            {
                Key = key;
                Values = values;
            }

            public T2 Key { get; set; }
            public List<T> Values { get; set; }

            public IEnumerator<T> GetEnumerator()
            {
                return Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        public static T First<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public static T[] ToArray<T>(this IEnumerable<T> items)
        {
            var ts = new List<T>();

            foreach (var item in items)
            {
                ts.Add(item);
            }
            return ts.ToArray();
        }

        public static T2[] Select<T, T2>(this T[] items, Func<T, T2> clause)
        {
            var items2 = new List<T2>();

            foreach (var item in items)
            {
                items2.Add(clause(item));
            }
            return items2.ToArray();
        }

        public static List<T2> SelectMany<T, T2>(this List<T> items, Func<T, List<T2>> clause)
        {
            var items2 = new List<T2>();

            foreach (var item in items)
            {
                items2.AddRange(clause(item));
            }
            return items2;
        }

        public static int Count<T>(this T[] items, Func<T, bool> clause)
        {
            var j = 0;
            foreach (var item in items)
            {
                if (clause(item)) j++;
            }
            return j;
        }

        public static T ElementAt<T>(this IEnumerable<T> items, int index)
        {
            var i = 0;
            foreach (var item in items)
            {
                if (i == index)
                {
                    return item;
                }
                i++;
            }
            return default(T);
        }

        public static int IndexOfFast(this List<int> items, int ind)
        {
            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                if (item == ind) return index;
            }
            return -1;
        }

        public static int IndexOfFast(this int[] items, int ind)
        {
            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                if (item == ind) return index;
            }
            return -1;
        }

        public static T[] Where<T>(this T[] items, Func<T, bool> clause)
        {
            var items2 = new List<T>();

            foreach (var item in items)
            {
                if (clause(item))
                {
                    items2.Add(item);
                }
            }
            return items2.ToArray();
        }

        public static T First<T>(this T[] items, Func<T, bool> clause)
        {
            foreach (var item in items)
            {
                if (clause(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public static bool All<T>(this T[] items, Func<T, bool> clause)
        {
            foreach (var item in items)
            {
                if (!clause(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool All<T>(this List<T> items, Func<T, bool> clause)
        {
            foreach (var item in items)
            {
                if (!clause(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Any<T>(this IEnumerable<T> items, Func<T, bool> clause)
        {
            foreach (var item in items)
            {
                if (clause(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Any<T>(this T[] items, Func<T, bool> clause)
        {
            foreach (var item in items)
            {
                if (clause(item))
                {
                    return true;
                }
            }
            return false;
        }


        public static T[] Where<T>(this IEnumerable<T> items, Func<T, bool> clause)
        {
            var items2 = new List<T>();

            foreach (var item in items)
            {
                if (clause(item))
                {
                    items2.Add(item);
                }
            }
            return items2.ToArray();
        }

        public static T2[] Select<T, T2>(this List<T> items, Func<T, T2> clause)
        {
            var items2 = new List<T2>();

            foreach (var item in items)
            {
                items2.Add(clause(item));
            }
            return items2.ToArray();
        }
    }
}