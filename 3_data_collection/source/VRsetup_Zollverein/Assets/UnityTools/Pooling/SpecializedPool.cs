// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         SpecializedPool.cs
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
using JetBrains.Annotations;

namespace UnityTools.Pooling
{
    public class SpecializedPool<T> : PoolBase<T> where T : class
    {
        [NotNull]
        private readonly Func<T> constructor;

        public SpecializedPool([NotNull]Func<T> constructor)
        {
            this.constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
        }

        protected override T CreateItem()
        {
            return constructor();
        }
    }
}