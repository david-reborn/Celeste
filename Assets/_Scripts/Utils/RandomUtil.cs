
using myd.celeste;
using System;
using System.Collections;
using System.Collections.Generic;

public static class RandomUtil 
{
    public static Random Random = new Random();
    private static Stack<Random> randomStack = new Stack<Random>();

    public static void PushRandom(int newSeed)
    {
        RandomUtil.randomStack.Push(Random);
        RandomUtil.Random = new Random(newSeed);
    }

    public static void PushRandom(Random random)
    {
        RandomUtil.randomStack.Push(RandomUtil.Random);
        RandomUtil.Random = random;
    }

    public static void PushRandom()
    {
        RandomUtil.randomStack.Push(RandomUtil.Random);
        RandomUtil.Random = new Random();
    }

    public static void PopRandom()
    {
        RandomUtil.Random = RandomUtil.randomStack.Pop();
    }

    public static T Choose<T>(this Rand random, T a, T b)
    {
        return GiveMe<T>(random.Next(2), a, b);
    }
    public static T Choose<T>(this Rand random, T a, T b, T c)
    {
        return GiveMe<T>(random.Next(3), a, b, c);
    }
    public static T Choose<T>(this Rand random, T a, T b, T c, T d)
    {
        return GiveMe<T>(random.Next(4), a, b, c, d);
    }
    public static T Choose<T>(this Rand random, T a, T b, T c, T d, T e)
    {
        return GiveMe<T>(random.Next(5), a, b, c, d, e);
    }
    public static T Choose<T>(this Rand random, T a, T b, T c, T d, T e, T f)
    {
        return GiveMe<T>(random.Next(6), a, b, c, d, e, f);
    }
    public static T Choose<T>(this Rand random, params T[] choices)
    {
        return choices[random.Next(choices.Length)];
    }
    public static T Choose<T>(this Rand random, List<T> choices)
    {
        return choices[random.Next(choices.Count)];
    }

    public static T GiveMe<T>(int index, T a, T b)
    {
        if (index == 0)
        {
            return a;
        }
        if (index != 1)
        {
            throw new Exception("Index was out of range!");
        }
        return b;
    }
    public static T GiveMe<T>(int index, T a, T b, T c)
    {
        switch (index)
        {
            case 0: return a;
            case 1: return b;
            case 2: return c;
            default: throw new Exception("Index was out of range!");
        }
    }
    public static T GiveMe<T>(int index, T a, T b, T c, T d)
    {
        switch (index)
        {
            case 0: return a;
            case 1: return b;
            case 2: return c;
            case 3: return d;
            default: throw new Exception("Index was out of range!");
        }
    }
    public static T GiveMe<T>(int index, T a, T b, T c, T d, T e)
    {
        switch (index)
        {
            case 0: return a;
            case 1: return b;
            case 2: return c;
            case 3: return d;
            case 4: return e;
            default: throw new Exception("Index was out of range!");
        }
    }
    public static T GiveMe<T>(int index, T a, T b, T c, T d, T e, T f)
    {
        switch (index)
        {
            case 0: return a;
            case 1: return b;
            case 2: return c;
            case 3: return d;
            case 4: return e;
            case 5: return f;
            default: throw new Exception("Index was out of range!");
        }
    }

    public static T Choose<T>(this Random random, T a, T b)
    {
        return GiveMe<T>(random.Next(2), a, b);
    }

    public static T Choose<T>(this Random random, T a, T b, T c)
    {
        return GiveMe<T>(random.Next(3), a, b, c);
    }

    public static T Choose<T>(this Random random, T a, T b, T c, T d)
    {
        return GiveMe<T>(random.Next(4), a, b, c, d);
    }

    public static T Choose<T>(this Random random, T a, T b, T c, T d, T e)
    {
        return GiveMe<T>(random.Next(5), a, b, c, d, e);
    }

    public static T Choose<T>(this Random random, T a, T b, T c, T d, T e, T f)
    {
        return GiveMe<T>(random.Next(6), a, b, c, d, e, f);
    }

    public static T Choose<T>(this Random random, params T[] choices)
    {
        return choices[random.Next(choices.Length)];
    }

    public static T Choose<T>(this Random random, List<T> choices)
    {
        return choices[random.Next(choices.Count)];
    }

    public static int Range(this Random random, int min, int max)
    {
        return min + random.Next(max - min);
    }

    public static float Range(this Random random, float min, float max)
    {
        return min + random.NextFloat(max - min);
    }

    public static UnityEngine.Vector2 Range(this Random random, UnityEngine.Vector2 min, UnityEngine.Vector2 max)
    {
        return min + new UnityEngine.Vector2(random.NextFloat(max.x - min.x), random.NextFloat(max.y - min.y));
    }

    public static int Facing(this Random random)
    {
        return (double)random.NextFloat() < 0.5 ? -1 : 1;
    }

    public static bool Chance(this Random random, float chance)
    {
        return (double)random.NextFloat() < (double)chance;
    }

    public static float NextFloat(this Random random)
    {
        return (float)random.NextDouble();
    }

    public static float NextFloat(this Random random, float max)
    {
        return random.NextFloat() * max;
    }

    public static float NextAngle(this Random random)
    {
        return random.NextFloat() * 6.283185f;
    }
    

}
