using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exfal
{
    public class Asset
    {
        public static ContentManager Content { get; set; }

        protected Asset() { }

        public static T Load<T>(string path) => Content.Load<T>(path);
        public static List<Asset<T>> LoadFolder<T>(string folder)
        {
            string folderPath = GetFolderPath(folder);

            string[] filePaths = Directory.GetFiles(folderPath, "*.xnb");

            List<Asset<T>> assets = new();
            foreach (string filePath in filePaths)
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                string path = Path.Combine(folder, name);

                assets.Add(new Asset<T>(path));
            }

            return assets;
        }
        
        private static string GetFolderPath(string folder)
        {
            return Path.Combine(Environment.CurrentDirectory, Content.RootDirectory, folder);
        }
    }

    public class Asset<T> : Asset
    {
        public T Value { get; } 
        public string Name { get; }
        public string FilePath { get; }

        public Asset(string path)
        {
            FilePath = path;
            Name = Path.GetFileNameWithoutExtension(path);
            Value = Load<T>(path);
        }

        public static implicit operator T(Asset<T> asset) => asset.Value;
    }
}