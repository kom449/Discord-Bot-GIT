using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace NewTestBot
{
    public static class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetAlert(string key)
        {
            if(alerts.ContainsKey(key)) return alerts[key];
            return "Wrong Key";
        }
    }
}
