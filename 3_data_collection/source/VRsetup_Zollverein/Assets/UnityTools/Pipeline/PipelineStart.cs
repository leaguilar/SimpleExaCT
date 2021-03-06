// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         PipelineStart.cs
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
    ///     The pipeline start.
    /// </summary>
    /// <typeparam name="TOut">
    /// </typeparam>
    public abstract class PipelineStart<TOut> : PipelineBase<TOut>
    {
        /// <summary>
        ///     The process next item.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool ProcessNextItem()
        {
            if (!HasItems())
            {
                return false;
            }

            var item = GetNextItem();

            foreach (var output in FollowupSteps)
            {
                output.AddItem(item);
            }

            return true;
        }

        /// <summary>
        ///     The get next item.
        /// </summary>
        /// <returns>
        ///     The <see cref="TOut" />.
        /// </returns>
        protected abstract TOut GetNextItem();

        /// <summary>
        ///     The has items.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected abstract bool HasItems();
    }
}