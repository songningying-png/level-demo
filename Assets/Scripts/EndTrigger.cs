using UnityEngine;

namespace GrisLikeDemo
{
    public class EndTrigger : MonoBehaviour
    {
        public GameDirector director;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController2D>() == null && other.name != "Player")
            {
                return;
            }

            if (director != null)
            {
                director.ShowEnding();
            }
        }
    }
}
