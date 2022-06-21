using System;
using System.Collections.Generic;

namespace Tour_Planner.Extensions
{
    public class MultiDictionary<TKey, TList> : Dictionary<TKey, List<TList>> where TKey : notnull
    {
        #region Private Methods

        // Checks if the key is already exists
        private void EnsureKey(TKey key)
        {
            if (!ContainsKey(key))
            {
                this[key] = new List<TList>(1);
            }
        }

        #endregion


        #region Public Methods

        public void AddValue(TKey key, TList newItem)
        {
            EnsureKey(key);
            this[key].Add(newItem);
        }



        public void AddValues(TKey key, IEnumerable<TList> newItems)
        {
            EnsureKey(key);
            this[key].AddRange(newItems);
        }


        public bool RemoveValue(TKey key, TList value)
        {
            if (!ContainsKey(key))
                return false;

            this[key].Remove(value);

            if (this[key].Count == 0)
                Remove(key);

            return true;
        }

        public bool RemoveAllValue(TKey key, Predicate<TList> match)
        {
            if (!ContainsKey(key))
                return false;

            this[key].RemoveAll(match);

            if (this[key].Count == 0)
                Remove(key);

            return true;
        }

        #endregion
    }
}
