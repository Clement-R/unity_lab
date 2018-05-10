using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphVisualizer
{
    public class Visualizer : MonoBehaviour
    {
        public int resolution = 100;
        public float sign = 1f;
        public float minAmp = 1f;
        public float maxAmp = 3f;
        public float phase = 1f;
        public Color color;
        public float speed = 0.002f;
        public float period = 3f;
        
        private float _t = 0f;
        private ParticleSystem _visualizer;
        private ParticleSystem.Particle[] _parts;

        void Start()
        {
            _parts = new ParticleSystem.Particle[resolution];
            
            float step = Camera.main.orthographicSize * 2f / (float)resolution;
            float startPosition = Camera.main.transform.position.x - Camera.main.orthographicSize;

            for (int i = 0; i < resolution; i++)
            {
                _parts[i].startColor = color;
                _parts[i].startSize = 0.1f;
                _parts[i].position = new Vector3(startPosition + ((float)i * step), 0, 0);
            }

            _visualizer = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            float step = Camera.main.orthographicSize * 2f / (float)resolution;
            float startPosition = Camera.main.transform.position.x - Camera.main.orthographicSize;
            for (int i = 0; i < resolution; i++)
            {
                Vector3 position = new Vector3();
                position.x = startPosition + ((float)i * step);

                _t += speed * Time.deltaTime;

                if (_t > 1.0f)
                {
                    float temp = maxAmp;
                    maxAmp = minAmp;
                    minAmp = temp;
                    _t = 0.0f;
                }

                position.y = Mathf.Sin((period * (position.x + Time.time)) - phase) * sign * Mathf.Lerp(minAmp, maxAmp, _t);
                _parts[i].position = position;
            }

            _visualizer.SetParticles(_parts, resolution);
        }
    }
}
