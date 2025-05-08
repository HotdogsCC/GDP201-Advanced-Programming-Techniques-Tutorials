using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public uint maxSize = 100;
    private Stack<T> pool;

    private Func<T> onCreate;
    private Action<T> onPop;
    private Action<T> onReturn;
    private Action<T> onDestroy;

    public ObjectPool(Func<T> _onCreate,
        Action<T> _onPop,
        Action<T> _onReturn,
        Action<T> _onDestroy,
        uint _maxSize = 100)
    {
        InitPool(_onCreate, _onPop, _onReturn, _onDestroy, maxSize);
    }

    public void InitPool(Func<T> _onCreate,
        Action<T> _onPop,
        Action<T> _onReturn,
        Action<T> _onDestroy,
        uint _maxSize = 100)
    {
        pool = pool ?? new Stack<T>(); //checks if the pool exists.
                                       //if it doesn't, create one

        foreach (T obj in pool)        //removes all left over elements in pool
        {
            onDestroy(obj);
        }
        pool.Clear();

        //assignment
        onCreate = _onCreate;
        onPop = _onPop;
        onReturn = _onReturn;
        onDestroy = _onDestroy;
        maxSize = _maxSize;

        //create objects for pool
        for(int i = 0; i < maxSize; i++)
        {
            T obj = onCreate();
            ReturnToPool(obj);
        }
    }

    public T PopFromPool()
    {
        T obj;

        //if there are no items left in the pool, return a new one
        if(pool.Count > 0)
        {
            obj = pool.Pop();
        }
        else
        {
            obj = onCreate();
        }
        onPop(obj);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        onReturn(obj);
        //if returning this will go over the max size, delete the obj
        if(pool.Count >= maxSize)
        {
            onDestroy(obj);
        }
        else
        {
            pool.Push(obj);
        }
    }
}
