using UnityEngine;

namespace GrisLikeDemo
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float factor = 0.2f;
        private Transform _cam;
        private Vector3 _start;

        private void Start()
        {
            _cam = Camera.main != null ? Camera.main.transform : null;
            _start = transform.position;
        }

        private void LateUpdate()
        {
            if (_cam == null)
            {
                return;
            }

            transform.position = new Vector3(
                _start.x + _cam.position.x * factor,
                _start.y + _cam.position.y * factor * 0.08f,
                _start.z
            );
        }
    }
}
