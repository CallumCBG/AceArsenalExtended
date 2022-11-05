﻿using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Helper;

namespace HelperUI
{
    internal class MetadataService
    {
        private string? currentFile;

        private MetadataRoot? metadata;

        internal ModelMetadata GetMetadataFor(DetectedModelInfo detectedConfigInfo)
        {
            if (metadata == null)
            {
                throw new InvalidOperationException();
            }
            var model = metadata.Models.FirstOrDefault(m => m.P3dModel == detectedConfigInfo.P3dModel && m.ClassRoot == detectedConfigInfo.ClassRoot);
            if( model == null)
            {
                model = metadata.Models.FirstOrDefault(m => detectedConfigInfo.Configs.Any(c => c.ClassName == m.FirstConfigClassName) && m.ClassRoot == detectedConfigInfo.ClassRoot);
                if (model == null)
                {
                    model = new ModelMetadata()
                    {
                        P3dModel = detectedConfigInfo.P3dModel,
                        ClassRoot = detectedConfigInfo.ClassRoot,
                        Name = detectedConfigInfo.Name,
                        FirstConfigClassName = detectedConfigInfo.Configs.First().ClassName
                    };
                    metadata.Models.Add(model);
                }
            }
            return model;
        }

        internal void Initialize(string aceaxMetadataFile)
        {
            currentFile = aceaxMetadataFile;
            if (File.Exists(currentFile))
            {
                metadata = JsonSerializer.Deserialize<MetadataRoot>(File.ReadAllText(currentFile)) ?? new MetadataRoot();
            }
            else
            {
                metadata = new MetadataRoot();
            }
        }

        internal void Save()
        {
            if (!string.IsNullOrEmpty(currentFile))
            {
                File.WriteAllText(currentFile, JsonSerializer.Serialize(metadata, new JsonSerializerOptions() { WriteIndented = true }));
            }
        }
    }
}