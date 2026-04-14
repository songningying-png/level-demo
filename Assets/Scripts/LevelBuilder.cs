using UnityEngine;

namespace GrisLikeDemo
{
    public static class LevelBuilder
    {
        public static void BuildGround(string name, float startX, float endX, float yBase, float amplitude, float roughness, Color color, int sortingOrder)
        {
            var root = new GameObject(name);
            for (float x = startX; x < endX; x += 1.4f)
            {
                float y = yBase + Mathf.Sin(x * roughness) * amplitude;
                var seg = BuildBlock(root.transform, x, y, 1.6f, 1.2f, color, sortingOrder);
                var col = seg.AddComponent<BoxCollider2D>();
                col.size = Vector2.one;
            }
        }

        public static void BuildMountainLayers(float startX, float endX, int layerCount)
        {
            for (int layer = 0; layer < layerCount; layer++)
            {
                var root = new GameObject("Mountains_" + layer);
                float yBase = -1.8f + layer * 0.42f;
                float amp = 0.25f + layer * 0.12f;
                float len = 0.04f + layer * 0.008f;
                var col = Color.Lerp(new Color(0.22f, 0.06f, 0.07f), new Color(0.54f, 0.2f, 0.2f), layer / 5f);
                col.a = 0.65f;

                for (float x = startX; x <= endX; x += 5.2f)
                {
                    float y = yBase + Mathf.Sin(x * len) * amp;
                    BuildBlock(root.transform, x, y, 6f, 1f, col, -20 + layer);
                }
            }
        }

        public static void BuildFogLayer(string name, float y, float minScale, float maxScale, float speed, int sortingOrder, Color color)
        {
            var root = new GameObject(name);
            for (int i = 0; i < 24; i++)
            {
                float x = i * 18f - 30f;
                var puff = BuildBlock(root.transform, x, y + Random.Range(-0.3f, 0.3f), Random.Range(8f, 14f), Random.Range(1.2f, 2.4f), color, sortingOrder);
                var fog = puff.AddComponent<MovingFog>();
                fog.speed = speed * Random.Range(0.6f, 1.3f);
                fog.minScale = minScale;
                fog.maxScale = maxScale;
            }
        }

        public static GameObject BuildObstacleBlock(string name, float x, float y, float w, float h, Color color)
        {
            var go = BuildBlock(null, x, y, w, h, color, 30);
            go.name = name;
            var col = go.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;
            return go;
        }

        public static void BuildStepSlope(string name, float startX, float baseY, int steps, float stepW, float stepH, bool upwards)
        {
            var root = new GameObject(name);
            for (int i = 0; i < steps; i++)
            {
                float x = startX + i * stepW;
                float y = upwards ? baseY + i * stepH : baseY - i * stepH;
                var step = BuildBlock(root.transform, x, y, stepW + 0.08f, 0.45f, new Color(0.32f, 0.09f, 0.11f), 34);
                var col = step.AddComponent<BoxCollider2D>();
                col.size = Vector2.one;
            }
        }

        public static void BuildRomanColumns(string name, float startX, float baseY, int count)
        {
            var root = new GameObject(name);
            for (int i = 0; i < count; i++)
            {
                float x = startX + i * 4.2f;
                BuildBlock(root.transform, x, baseY + 1.6f, 0.8f, 3f, new Color(0.48f, 0.2f, 0.2f, 0.45f), -2);
                BuildBlock(root.transform, x, baseY + 3.2f, 1.2f, 0.35f, new Color(0.5f, 0.22f, 0.21f, 0.42f), -2);
            }
        }

        public static void BuildEyeGlyphBand(string name, float startX, float y, int count)
        {
            var root = new GameObject(name);
            for (int i = 0; i < count; i++)
            {
                float x = startX + i * 3.1f;
                float size = Random.Range(1.1f, 1.8f);
                var e = BuildBlock(root.transform, x, y + Mathf.Sin(i * 0.6f) * 0.6f, size, size * 0.45f, new Color(0.68f, 0.25f, 0.24f, 0.38f), -6);
                e.transform.Rotate(Vector3.forward, Random.Range(-13f, 13f));
            }
        }

        public static void BuildBridge(string name, float x, float y, float width)
        {
            var root = new GameObject(name);
            for (float xx = x; xx < x + width; xx += 1.3f)
            {
                var plank = BuildBlock(root.transform, xx, y + Mathf.Sin(xx * 0.18f) * 0.08f, 1.4f, 0.35f, new Color(0.3f, 0.1f, 0.1f), 32);
                var col = plank.AddComponent<BoxCollider2D>();
                col.size = Vector2.one;
            }
        }

        public static void BuildFloatingPlatforms(string name, float startX, float startY, int count)
        {
            var root = new GameObject(name);
            for (int i = 0; i < count; i++)
            {
                float x = startX + i * 5.7f;
                float y = startY + i * 0.55f + Mathf.Sin(i * 0.7f) * 0.2f;
                var p = BuildBlock(root.transform, x, y, 3.1f, 0.5f, new Color(0.34f, 0.12f, 0.12f), 33);
                var col = p.AddComponent<BoxCollider2D>();
                col.size = Vector2.one;
            }
        }

        private static GameObject BuildBlock(Transform parent, float x, float y, float w, float h, Color color, int sortingOrder)
        {
            var go = new GameObject("Block");
            if (parent != null)
            {
                go.transform.SetParent(parent);
            }

            go.transform.position = new Vector3(x, y, 0f);
            go.transform.localScale = new Vector3(w, h, 1f);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = GameDirector.CreateRuntimeSprite(Color.white);
            sr.color = color;
            sr.sortingOrder = sortingOrder;
            return go;
        }
    }
}
