// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         Simple3DBoundsCollection.cs
// 
// Created:          24.08.2019  23:15
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

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityTools.Core;
using UnityEngine;

namespace UnityTools.Collections
{
    /// <summary>
    /// Reference implementation for <see cref="IBounds3DCollection{T}"/>.<br/>
    /// For small amount of items, this collection may very well be the fastest solution.<br/>
    /// This implementation can be used to test other implementations, it is very simple, thus good as a reference.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    public class Simple3DBoundsCollection<T> : IBounds3DCollection<T>
    {
        [NotNull]
        private readonly List<ItemEntry> items;

        private readonly List<T> searchCache;

        public Simple3DBoundsCollection()
        {
            this.items = new List<ItemEntry>();
            this.searchCache = new List<T>();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item.Item;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public int Count => items.Count;

        /// <inheritdoc/>
        public void Add(T item, Bounds bounds)
        {
            if (this.Contains(item, bounds))
            {
                return;
            }

            items.Add(new ItemEntry(item, bounds));
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(T item, Bounds bounds)
        {
            foreach (var entry in items)
            {
                if (entry.Equals(item, bounds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool MoveItem(T item, Bounds oldBounds, Bounds newBounds)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var entry = items[i];
                if (entry.Equals(item, oldBounds))
                {
                    items[i] = new ItemEntry(item, newBounds);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Remove(T item, Bounds bounds)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var entry = items[i];
                if (entry.Equals(item, bounds))
                {
                    items[i] = items[items.Count - 1];
                    items.RemoveAt(items.Count - 1);

                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public T[] FindInRadius(Vector3 center, float radius)
        {
            var sqRad = radius * radius;
            searchCache.Clear();

            foreach (var entry in items)
            {
                var sqrDist = entry.Bounds.SqrDistance(center);
                if (sqrDist <= sqRad)
                {
                    searchCache.Add(entry.Item);
                }
            }

            return searchCache.ToArray();
        }

        /// <inheritdoc/>
        public T[] FindInBounds(Bounds bounds)
        {
            searchCache.Clear();

            foreach (var entry in items)
            {
                if (entry.Bounds.Intersects(bounds))
                {
                    searchCache.Add(entry.Item);
                }
            }

            return searchCache.ToArray();
        }

        private struct ItemEntry
        {
            public readonly T Item;

            public readonly Bounds Bounds;

            public bool Equals(T other, Bounds otherBounds)
            {
                return this.Bounds == otherBounds && Equals(this.Item, other);
            }

            public ItemEntry(T item, Bounds bounds)
            {
                this.Item = item;
                this.Bounds = bounds;
            }
        }
    }
}
