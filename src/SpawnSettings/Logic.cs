using pworld.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrokenLimbs
{
    public class Logic : MonoBehaviour
    {
        private static Rect windowRect = new Rect(Screen.width / 2, Screen.height / 2, 200, 300);
        private static float margin = 10f;
        private static float padding = 5f;
        private static Character ch;

        public List<Limb> limbs = new List<Limb>();

        //void OnEnable()
        //{
        //    SceneManager.sceneLoaded += OnSceneLoaded;
        //}

        //void OnDisable()
        //{
        //    SceneManager.sceneLoaded -= OnSceneLoaded;
        //}

        //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    if (scene.name.Contains("Level"))
        //    {
        //        Debug.Log("scene loaded resetting health.");
        //        foreach (Limb limb in limbs)
        //        {
        //            limb.Health = 100;
        //        }
        //    }
        //}

        public float ConvertImpactVelocityToDamage(float impactVelocity)
        {
            // Parameters
            float minVelocity = 7.5f;    // Below this, no damage
            float maxVelocity = 30;   // At or above this, max damage
            float maxDamage = 100f;    // Max damage possible

            if (impactVelocity < minVelocity)
                return 0f;

            // Clamp velocity between min and max
            float clampedVelocity = Mathf.Clamp(impactVelocity, minVelocity, maxVelocity);

            // Map clamped velocity to damage 0..maxDamage
            float damage = (clampedVelocity - minVelocity) / (maxVelocity - minVelocity) * maxDamage;

            return damage;
        }

        void Start ()
        {
            windowRect.x = Screen.width - windowRect.width / 2 - margin;
            windowRect.y = margin;

            Limb head = new Limb("Head", Color.white, new Rect(windowRect.x, windowRect.y, 50, 50));
            Limb torso = new Limb("Torso", Color.white, new Rect(windowRect.x, head.sizeRect.y + head.sizeRect.height + padding, head.sizeRect.width, head.sizeRect.height * 2));
            Limb rightArm = new Limb("Arm_R", Color.white, new Rect(torso.sizeRect.x + torso.sizeRect.width + padding, torso.sizeRect.y, head.sizeRect.width / 2, torso.sizeRect.height));
            Limb leftArm = new Limb("Arm_L", Color.white, new Rect(torso.sizeRect.x - rightArm.sizeRect.width - padding, rightArm.sizeRect.y, rightArm.sizeRect.width, rightArm.sizeRect.height));
            Limb rightLeg = new Limb("Foot_L", Color.white, new Rect(torso.sizeRect.x, torso.sizeRect.y + torso.sizeRect.height + padding, rightArm.sizeRect.width - padding / 2, rightArm.sizeRect.height));
            Limb leftLeg = new Limb("Foot_R", Color.white, new Rect(torso.sizeRect.x + torso.sizeRect.width - rightLeg.sizeRect.width + 1, torso.sizeRect.y + torso.sizeRect.height + padding, rightLeg.sizeRect.width, rightLeg.sizeRect.height));

            limbs.Add(head);
            limbs.Add(torso);
            limbs.Add(rightArm);
            limbs.Add(leftArm);
            limbs.Add(rightLeg);
            limbs.Add(leftLeg);
        
        }

        void Update ()
        {
            ch = Character.localCharacter;

            if (ch != null)
            {
                CharacterMovement chm = ch.GetComponent<CharacterMovement>();
                if (ch.data.sinceGrounded > Plugin.BoundConfig.fallDistance.Value && !ch.data.isClimbingAnything && !ch.data.isJumping && !ch.data.isCarried)
                {
                    ch.RPCA_Fall(Plugin.BoundConfig.ragdollDuration.Value);
                    chm.ApplyExtraDrag(0.9f, false);
                    GamefeelHandler.instance.AddPerlinShake(5f * chm.FallFactor(3f, 1f), 0.2f, 10f);
                } 
            }
        }

        public void Damage(string name, float dmg)
        {
            Limb limb = limbs.FirstOrDefault(l => l.Name.ToLower() == name.ToLower());

            if (limb != null)
            {
                limb.Damage(dmg);
            }
        }

        public void SetHealth(string name, float healthValue)
        {
            Limb limb = limbs.FirstOrDefault(l => l.Name.ToLower() == name.ToLower());

            if (limb != null)
            {
                limb.Health = healthValue;
            }
        }

        public List<Limb> GetLimbs()
        {
            return limbs;
        }
    }
}
