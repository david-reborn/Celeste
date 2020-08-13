using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            Collider2D test= Physics2D.OverlapPoint(new Vector2(1, 1));
            if (test != null)
            {
                Debug.Log(test);
            }
            else
            {
                Debug.Log("No Collider");
            }
        }
    }
}
