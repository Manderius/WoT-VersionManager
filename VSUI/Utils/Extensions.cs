using System.Collections;

namespace VSUI.Utils
{
    public static class HashtableExtentions
    {
        public static V GetOrInsert<K, V>(this Hashtable hashtable, K key, V value)
        {
            if (!hashtable.ContainsKey(key))
            {
                hashtable.Add(key, value);
            }
            return (V) hashtable[key];
        }
    }
}
