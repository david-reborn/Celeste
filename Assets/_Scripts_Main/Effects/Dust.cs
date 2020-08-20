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
            //Level scene = Engine.Scene as Level;
            for (int index = 0; index < count; ++index)
            {
                //创建N个粒子，进行发射

            }
            //    scene.Particles.Emit(particleType, Vector2.op_Addition(position, Calc.Random.Range(Vector2.op_UnaryNegation(vector), vector)), direction);
        }

        //public static void BurstFG(
        //  Vector2 position,
        //  float direction,
        //  int count = 1,
        //  float range = 4f,
        //  ParticleType particleType = null)
        //{
        //    if (particleType == null)
        //        particleType = ParticleTypes.Dust;
        //    Vector2 vector = Calc.AngleToVector(direction - 1.570796f, range);
        //    vector.X = (__Null)(double)Math.Abs((float)vector.X);
        //    vector.Y = (__Null)(double)Math.Abs((float)vector.Y);
        //    Level scene = Engine.Scene as Level;
        //    for (int index = 0; index < count; ++index)
        //        scene.ParticlesFG.Emit(particleType, Vector2.op_Addition(position, Calc.Random.Range(Vector2.op_UnaryNegation(vector), vector)), direction);
        //}
    }
}
