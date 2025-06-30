using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace BrokenLimbs;
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Log { get; private set; } = null!;
    private static Harmony har = new Harmony("com.shiro.brokenlimbs");
    public static GameObject mod;

    internal static PluginConfig BoundConfig { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;

        BoundConfig = new PluginConfig(base.Config);

        try
        {
            har.PatchAll(Assembly.GetExecutingAssembly());
            LoadResources();
        }
        catch (Exception ex)
        {
            Log.LogInfo($"[BrokenLimbs] Harmony patch failed: {ex}");
        }

        // Log our awake here so we can see it in LogOutput.log file
        Log.LogInfo($"Plugin {Name} is loaded!");
    }

    private void OnDestroy()
    {
        har?.UnpatchSelf();
        UnloadResources();
    }

    private void LoadResources()
    {
        mod = new GameObject("brokenlimbs");

        mod.AddComponent<DebugGui>();
        mod.AddComponent<BrokenLimbsUI>();
        mod.AddComponent<Logic>();
        UnityEngine.Object.DontDestroyOnLoad(mod);
        Log.LogInfo($"Mod Object {mod.name} loaded!");
    }

    private void UnloadResources()
    {
        Destroy(mod);

        Log.LogInfo($"Mod Object {mod.name} unloaded!");
    }
}
