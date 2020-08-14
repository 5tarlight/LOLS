using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LOLS.Modules
{
    public class Logger
    {
        public static Task Log (LogMessage msg)
        {
            Perform(msg.ToString());
            return Task.CompletedTask;
        }

        public static Task Log (string msg)
        {
            Perform(msg);
            return Task.CompletedTask;
        }

        private static void Perform(string msg)
        {
            string type = msg.Split(' ')[0].ToLower();
            string[] types = { "info", "warn", "error", "log", "debug", "success" };
            bool isTypeGiven = types.Contains(type);
            string value;

            string day = DateTime.Today.ToString("dddd");
            string year = DateTime.Now.Year.ToString("0000");
            string month = DateTime.Now.Month.ToString("00");
            string date = DateTime.Now.Day.ToString("00");
            string hour = DateTime.Now.Hour.ToString("00");
            string minute = DateTime.Now.Minute.ToString("00");
            string second = DateTime.Now.Second.ToString("00");

            string timestamp = $"[{year}-{month}-{date} {hour}:{minute}:{second}] ";

            if (isTypeGiven)
                value = msg.Substring(type.Length).Trim();
            else
                value = msg.Trim();

            switch (type)
            {
                case "info":
                    value = timestamp + "[INFO] " + value;
                    break;
                case "warn":
                    value = timestamp + "[WARN] " + value;
                    break;
                case "error":
                    value = timestamp + "[ERROR] " + value;
                    break;
                case "log":
                    value = timestamp + "[LOG] " + value;
                    break;
                case "debug":
                    value = timestamp + "[DEBUG] " + value;
                    break;
                case "success":
                    value = timestamp + "[SUCCESS] " + value;
                    break;
                default:
                    value = timestamp + value;
                    break;
            }

            Console.WriteLine(value);
        }
    }
}
