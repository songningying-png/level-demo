using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GrisLikeDemo
{
    public static class EditorSpriteAutoLoader
    {
        public static void TryLoad(DemoBootstrap bootstrap)
        {
#if UNITY_EDITOR
            if (bootstrap == null)
            {
                return;
            }

            bootstrap.runFrames = LoadSet(new[]
            {
                "juese_paob_01", "juese_paob_02", "juese_paob_03",
                "juese_paob_04", "juese_paob_05", "juese_paob_06"
            });

            bootstrap.idleFrames = LoadSet(new[]
            {
                "juese_sanshitu_01", "juese_sanshitu_02", "juese_sanshitu_03"
            });

            bootstrap.jumpFrames = LoadSet(new[]
            {
                "juese_tiaoyue_01", "juese_tiaoyue_02", "juese_tiaoyue_03",
                "juese_tiaoyue_04", "juese_tiaoyue_05", "juese_tiaoyue_06"
            });

            bootstrap.bg0 = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/bg_0.png");
            bootstrap.mountainMid = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_mountain_mid.png");
            bootstrap.vista2 = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_vista2.png");
            bootstrap.dragonSpine = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_dragon_spine.png");
            bootstrap.handStatue = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_hand_statue1.png");
            bootstrap.rubble = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_rubble.png");
            bootstrap.rubble1 = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_rubble1.png");
            bootstrap.ruinsDebris = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_ruins_debris.png");
            bootstrap.gateFinal = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_gate_final.png");
            bootstrap.building02Full = LoadSingle("Assets/Sprites/Scene1_BoneRuins/Parallax/lv1_building_02_full.png");
#endif
        }

        private static Sprite[] LoadSet(string[] names)
        {
#if UNITY_EDITOR
            var arr = new Sprite[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                arr[i] = LoadSingle("Assets/Sprites/Player/" + names[i] + ".png");
            }

            return arr;
#else
            return null;
#endif
        }

        private static Sprite LoadSingle(string path)
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
#else
            return null;
#endif
        }
    }
}
