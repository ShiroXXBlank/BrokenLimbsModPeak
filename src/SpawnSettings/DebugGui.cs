using BrokenLimbs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static Zorro.ControllerSupport.Rumble.RumbleClip;

namespace BrokenLimbs
{
    public class DebugGui : MonoBehaviour
    {
        private List<string> logMessages = new List<string>();
        private Vector2 scrollPos;
        private string inputText = "";

        private Rect windowRect = new Rect(20, 20, 700, 600);

        private const int maxLogs = 1000;
        private int logCount = 0;
        private string[] debugLogs = new string[maxLogs];
        private Vector2 debugScrollPos = Vector2.zero;
        private string debugInput = "";
        private int lastLogCount = 0;

        bool showWindow = true;

        private CommandManager commandManager = new CommandManager();

        public void Start ()
        {
            // Register Commands
            commandManager.RegisterCommand(new Command("Help", "Displays all of the current commands and their descriptions", "help", (args) =>
            {
                // Reference epicGUI
                GameObject parent = Plugin.mod;
                DebugGui gui = parent.GetComponent<DebugGui>();

                if (args.Length == 0)
                {
                    foreach (string cmd in gui.commandManager.GetCommandList())
                    {
                        Debug.Log(cmd);
                    }
                } else if (args.Length == 1)
                {
                    Command cmd = gui.commandManager.GetCommands().FirstOrDefault(c => c.Name.ToLower() == args[0].ToLower());

                    Debug.Log(cmd.Usage);
                }
            }));

            commandManager.RegisterCommand(new Command("Damage", "Damages the limbs health", "Damage <string> <float>", (args) =>
            {
                // Reference epicGUI
                GameObject parent = Plugin.mod;
                Logic Logic = parent.GetComponent<Logic>();

                if (args.Length == 2)
                {
                    Logic.Damage(args[0], float.Parse(args[1]));
                }
            }));

            commandManager.RegisterCommand(new Command("GetHp", "Gets the limbs health", "GetHp <string>", (args) =>
            {
                // Reference epicGUI
                GameObject parent = Plugin.mod;
                Logic Logic = parent.GetComponent<Logic>();

                if (args.Length == 1)
                {
                    Debug.Log(Logic.limbs.Find(x=>x.Name.ToLower() == args[0].ToLower()).health);
                }
            }));

            commandManager.RegisterCommand(new Command("SetHealth", "Damages the limbs health", "SetHealth <string> <float>", (args) =>
            {
                // Reference epicGUI
                GameObject parent = Plugin.mod;
                Logic Logic = parent.GetComponent<Logic>();

                if (args.Length == 2)
                {
                    Logic.SetHealth(args[0], float.Parse(args[1]));
                }
            }));

            commandManager.RegisterCommand(new Command("ResetHealth", "Resets the limbs health", "ResetHealth", (args) =>
            {
                // Reference epicGUI

                GameObject parent = Plugin.mod;
                Logic Logic = parent.GetComponent<Logic>();

                if (args.Length == 0)
                {
                    foreach (Limb limb in Logic.limbs)
                    {
                        limb.Health = 100;
                    }
                }
            }));

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                showWindow = !showWindow;
            }
        }

        void AddLog(string log)
        {
            if (logCount < maxLogs)
            {
                debugLogs[logCount] = log;
                logCount++;
            }
            else
            {
                // Shift all logs up by 1
                for (int i = 1; i < maxLogs; i++)
                {
                    debugLogs[i - 1] = debugLogs[i];
                }
                // Add new log at the end
                debugLogs[maxLogs - 1] = log;
            }
        }

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            string coloredLog;

            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    // Include stack trace for errors and exceptions
                    coloredLog = "<color=red>" + logString + " - " + stackTrace + "</color>";
                    break;
                case LogType.Warning:
                    coloredLog = "<color=yellow>" + logString + "</color>";
                    break;
                case LogType.Log:
                    coloredLog = "<color=white>" + logString + "</color>";
                    break;
                default:
                    coloredLog = logString;
                    break;
            }

            AddLog(coloredLog);
        }

        private void OnGUI()
        {
            if (showWindow)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.6f);
                GUIStyle windowStyle = new GUIStyle(GUI.skin.window);
                windowStyle.normal.textColor = Color.white;  // Change title text color here
                windowRect = GUI.Window(985, windowRect, DrawWindow, "Debug Console", windowStyle);
            }
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            // Scroll View
            debugScrollPos = GUILayout.BeginScrollView(debugScrollPos, false, true, GUILayout.ExpandHeight(true)); // <-- force height

            GUI.color = Color.white;

            if (logCount == 0)
            {
                GUILayout.Label("No logs yet.");
            }
            else
            {
                for (int i = 0; i < logCount; i++)
                {
                    GUILayout.Label(debugLogs[i]);
                }
            }

            GUILayout.EndScrollView();

            // Auto-scroll only if user hasn't scrolled up
            if (logCount > lastLogCount)
            {
                debugScrollPos.y = float.MaxValue;
            }
            lastLogCount = logCount;

            GUI.SetNextControlName("DebugInputField");
            debugInput = GUILayout.TextField(debugInput);

            if (GUILayout.Button("Enter", GUILayout.Width(60)))
            {
                if (!string.IsNullOrWhiteSpace(debugInput))
                {
                    commandManager.ExecuteCommand(debugInput);
                    debugInput = "";
                }
            }

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, windowRect.width, 25));
        }
    }
}