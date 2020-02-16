using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

};


[Serializable]
public class KeyHash<K>
{
    public ulong hashFunction(K key, int TABLE_SIZE)
    {
        return (ulong)(Mathf.Abs(key.GetHashCode()) % TABLE_SIZE);
    }
};

[Serializable]
public class HashMap<K, V> : MonoBehaviour
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

    public void setTableSize(int n)
    {
        TABLE_SIZE = n;
    }

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
            Debug.Log(key);
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

    public List<K> getKeys()
    {
        return keys;
    }

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
            entry = null;
        }
    }
};

public class HashTable : MonoBehaviour
{

}
