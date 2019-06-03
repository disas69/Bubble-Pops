using System;
using UnityEngine;

namespace Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ResourcePathAttribute : Attribute
    {
        public string Filepath { get; private set; }

        public ResourcePathAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path! (its null or empty)");
            }

            Filepath = path;
        }
    }
}