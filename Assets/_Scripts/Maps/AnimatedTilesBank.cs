﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedTilesBank
{
    public Dictionary<string, AnimatedTilesBank.Animation> AnimationsByName = new Dictionary<string, AnimatedTilesBank.Animation>();
    public List<AnimatedTilesBank.Animation> Animations = new List<AnimatedTilesBank.Animation>();

    public void Add(
      string name,
      float delay,
      Vector2 offset,
      Vector2 origin,
      List<MTexture> textures)
    {
        AnimatedTilesBank.Animation animation = new AnimatedTilesBank.Animation()
        {
            Name = name,
            Delay = delay,
            Offset = offset,
            Origin = origin,
            Frames = textures.ToArray()
        };
        animation.ID = this.Animations.Count;
        this.Animations.Add(animation);
        this.AnimationsByName.Add(name, animation);
    }

    public struct Animation
    {
        public int ID;
        public string Name;
        public float Delay;
        public Vector2 Offset;
        public Vector2 Origin;
        public MTexture[] Frames;
    }
}
