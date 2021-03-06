// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         PoolBase.cs
// 
// Created:          29.01.2020  19:32
// Last modified:    05.02.2020  19:39
// 
// --------------------------------------------------------------------------------------
// 
// MIT License
// 
// Copyright (c) 2019 chillersanim
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityTools.Core;

namespace UnityTools.Pooling
{
    public abstract class PoolBase<T> : IPool<T> where T : class
    {
        public int MaxSize
        {
            get => maxSize;
            set
            {
                if (maxSize == value) return;

                maxSize = value;

                if (maxSize < 1)
                {
                    maxSize = 1;
                }

                while (items.Count > maxSize)
                {
                    var item = items.Pop();
                    if (ImplementsDisposable) 
                    {
                        ((IDisposable)item).Dispose();
                    }
                }
            }
        } // ReSharper disable StaticMemberInGenericType

        private static readonly bool ImplementsReusable;

        private static readonly bool ImplementsDisposable;
        // ReSharper enable StaticMemberInGenericType

        private readonly Stack<T> items;
        private int maxSize;

        static PoolBase()
        {
            ImplementsReusable = typeof(IReusable).IsAssignableFrom(typeof(T));
            ImplementsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(T));
        }

        protected PoolBase()
        {
            items = new Stack<T>();
            MaxSize = 128;
        }

        [NotNull]
        public T Get()
        {
            return items.Count > 0 ? items.Pop() : CreateItem();
        }

        public void Put([NotNull]T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (items.Count <= MaxSize)
            {
                if (ImplementsDisposable)
                {
                    ((IDisposable)item).Dispose();
                }

                return;
            }

            if (ImplementsReusable)
            {
                ((IReusable)item).Reuse();
            }

            items.Push(item);
        }

        protected abstract T CreateItem();
    }
}
