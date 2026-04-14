using UnityEngine;

namespace GrisLikeDemo
{
    public class DemoBootstrap : MonoBehaviour
    {
        [Header("Auto assign from Assets/Sprites/... in editor")]
        public bool autoLoadSpritesInEditor = true;

        [Header("Optional manual assignment")]
        public Sprite[] runFrames;
        public Sprite[] idleFrames;
        public Sprite[] jumpFrames;

        [Header("Scene1 parallax")]
        public Sprite bg0;
        public Sprite mountainMid;
        public Sprite vista2;
        public Sprite dragonSpine;
        public Sprite handStatue;
        public Sprite rubble;
        public Sprite rubble1;
        public Sprite ruinsDebris;
        public Sprite gateFinal;
        public Sprite building02Full;

        private void Awake()
        {
            if (autoLoadSpritesInEditor)
            {
                EditorSpriteAutoLoader.TryLoad(this);
            }

            var director = gameObject.AddComponent<GameDirector>();
            director.runFrames = runFrames;
            director.idleFrames = idleFrames;
            director.jumpFrames = jumpFrames;

            director.bg0 = bg0;
            director.mountainMid = mountainMid;
            director.vista2 = vista2;
            director.dragonSpine = dragonSpine;
            director.handStatue = handStatue;
            director.rubble = rubble;
            director.rubble1 = rubble1;
            director.ruinsDebris = ruinsDebris;
            director.gateFinal = gateFinal;
            director.building02Full = building02Full;
        }
    }
}
