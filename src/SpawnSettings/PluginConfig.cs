using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BrokenLimbs
{
    class PluginConfig
    {
        public readonly ConfigEntry<float> ragdollDuration;
        public readonly ConfigEntry<float> fallDistance;

        public PluginConfig (ConfigFile cfg)
        {
            cfg.SaveOnConfigSet = false;

            ragdollDuration = cfg.Bind(
                "Settings",                          // Config section
                "ragdollDuration",                     // Key of this config
                2f,                    // Default value
                "The maxiumum placement range for the anchor"    // Description
            );

            fallDistance = cfg.Bind(
                "Settings",                          // Config section
                "fallDistance",                     // Key of this config
                1.5f,                    // Default value
                "The maxiumum ghost range for the anchor"    // Description
            );

            ClearOrphanedEntries(cfg);
            // We need to manually save since we disabled `SaveOnConfigSet` earlier //
            cfg.Save();
            // And finally, we re-enable `SaveOnConfigSet` so changes to our config //
            // entries are written to the config file automatically from now on //
            cfg.SaveOnConfigSet = true;
        }

        static void ClearOrphanedEntries(ConfigFile cfg)
        {
            // Find the private property `OrphanedEntries` from the type `ConfigFile` //
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            // And get the value of that property from our ConfigFile instance //
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
            // And finally, clear the `OrphanedEntries` dictionary //
            orphanedEntries.Clear();
        }
    }
}
