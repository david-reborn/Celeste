using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace myd.celeste
{
    /// <summary>
    /// 背景渲染器
    /// </summary>
    public class BackdropRenderer : MonoBehaviour
    {
        public List<Backdrop> Backdrops = new List<Backdrop>();
        public float Fade = 0.0f;
        public Color FadeColor = Color.black;
        private bool usingSpritebatch;

        public void Update()
        {
            //    foreach (Backdrop backdrop in this.Backdrops)
            //        backdrop.Update(scene);
        }
    }

}
