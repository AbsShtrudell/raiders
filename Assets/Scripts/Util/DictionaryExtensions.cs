using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Raiders.Util.Collections
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> ConverListPair<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, List<Pair<TKey, TValue>> pairList)
        {
            dictionary.Clear();

            foreach (var pair in pairList)
                dictionary.Add(pair.First, pair.Second);

            return dictionary;
        }
    }
}
