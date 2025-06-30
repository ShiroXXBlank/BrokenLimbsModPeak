using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;

namespace BrokenLimbs.Patches
{
    [HarmonyPatch(typeof(Bodypart), "OnCollisionEnter")]
    public class BodyPartPatch
    {
        private static Logic logic = GameObject.FindFirstObjectByType<Logic>();
        private static string[] limbNames = { "Head", "Torso", "Foot_L", "Foot_R", "Arm_L", "Arm_R" };
        private static int vinesLayer = LayerMask.NameToLayer("Vines");
        private static int ropeLayer = LayerMask.NameToLayer("Rope");
        private static int characterLayer = LayerMask.NameToLayer("Character");
        private static int unknownLayer = 0;

        static bool Prefix(Collision collision, Bodypart __instance)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!sceneName.Contains("Level"))
                return true;
            if (__instance.character == null || __instance.character.data.sinceGrounded < 0.1f || __instance.character.data.isClimbingAnything){ 
                return true;
            }
            int layer = collision.gameObject.layer;

            string name = __instance.name;
            if (limbNames.Contains(name) && (layer != vinesLayer && layer != ropeLayer && layer != characterLayer && layer != unknownLayer) && collision.gameObject.name != "Elbow_R" && collision.gameObject.name != "Elbow_L" && __instance.character.data.sinceGrounded > 1f)
            {
                float impactSpeed = collision.relativeVelocity.magnitude;
                float dmg = logic.ConvertImpactVelocityToDamage(impactSpeed);


                logic.Damage(name, dmg);

                Debug.Log($"Name: {name} taking {dmg} damage with {impactSpeed} m/s collided with {collision.gameObject.name}, layer: {layer} ");
            }


            return true;
        }
    }
}
