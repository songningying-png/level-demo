using UnityEngine;

namespace GrisLikeDemo
{
    public class CameraController2D : MonoBehaviour
    {
        public Transform target;
        public float smooth = 3.2f;
        public Vector3 offset = new Vector3(3.5f, 1.2f, -10f);

        private float _minX;
        private float _maxX = 9999f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desired = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);
            desired.x = Mathf.Clamp(desired.x, _minX, _maxX);
            transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * smooth);

            float zoom = 5.8f;
            if (target.position.x > 78f && target.position.x < 93f)
            {
                zoom = 7.3f;
            }
            else if (target.position.x > 198f && target.position.x < 236f)
            {
                zoom = 6.9f;
            }

            var cam = GetComponent<Camera>();
            if (cam != null)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * 1.5f);
            }
        }

        public void SetFocusWindow(float minX, float maxX)
        {
            _minX = minX;
            _maxX = maxX;
        }
    }
}
