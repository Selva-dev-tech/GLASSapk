using System.IO;
using UnityEngine;

namespace WorkflowSystem
{
    public class LocalJsonLoader : IJsonLoader
    {
        private string filePath;

        public LocalJsonLoader(string fileName)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        }

        public string LoadJson()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("JSON file not found at path: " + filePath);
                return null;
            }
        }
    }
}
