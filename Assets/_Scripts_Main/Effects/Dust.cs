using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public static class Dust
    {
        public static void Burst(Vector2 position, float direction, int count = 1, ParticleType particleType = null)
        {
            if (particleType == null)
                particleType = ParticleTypes.Dust;
            Vector2 vector = Util.AngleToVector(direction - 1.570796f, 4f);
            vector.x = Math.Abs((float)vector.x);
            vector.y = Math.Abs((float)vector.y);

            for (int index = 0; index < count; ++index)
            {
                //创建N个粒子，进行发射
                ParticleController.instance.Particles.Emit(particleType, position + RandomUtil.Random.Range(-vector, vector), direction);
            }
        }

        public static void BurstFG(Vector2 position, float direction, int count = 1, float range = 4f, ParticleType particleType = null)
        {
            if (particleType == null)
                particleType = ParticleTypes.Dust;
            Vector2 vector = Util.AngleToVector(direction - 1.570796f, range);
            vector.x = Math.Abs((float)vector.x);
            vector.y = Math.Abs((float)vector.y);
            for (int index = 0; index < count; ++index)
            {
                ParticleController.instance.ParticlesFG.Emit(particleType, position + RandomUtil.Random.Range(-vector, vector), direction);
            }
        }
    }
}
