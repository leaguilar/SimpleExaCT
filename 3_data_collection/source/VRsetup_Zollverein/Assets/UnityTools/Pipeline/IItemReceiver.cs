﻿// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         IItemReceiver.cs
// 
// Created:          12.08.2019  19:04
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

namespace UnityTools.Pipeline
{
    /// <summary>
    ///     The ItemReceiver interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IItemReceiver<in T> : IPipelineNode
    {
        /// <summary>
        ///     The add item.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        void AddItem(T item);
    }
}