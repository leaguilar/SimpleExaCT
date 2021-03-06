// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         PipelineItemWorker.cs
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
    ///     The pipeline item worker.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class PipelineItemWorker<T> : PipelineWorker<T, T>
    {
        /// <summary>
        ///     The process next item.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool ProcessNextItem()
        {
            if (!HasWaitingItems)
            {
                return false;
            }

            var item = GetNextItem();
            WorkOnItem(item);

            foreach (var output in FollowupSteps)
            {
                output.AddItem(item);
            }

            return true;
        }

        /// <summary>
        ///     The work on item.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        protected abstract void WorkOnItem(T item);
    }
}