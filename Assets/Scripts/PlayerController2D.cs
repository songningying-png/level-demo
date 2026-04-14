using UnityEngine;

namespace GrisLikeDemo
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController2D : MonoBehaviour
    {
        public Sprite[] runFrames;
        public Sprite[] idleFrames;
        public Sprite[] jumpFrames;

        public float baseMoveSpeed = 3.2f;
        public float jumpForce = 10f;
        public LayerMask groundMask;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private float _animTimer;
        private int _animIndex;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float dir = Mathf.Abs(xInput) > 0.01f ? Mathf.Sign(xInput) : 1f;
            float targetSpeed = baseMoveSpeed * dir;
            _rb.velocity = new Vector2(targetSpeed, _rb.velocity.y);

            bool grounded = IsGrounded();
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && grounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }

            if (targetSpeed < 0f)
            {
                _sr.flipX = true;
            }
            else if (targetSpeed > 0f)
            {
                _sr.flipX = false;
            }

            Animate(grounded, Mathf.Abs(targetSpeed));
        }

        private bool IsGrounded()
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.65f);
            var hit = Physics2D.Raycast(origin, Vector2.down, 0.16f, groundMask);
            return hit.collider != null;
        }

        private void Animate(bool grounded, float horizontalSpeed)
        {
            Sprite[] current = idleFrames;
            float fps = 6f;

            if (!grounded)
            {
                current = jumpFrames;
                fps = 10f;
            }
            else if (horizontalSpeed > 0.2f)
            {
                current = runFrames;
                fps = 13f;
            }

            if (current == null || current.Length == 0)
            {
                return;
            }

            _animTimer += Time.deltaTime;
            if (_animTimer >= 1f / fps)
            {
                _animTimer = 0f;
                _animIndex = (_animIndex + 1) % current.Length;
                _sr.sprite = current[_animIndex];
            }
        }

        public void AllowCameraZoomMoments() {}
    }
}
