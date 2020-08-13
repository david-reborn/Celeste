using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myd.celeste.demo;
public class Test : MonoBehaviour
{

    public Vector2 testPosition;
    public Vector2 testCenter;
    public Vector2 testSize;
    public LayerMask mask;
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("test:"+ColliderUtil.CollideCheck(testPosition, mask));
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            Debug.Log("test:" + (Physics2D.OverlapBox(testCenter, testSize, 0, mask)));
        }
    }
}
