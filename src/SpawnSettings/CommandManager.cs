using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text.RegularExpressions;

namespace BrokenLimbs
{
    public class CommandManager
    {
        public Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public void RegisterCommand(Command command)
        {
            commands[command.Name.ToLower()] = command;
        }


        public static string[] ParseArguments(string input)
        {
            var matches = Regex.Matches(input, @"[\""].+?[\""]|'[^']*'|\S+");
            return matches.Cast<Match>()
                          .Select(m => m.Value.Trim().Trim('"', '\'')) // remove surrounding quotes
                          .ToArray();
        }

        public bool ExecuteCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string[] parts = ParseArguments(input);
            string commandName = parts[0].ToLower();
            string[] args = parts.Skip(1).ToArray();

            if (commands.TryGetValue(commandName, out Command command))
            {
                command.Execute(args);
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetCommandList()
        {
            return commands.Values.Select(c => c.Name + " - " + c.Description);
        }

        public IEnumerable<Command> GetCommands()
        {
            return commands.Values;
        }
    }
}
