using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BrokenLimbs
{
    public class Command
    {
        public string Name;
        public string Description;
        public string Usage;
        public System.Action<string[]> Execute;

        public Command(string name, string description, string usage, System.Action<string[]> execute)
        {
            Name = name.ToLower();
            Description = description;
            Usage = usage;
            Execute = execute;
        }
    }
}
