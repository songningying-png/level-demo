using UnityEngine;

namespace GrisLikeDemo
{
    public class MovingFog : MonoBehaviour
    {
        public float speed = 0.01f;
        public float minScale = 0.4f;
        public float maxScale = 0.8f;

        private Vector3 _basePos;
        private float _seed;

        private void Start()
        {
            _basePos = transform.position;
            _seed = Random.Range(0f, 100f);
        }

        private void Update()
        {
            float t = Time.time + _seed;
            transform.position = _basePos + new Vector3(Mathf.Sin(t * speed) * 2.4f, Mathf.Cos(t * speed * 0.7f) * 0.35f, 0f);
            float s = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(t * speed * 1.2f) + 1f) * 0.5f);
            transform.localScale = new Vector3(transform.localScale.x, s, 1f);
        }
    }
}
