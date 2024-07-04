using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using OpenAI;

namespace OpenAI
{
    public class Configuration
    {
        public string ApiKey { get; private set; }

        public Configuration()
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var authPath = Path.Combine(userPath, ".openai/auth.json");
            if (File.Exists(authPath))
            {
                var json = File.ReadAllText(authPath);
                var configData = JsonUtility.FromJson<ConfigData>(json);
                ApiKey = configData.apiKey;
            }
            else
            {
                Debug.LogError("Configuration file not found.");
            }
        }

        [Serializable]
        private class ConfigData
        {
            public string apiKey;
        }
    }
}
