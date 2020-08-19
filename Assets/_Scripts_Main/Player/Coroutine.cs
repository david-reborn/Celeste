﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coroutine 
{
    public bool RemoveOnComplete = true;
    public bool UseRawDeltaTime = false;
    private Stack<IEnumerator> enumerators;
    private float waitTimer;
    private bool ended;
    public bool Active { get; set;}
    public bool Finished { get; private set; }

    public Coroutine(IEnumerator functionCall, bool removeOnComplete = true)
    {
        this.Active = true;
        this.enumerators = new Stack<IEnumerator>();
        this.enumerators.Push(functionCall);
        this.RemoveOnComplete = removeOnComplete;
    }

    public Coroutine(bool removeOnComplete = true)
    {
        this.Active = false;
        this.RemoveOnComplete = removeOnComplete;
        this.enumerators = new Stack<IEnumerator>();
    }

    public void Update()
    {
        this.ended = false;
        if ((double)this.waitTimer > 0.0)
        {
            this.waitTimer -= Time.deltaTime;
        }
        else
        {
            if (this.enumerators.Count <= 0)
                return;
            IEnumerator enumerator = this.enumerators.Peek();
            if (enumerator.MoveNext() && !this.ended)
            {
                if (enumerator.Current is int)
                    this.waitTimer = (float)(int)enumerator.Current;
                if (enumerator.Current is float)
                    this.waitTimer = (float)enumerator.Current;
                else if (enumerator.Current is IEnumerator)
                    this.enumerators.Push(enumerator.Current as IEnumerator);
            }
            else if (!this.ended)
            {
                this.enumerators.Pop();
                if (this.enumerators.Count == 0)
                {
                    this.Finished = true;
                    this.Active = false;
                    //if (this.RemoveOnComplete)
                    //    this.RemoveSelf();
                }
            }
        }
    }

    public void Cancel()
    {
        this.Active = false;
        this.Finished = true;
        this.waitTimer = 0.0f;
        this.enumerators.Clear();
        this.ended = true;
    }

    public void Replace(IEnumerator functionCall)
    {
        this.Active = true;
        this.Finished = false;
        this.waitTimer = 0.0f;
        this.enumerators.Clear();
        this.enumerators.Push(functionCall);
        this.ended = true;
    }
}