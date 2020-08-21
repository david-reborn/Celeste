using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class ParticleSystem2D : MonoBehaviour
    {
        private Particle[] particles;
        private int nextSlot;

        public void Init(int size, Particle particlePrefab)
        {
            this.particles = new Particle[size];
            for (int i = 0; i < this.particles.Length; i++)
            {
                this.particles[i] = Instantiate(particlePrefab, this.transform);
            }
        }

        public void Emit(ParticleType type, Vector2 position)
        {
            type.Create(this.particles[this.nextSlot], position);
            this.particles[this.nextSlot].Reload();
            this.nextSlot = (this.nextSlot + 1) % this.particles.Length;
        }

        public void Emit(ParticleType type, Vector2 position, float direction)
        {
            type.Create(this.particles[this.nextSlot], position, direction);
            this.particles[this.nextSlot].Reload();
            this.nextSlot = (this.nextSlot + 1) % this.particles.Length;
        }

        private void Update()
        {
            for (int index = 0; index < this.particles.Length; ++index)
            {
                if (this.particles[index].gameObject.activeSelf)
                {
                    this.particles[index].OnUpdate(new float?());
                    this.particles[index].OnRender();
                }
            }
        }
    }
}
