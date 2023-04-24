using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    public class Config
    {
        public bool IsActiveAutoInput { get; set; }
        
        private static object _lock = new object();

        private Config() { }

        public static Config Read()
        {
            var config = new Config();

            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(assemblyFolder, Constants.NAME_FILE_CONFIG);

            if(File.Exists(configFile) )
            {
                var json = String.Empty;

                lock (_lock)
                {
                    json = File.ReadAllText(configFile);
                }

                if(String.IsNullOrEmpty(json))
                {
                    config = new Config();
                    config.IsActiveAutoInput = true;
                }
                else
                {
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
            }
            else
            {
                config = new Config();
                config.IsActiveAutoInput = true;
            }

            return config;
        }

        public void Save()
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(assemblyFolder, Constants.NAME_FILE_CONFIG);

            if (!File.Exists(configFile))
            {
                File.Create(configFile);
            }

            var json = JsonConvert.SerializeObject(this);

            lock (_lock)
            {
                File.WriteAllText(configFile, json);
            }
        }

    }
}
