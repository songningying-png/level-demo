using UnityEngine;
using UnityEngine.UI;

namespace GrisLikeDemo
{
    public class GameDirector : MonoBehaviour
    {
        [HideInInspector] public Sprite[] runFrames;
        [HideInInspector] public Sprite[] idleFrames;
        [HideInInspector] public Sprite[] jumpFrames;

        [HideInInspector] public Sprite bg0;
        [HideInInspector] public Sprite mountainMid;
        [HideInInspector] public Sprite vista2;
        [HideInInspector] public Sprite dragonSpine;
        [HideInInspector] public Sprite handStatue;
        [HideInInspector] public Sprite rubble;
        [HideInInspector] public Sprite rubble1;
        [HideInInspector] public Sprite ruinsDebris;
        [HideInInspector] public Sprite gateFinal;
        [HideInInspector] public Sprite building02Full;

        private Transform _player;
        private PlayerController2D _playerController;
        private CameraController2D _cameraController;
        private bool _level2Active;
        private bool _ended;

        private void Start()
        {
            SetupCamera();
            BuildLightingAndTint();
            BuildLevel1();
            BuildUIHint();
        }

        private void SetupCamera()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var cameraObject = new GameObject("Main Camera");
                cam = cameraObject.AddComponent<Camera>();
                cam.tag = "MainCamera";
            }

            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.11f, 0.02f, 0.03f);
            cam.orthographic = true;
            cam.orthographicSize = 5.8f;
            cam.transform.position = new Vector3(0f, 1.3f, -10f);

            _cameraController = cam.gameObject.GetComponent<CameraController2D>();
            if (_cameraController == null)
            {
                _cameraController = cam.gameObject.AddComponent<CameraController2D>();
            }
        }

        private void BuildLightingAndTint()
        {
            var tint = new GameObject("RedGlobalTint");
            var sr = tint.AddComponent<SpriteRenderer>();
            sr.sprite = CreateRuntimeSprite(Color.white);
            sr.color = new Color(0.75f, 0.15f, 0.15f, 0.12f);
            sr.sortingOrder = 900;
            tint.transform.localScale = new Vector3(160f, 90f, 1f);
            tint.transform.position = new Vector3(80f, 0f, 0f);
        }

        private void BuildLevel1()
        {
            _level2Active = false;

            LevelBuilder.BuildGround(
                "L1_Ground",
                0f,
                390f,
                yBase: -3.65f,
                amplitude: 0.17f,
                roughness: 0.13f,
                color: new Color(0.24f, 0.09f, 0.10f),
                sortingOrder: 1
            );

            LevelBuilder.BuildMountainLayers(0f, 390f, 5);
            LevelBuilder.BuildFogLayer("Fog_Back", -1.2f, 0.24f, 0.55f, 0.012f, 2, new Color(1f, 0.88f, 0.88f, 0.18f));
            LevelBuilder.BuildFogLayer("Fog_Front", 0.55f, 0.4f, 0.85f, 0.018f, 80, new Color(1f, 0.75f, 0.75f, 0.24f));

            BuildParallaxSprite(bg0, "L1_BG0", new Vector3(80f, -0.2f, 0f), new Vector3(95f, 22f, 1f), -10, 0.06f);
            BuildParallaxSprite(vista2, "L1_Vista2", new Vector3(95f, -0.6f, 0f), new Vector3(96f, 24f, 1f), -7, 0.12f);
            BuildParallaxSprite(mountainMid, "L1_Mid", new Vector3(110f, -0.8f, 0f), new Vector3(105f, 22f, 1f), -4, 0.2f);
            BuildParallaxSprite(dragonSpine, "L1_Dragon", new Vector3(135f, -0.4f, 0f), new Vector3(55f, 15f, 1f), 18, 0.38f);
            BuildParallaxSprite(rubble, "L1_Rubble", new Vector3(70f, -2.5f, 0f), new Vector3(30f, 6f, 1f), 22, 0.5f);
            BuildParallaxSprite(rubble1, "L1_Rubble1", new Vector3(148f, -2.4f, 0f), new Vector3(25f, 7f, 1f), 22, 0.52f);
            BuildParallaxSprite(ruinsDebris, "L1_RuinsDebris", new Vector3(200f, -1.8f, 0f), new Vector3(42f, 8f, 1f), 24, 0.58f);
            BuildParallaxSprite(handStatue, "L1_Hand", new Vector3(38f, -1.7f, 0f), new Vector3(13f, 10f, 1f), 25, 0.65f);
            BuildParallaxSprite(gateFinal, "L1_Gate", new Vector3(245f, -1.65f, 0f), new Vector3(18f, 11f, 1f), 28, 0.7f);
            BuildParallaxSprite(building02Full, "L1_Building02", new Vector3(290f, -1.95f, 0f), new Vector3(24f, 12f, 1f), 28, 0.72f);

            LevelBuilder.BuildObstacleBlock("Obstacle_Hand", x: 31f, y: -2.1f, w: 4.5f, h: 2.7f, color: new Color(0.35f, 0.09f, 0.11f));
            LevelBuilder.BuildObstacleBlock("Obstacle_Building", x: 58f, y: -1.8f, w: 3.4f, h: 2.1f, color: new Color(0.3f, 0.08f, 0.1f));
            LevelBuilder.BuildObstacleBlock("Building01_Climb", x: 86f, y: -0.6f, w: 9.5f, h: 3.2f, color: new Color(0.28f, 0.07f, 0.09f));
            LevelBuilder.BuildObstacleBlock("Ruins_01", x: 126f, y: -1.2f, w: 6.8f, h: 2.5f, color: new Color(0.3f, 0.08f, 0.1f));
            LevelBuilder.BuildObstacleBlock("Ruins_02", x: 160f, y: -1.3f, w: 8.2f, h: 2.6f, color: new Color(0.26f, 0.06f, 0.08f));

            LevelBuilder.BuildStepSlope("L1_UpSlope45", startX: 205f, baseY: -3.45f, steps: 24, stepW: 1.2f, stepH: 0.33f, true);
            LevelBuilder.BuildRomanColumns("L1_Columns", 210f, -2.6f, 10);

            LevelBuilder.BuildObstacleBlock("L1_SymmetricBuildings", x: 262f, y: -0.9f, w: 22f, h: 4.2f, color: new Color(0.34f, 0.09f, 0.12f));

            SpawnPlayer(new Vector3(0f, -2.7f, 0f));
            SpawnTransitionOrb(new Vector3(328f, 1.4f, 0f));
        }

        private void SpawnPlayer(Vector3 startPos)
        {
            var player = new GameObject("Player");
            player.transform.position = startPos;
            player.layer = LayerMask.NameToLayer("Default");

            var sr = player.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 100;
            if (idleFrames != null && idleFrames.Length > 0)
            {
                sr.sprite = idleFrames[0];
            }
            else if (runFrames != null && runFrames.Length > 0)
            {
                sr.sprite = runFrames[0];
            }
            else
            {
                sr.sprite = CreateRuntimeSprite(Color.black);
            }

            player.transform.localScale = new Vector3(1.7f, 1.7f, 1f);

            var body = player.AddComponent<Rigidbody2D>();
            body.gravityScale = 4.2f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            var col = player.AddComponent<CapsuleCollider2D>();
            col.size = new Vector2(0.6f, 1.08f);
            col.offset = new Vector2(0f, -0.08f);

            _playerController = player.AddComponent<PlayerController2D>();
            _playerController.runFrames = runFrames;
            _playerController.idleFrames = idleFrames;
            _playerController.jumpFrames = jumpFrames;
            _playerController.baseMoveSpeed = 3.2f;
            _playerController.jumpForce = 10.8f;
            _playerController.groundMask = LayerMask.GetMask("Default");

            _player = player.transform;
            _cameraController.target = _player;
        }

        private void SpawnTransitionOrb(Vector3 pos)
        {
            var orb = new GameObject("Level1Orb");
            orb.transform.position = pos;
            orb.layer = LayerMask.NameToLayer("Default");

            var sr = orb.AddComponent<SpriteRenderer>();
            sr.sprite = CreateRuntimeSprite(Color.white);
            sr.color = new Color(1f, 1f, 1f, 0.92f);
            sr.sortingOrder = 120;
            orb.transform.localScale = new Vector3(0.72f, 0.72f, 1f);

            var col = orb.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.52f;

            var orbFx = orb.AddComponent<TransitionOrb>();
            orbFx.director = this;
            orbFx.particleCount = 30000;
        }

        private void BuildLevel2()
        {
            _level2Active = true;

            LevelBuilder.BuildGround(
                "L2_Ground",
                390f,
                840f,
                yBase: -3.75f,
                amplitude: 0.2f,
                roughness: 0.12f,
                color: new Color(0.28f, 0.08f, 0.09f),
                sortingOrder: 1
            );

            LevelBuilder.BuildMountainLayers(390f, 840f, 5);
            LevelBuilder.BuildFogLayer("L2_Fog_Back", -1.4f, 0.22f, 0.5f, 0.01f, 2, new Color(1f, 0.8f, 0.8f, 0.16f));
            LevelBuilder.BuildFogLayer("L2_Fog_Front", 0.6f, 0.4f, 0.9f, 0.019f, 82, new Color(1f, 0.72f, 0.72f, 0.22f));

            LevelBuilder.BuildObstacleBlock("L2_SymmetryGroup_01", x: 450f, y: -1.1f, w: 32f, h: 4f, color: new Color(0.34f, 0.08f, 0.1f));
            LevelBuilder.BuildStepSlope("L2_DownSlope45", startX: 504f, baseY: 1.2f, steps: 24, stepW: 1.1f, stepH: 0.29f, false);
            LevelBuilder.BuildEyeGlyphBand("L2_BackGlyphs", 524f, -0.5f, 24);
            LevelBuilder.BuildBridge("L2_Bridge", 552f, -2.2f, 30f);
            LevelBuilder.BuildFloatingPlatforms("L2_Floaters", 604f, -1.4f, 13);

            LevelBuilder.BuildObstacleBlock(
                "L2_GiantStatueHand",
                x: 680f,
                y: 2.6f,
                w: 13f,
                h: 2.4f,
                color: new Color(0.42f, 0.13f, 0.14f)
            );

            var end = new GameObject("EndTrigger");
            end.transform.position = new Vector3(683.5f, 4f, 0f);
            var col = end.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(12f, 2.6f);
            var endTrigger = end.AddComponent<EndTrigger>();
            endTrigger.director = this;

            _cameraController.SetFocusWindow(390f, 840f);
            _playerController.baseMoveSpeed = 3.35f;
            _playerController.AllowCameraZoomMoments();
        }

        private void BuildParallaxSprite(Sprite sprite, string name, Vector3 pos, Vector3 scale, int sortingOrder, float factor)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            go.transform.localScale = scale;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite != null ? sprite : CreateRuntimeSprite(new Color(0.4f, 0.12f, 0.13f));
            sr.sortingOrder = sortingOrder;

            var p = go.AddComponent<ParallaxLayer>();
            p.factor = factor;
        }

        private void BuildUIHint()
        {
            var canvasGo = new GameObject("UI");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.AddComponent<GraphicRaycaster>();

            var textGo = new GameObject("Hint");
            textGo.transform.SetParent(canvasGo.transform, false);
            var txt = textGo.AddComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 24;
            txt.alignment = TextAnchor.UpperCenter;
            txt.color = new Color(1f, 0.93f, 0.93f, 0.9f);
            txt.text = "A/D 移动，空格跳跃，触碰光球后点击鼠标进入第二关";

            var rect = textGo.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -18f);
            rect.sizeDelta = new Vector2(1200f, 80f);
        }

        public void OnOrbTouched()
        {
            if (_level2Active)
            {
                return;
            }

            StartCoroutine(LoadLevel2WhenClicked());
        }

        private System.Collections.IEnumerator LoadLevel2WhenClicked()
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            BuildLevel2();
            _player.position = new Vector3(396f, -2.6f, 0f);
        }

        public void ShowEnding()
        {
            if (_ended)
            {
                return;
            }

            _ended = true;

            var canvasGo = GameObject.Find("UI");
            if (canvasGo == null)
            {
                return;
            }

            var textGo = new GameObject("EndingText");
            textGo.transform.SetParent(canvasGo.transform, false);
            var txt = textGo.AddComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 50;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = new Color(1f, 1f, 1f, 0.96f);
            txt.text = "一切，才刚刚开始。";

            var rect = textGo.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(1200f, 180f);
            rect.anchoredPosition = Vector2.zero;

            _playerController.enabled = false;
        }

        public static Sprite CreateRuntimeSprite(Color color)
        {
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
    }
}
