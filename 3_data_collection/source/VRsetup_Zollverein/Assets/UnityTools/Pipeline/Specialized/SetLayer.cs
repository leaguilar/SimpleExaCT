// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         SetLayer.cs
// 
// Created:          12.08.2019  19:05
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

#region usings

using System;
using UnityEngine;

#endregion

namespace UnityTools.Pipeline.Specialized
{
    /// <summary>
    ///     The p p_ set layer.
    /// </summary>
    public sealed class SetLayer : PipelineItemWorker<GameObject>
    {
        /// <summary>
        ///     The layer name.
        /// </summary>
        private readonly string layerName;

        /// <summary>
        ///     The layer.
        /// </summary>
        private int layer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetLayer" /> class.
        /// </summary>
        /// <param name="layer">
        ///     The layer.
        /// </param>
        public SetLayer(int layer)
        {
            layerName = LayerMask.LayerToName(layer);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetLayer" /> class.
        /// </summary>
        /// <param name="layerName">
        ///     The layer name.
        /// </param>
        public SetLayer(string layerName)
        {
            this.layerName = layerName;
        }

        /// <summary>
        ///     The initialize.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public override void Initialize()
        {
            base.Initialize();

            if (string.IsNullOrEmpty(layerName))
            {
                throw new ArgumentException("The layer name cannot be null or empty.");
            }

            var foundLayer = LayerMask.NameToLayer(layerName);

            if (foundLayer == -1)
            {
                throw new ArgumentException("There is no layer with the given layer name.");
            }

            layer = foundLayer;
        }

        /// <summary>
        ///     The work on item.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        protected override void WorkOnItem(GameObject item)
        {
            item.layer = layer;
        }
    }
}