using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace LOLS.Modules
{
    class ConfigHandler
    {
        struct Config
        {
            public string token;
            public string prefix;
        }

        private Config conf;
        private string configPath, line;

        public string Token
        {
            get
            {
                return conf.token;
            }
        }

        public string Prefix
        {
            get
            {
                return conf.prefix;
            }
        }

        public ConfigHandler()
        {
            conf = new Config()
            {
                token = "",
                prefix = "!"
            };
        }

        public async Task PopulateConfig()
        {
            configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json").Replace(@"\", @"\\");
            await Logger.Log($"info {configPath}");

            if (!File.Exists(configPath))
            {
                using (StreamWriter sw = File.AppendText(configPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(conf));
                }

                await Logger.Log("warn New Config initialized! Need to fill in values before running commands!");
                throw new Exception("NO CONFIG AVAILABLE! Go to executable path and fill out newly created file!");
            }

            using (StreamReader reader = new StreamReader(configPath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    conf = JsonConvert.DeserializeObject<Config>(line);
                }

                await Task.CompletedTask;
            }
        }
    }
}
