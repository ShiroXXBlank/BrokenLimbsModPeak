using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrokenLimbs
{
    public class BrokenLimbsUI : MonoBehaviour
    {
        private static List<Limb> limbs = new List<Limb>();
        private static Logic logic = new Logic();

        void Start ()
        {
            logic = GameObject.FindFirstObjectByType<Logic>();
            limbs = logic.limbs;
            Debug.Log("CUm");
        }

        public void OnGUI()
        {
            string activeScene = SceneManager.GetActiveScene().name;
            bool flag = Character.localCharacter == null || !activeScene.Contains("Level");
            if (flag){ 
                return; 
            }

            foreach(Limb limb in limbs)
            {
                if (limb.Draw) {
                    DrawLimb(limb);
                }
            }
        }

        void DrawLimb(Limb limb)
        {
            Color color = limb.Color;

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = tex;

            GUI.backgroundColor = color;
            GUI.Box(limb.sizeRect, "", boxStyle);
        }
    }
}
