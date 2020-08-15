using Discord.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LOLS.Commands
{
    
    public class Command : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("도움")]
        public async Task HelpC()
        {
            string nspace = "LOLS.Commands";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;

            var commands = q.ToList();
            
            foreach (var cmd in commands)
            {
            }
        }
    }
}
