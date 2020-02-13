using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HashNode<K, V>
{
    private K key;
    private V value;
    private HashNode<K, V>* next;

    public HashNode(K keyCon, V valueCon) {
        key = keyCon;
        value = valueCon;
        next = null;
    }

    public K getKey() {
        return key;
    }

    public V getValue()
    {
        reutrn value;
    }

    public void setValue(V value)
    {
        HashNode::value = value;
    }

    public HashNode<K, V>* getNext()
    {
        return next;
    }

    public void setNext(HashNode<K, V>* next)
    {
        HashNode::next = next;
    }

};


[Serializable]
public class KeyHash<K>
{
    public ulong hashFunction(K key)
    {
        return (ulong)(key) % TABLE_SIZE;
    }
};

public class HashMap<K, V, F> where F : KeyHash<K>
{
    private HashNode<K, V>** table;
    private F hashFunc;

    public HashMap()
    {
        table = new HashNode<K, V> *[TABLE_SIZE];
    }

    ~HashMap()
    {
        for (int i = 0; i < TABLE_SIZE; ++i)
        {
            HashNode<K, V>* entry = table[i];
            while (entry != null)
            {
                HashNode<K, V>* prev = entry;
                entry = entry->getNext();
                prev = null;
            }
            table[i] = null;
        }
        table = null;
    }

    public bool get(K key, V value)
    {
        ulong hashValue = hashFunc.hashFunction(key);
        HashNode<K, V>* entry = table[hashValue];

        while (entry != null)
        {
            if (entry->getKey() == key)
            {
                value = entry->getValue();
                return true;
            }
            entry = entry->getNext();
        }

        return false;
    }

    public void put(K key, V value)
    {
        ulong hashValue = hashFunc.hashFunction(key);
        HashNode<K, V>* prev = null;
        HashNode<K, V>* entry = table[hashValue];

        while (entry != null && entry->getKey() != key)
        {
            prev = entry;
            entry = entry->getNext();
        }

        if (entry == null)
        {
            entry = new HashNode<K, V>(key, value);
            if (prev == null)
            {
                table[hashValue] = entry;
            }
            else
            {
                prev->setNext(entry);
            }
        }
        else
        {
            entry->setValue(value);
        }
    }

    public void remove(K key)
    {
        ulong hashValue = hashFunc.hashFunction(key);
        HashNode<K, V>* prev = null;
        HashNode<K, V>* entry = table[hashValue];

        while (entry != null && entry->getKey() != key)
        {
            prev = entry;
            entry = entry->getNext();
        }

        if (entry == null)
        {
            return;
        }
        else
        {
            if (prev == null)
            {
                table[hashValue] = entry->getNext();
            }
            else
            {
                prev->setNext(entry->getNext());
            }
            entry = null;
        }
    }
};