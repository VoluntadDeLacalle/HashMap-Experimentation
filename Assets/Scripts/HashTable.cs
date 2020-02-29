using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class helps in building the HashMap class by creating the nodes that the hashed values will be stored in.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
class HashNode<K, V>
{
    private K key;
    private V value;
    private HashNode<K, V> next;

    public HashNode(K keyCon, V valueCon)
    {
        key = keyCon;
        value = valueCon;
        next = null;
    }

    public K getKey()
    {
        return key;
    }

    public V getValue()
    {
        return value;
    }

    public void setValue(V valueVar)
    {
        value = valueVar;
    }

    public HashNode<K, V> getNext()
    {
        return next;
    }

    public void setNext(HashNode<K, V> nextVar)
    {
        next = nextVar;
    }

}

/// <summary>
/// The hashing function for the HashMap class. The hashFunction takes in a key variable and returns an unisigned long
/// that is the hash ID for said key.
/// </summary>
/// <typeparam name="K"></typeparam>
[Serializable]
public class KeyHash<K>
{
    public ulong hashFunction(K key, int TABLE_SIZE)
    {
        return (ulong)(Mathf.Abs(key.GetHashCode()) % TABLE_SIZE);
    }
};

/// <summary>
/// This is the HashMap class.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
[Serializable]
public class HashMap<K, V>
{
    private HashNode<K, V>[] table;
    private KeyHash<K> hashFunc;
    private List<K> keys;
    private int TABLE_SIZE;

    public HashMap(int n)
    {
        TABLE_SIZE = n;
        table = new HashNode<K, V>[TABLE_SIZE];
        hashFunc = new KeyHash<K>();
        keys = new List<K>();
    }

    ~HashMap()
    {
        for (int i = 0; i < TABLE_SIZE; ++i)
        {
            HashNode<K, V> entry = table[i];
            while (entry != null)
            {
                HashNode<K, V> prev = entry;
                entry = entry.getNext();
                prev = null;
            }
            table[i] = null;
        }
        table = null;
        keys.Clear();
    }


    /// <summary>
    /// Sets the table size for the HashMap. If the table size set in the constructor needs to change, it can be done
    /// using this function.
    /// </summary>
    /// <param name="n"></param>
    public void setTableSize(int n)
    {
        TABLE_SIZE = n;
    }

    /// <summary>
    /// Takes in a key variable and returns whatever the value that the key variable is paired with.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public V get(K key)
    {
        ulong hashValue = hashFunc.hashFunction(key, TABLE_SIZE);
        HashNode<K, V> entry = table[hashValue];

        while (entry != null)
        {
            if (EqualityComparer<K>.Default.Equals(entry.getKey(), key))
            {
                return entry.getValue();
            }
            entry = entry.getNext();
        }

        return default(V);
    }

    /// <summary>
    /// Takes in a key variable and returns either true or false depending on if the key was found in the HashMap or not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool find(K key)
    {
        ulong hashValue = hashFunc.hashFunction(key, TABLE_SIZE);
        HashNode<K, V> entry = table[hashValue];

        while (entry != null)
        {
            if (EqualityComparer<K>.Default.Equals(entry.getKey(), key))
            {
                return true;
            }

            entry = entry.getNext();
        }

        return false;
    }


    /// <summary>
    /// This function allows a new key-value pair to be added to the HashMap.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void put(K key, V value)
    {
        ulong hashValue = hashFunc.hashFunction(key, TABLE_SIZE);
        HashNode<K, V> prev = null;
        HashNode<K, V> entry = table[hashValue];

        while (entry != null && !EqualityComparer<K>.Default.Equals(entry.getKey(), key))
        {
            prev = entry;
            entry = entry.getNext();
        }

        if (entry == null)
        {
            entry = new HashNode<K, V>(key, value);
            keys.Add(key);
            if (prev == null)
            {
                table[hashValue] = entry;
            }
            else
            {
                prev.setNext(entry);
            }
        }
        else
        {
            entry.setValue(value);
        }
    }

    /// <summary>
    /// Returns all of the keys that are currently stored in the HashMap.
    /// </summary>
    /// <returns></returns>
    public List<K> getKeys()
    {
        return keys;
    }

    /// <summary>
    /// Takes in a key variable and removes the key-value pair associated from the HashMap.
    /// </summary>
    /// <param name="key"></param>
    public void remove(K key)
    {
        ulong hashValue = hashFunc.hashFunction(key, TABLE_SIZE);
        HashNode<K, V> prev = null;
        HashNode<K, V> entry = table[hashValue];

        while (entry != null && !EqualityComparer<K>.Default.Equals(entry.getKey(), key))
        {
            prev = entry;
            entry = entry.getNext();
        }

        if (entry == null)
        {
            keys.Remove(key);
            return;
        }
        else
        {
            if (prev == null)
            {
                table[hashValue] = entry.getNext();
            }
            else
            {
                prev.setNext(entry.getNext());
            }

            keys.Remove(key);
            entry = null;
        }
    }
}
