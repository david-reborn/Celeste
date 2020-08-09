using UnityEngine;
using System.Collections;
using System;

namespace myd.celeste.demo
{
    public class StateMachine 
    {
        private int state;
        private Action[] begins;
        private Func<int>[] updates;
        private Action[] ends;
        private Func<IEnumerator>[] coroutines;
        //private Coroutine currentCoroutine;
        public bool ChangedStates;
        public bool Log;
        public bool Locked;

        public int PreviousState { get; private set; }

        public StateMachine(int maxStates = 10) 
        {
            this.PreviousState = (this.state = -1);
            this.begins = new Action[maxStates];
            this.updates = new Func<int>[maxStates];
            this.ends = new Action[maxStates];
            this.coroutines = new Func<IEnumerator>[maxStates];
            //this.currentCoroutine = new Coroutine(true);
            //this.currentCoroutine.RemoveOnComplete = false;
        }

        public void SetCallbacks(int state, Func<int> onUpdate, Func<IEnumerator> coroutine = null, Action begin = null, Action end = null)
        {
            this.updates[state] = onUpdate;
            this.begins[state] = begin;
            this.ends[state] = end;
            //this.coroutines[state] = coroutine;
        }

        public void Update()
        {
            this.ChangedStates = false;
            bool flag = this.updates[this.state] != null;
            if (flag)
            {
                this.State = this.updates[this.state]();
            }
            //bool active = this.currentCoroutine.Active;
            //if (active)
            //{
            //    this.currentCoroutine.Update();
            //    bool flag2 = !this.ChangedStates && this.Log && this.currentCoroutine.Finished;
            //    if (flag2)
            //    {
            //        Calc.Log(new object[]
            //        {
            //            "Finished Coroutine " + this.state
            //        });
            //    }
            //}
        }

        public int State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.Locked || this.state == value)
                    return;
                if (this.Log)
                    Debug.Log(("Enter State " + value + " (leaving " + this.state + ")"));
                this.ChangedStates = true;
                this.PreviousState = this.state;
                this.state = value;
                if (this.PreviousState != -1 && this.ends[this.PreviousState] != null)
                {
                    if (this.Log)
                        Debug.Log(("Calling End " + this.PreviousState));
                    this.ends[this.PreviousState]();
                }
                if (this.begins[this.state] != null)
                {
                    if (this.Log)
                        Debug.Log(("Calling Begin " + this.state));
                    this.begins[this.state]();
                }
                //if (this.coroutines[this.state] != null)
                //{
                //    if (this.Log)
                //        Debug.Log((object)("Starting Coroutine " + (object)this.state));
                //    this.currentCoroutine.Replace(this.coroutines[this.state]());
                //}
                //else
                //    this.currentCoroutine.Cancel();
            }
        }
    }
}
