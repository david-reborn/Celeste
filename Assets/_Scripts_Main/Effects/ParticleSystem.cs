using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class ParticleSystem : MonoBehaviour
    {
        public static ParticleSystem instance;

        public Particle particlePrefab;
        private Particle[] particles;
        private int nextSlot;

        public void Awake()
        {
            instance = this;
            this.particles = new Particle[100];
            for (int i = 0; i < this.particles.Length; i++)
            {
                this.particles[i] = Instantiate(particlePrefab, this.transform);
            }
        }

        public void Emit(ParticleType type, Vector2 position)
        {
            type.Create(this.particles[this.nextSlot], position);
            this.nextSlot = (this.nextSlot + 1) % this.particles.Length;
        }

    }
}
