using UnityEngine;

namespace GrisLikeDemo
{
    public static class AutoBootstrapOnLoad
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureBootstrapExists()
        {
            if (Object.FindObjectOfType<DemoBootstrap>() != null)
            {
                return;
            }

            var bootstrap = new GameObject("Bootstrap");
            bootstrap.AddComponent<DemoBootstrap>();
        }
    }
}
