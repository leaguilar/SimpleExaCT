﻿// Solution:         Unity Tools
// Project:          UnityTools
// Filename:         SetMaterial.cs
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
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace UnityTools.Pipeline.Specialized
{
    /// <summary>
    ///     The p p_ set material.
    /// </summary>
    public sealed class SetMaterial : PipelineItemWorker<GameObject>
    {
        /// <summary>
        ///     The material paths.
        /// </summary>
        private readonly string[] materialPaths;

        /// <summary>
        ///     The materials.
        /// </summary>
        [NotNull] private Material[] materials;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetMaterial" /> class.
        /// </summary>
        /// <param name="materials">
        ///     The materials.
        /// </param>
        public SetMaterial(params Material[] materials)
        {
            this.materials = materials ?? Array.Empty<Material>();
            materialPaths = null;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetMaterial" /> class.
        /// </summary>
        /// <param name="materialPaths">
        ///     The material paths.
        /// </param>
        public SetMaterial(params string[] materialPaths)
        {
            materials = Array.Empty<Material>();
            this.materialPaths = materialPaths ?? Array.Empty<string>();
        }

        /// <summary>
        ///     The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (materialPaths != null)
            {
                materials = new Material[materialPaths.Length];

                for (var i = 0; i < materialPaths.Length; i++)
                {
                    var path = materialPaths[i];
                    materials[i] = Resources.Load<Material>(path);
                }
            }
        }

        /// <summary>
        ///     The work on item.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        protected override void WorkOnItem(GameObject item)
        {
            if (item == null)
            {
                return;
            }

            var renderer = item.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterials = materials;
            }
        }
    }
}