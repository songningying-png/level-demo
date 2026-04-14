using UnityEngine;

namespace GrisLikeDemo
{
    public class TransitionOrb : MonoBehaviour
    {
        public GameDirector director;
        public int particleCount = 30000;

        private bool _triggered;

        private void Update()
        {
            float pulse = 0.75f + Mathf.Sin(Time.time * 3.2f) * 0.1f;
            transform.localScale = new Vector3(pulse, pulse, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered || (other.GetComponent<PlayerController2D>() == null && other.name != "Player"))
            {
                return;
            }

            _triggered = true;
            SpawnBurst();
            if (director != null)
            {
                director.OnOrbTouched();
            }
        }

        private void SpawnBurst()
        {
            var ps = new GameObject("OrbBurstParticles").AddComponent<ParticleSystem>();
            ps.transform.position = transform.position;

            var main = ps.main;
            main.maxParticles = particleCount;
            main.startLifetime = 2.2f;
            main.startSpeed = 8f;
            main.startSize = 0.06f;
            main.startColor = new Color(1f, 1f, 1f, 0.95f);
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 0f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;

            ps.Emit(particleCount);
            Destroy(ps.gameObject, 2.8f);
            Destroy(gameObject);
        }
    }
}
