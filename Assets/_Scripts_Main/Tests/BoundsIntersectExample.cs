using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsIntersectExample : MonoBehaviour
{
    public GameObject m_MyObject, m_NewObject;
    Collider m_Collider, m_Collider2;

    void Start()
    {
        //Check that the first GameObject exists in the Inspector and fetch the Collider
        if (m_MyObject != null)
            m_Collider = m_MyObject.GetComponent<Collider>();

        //Check that the second GameObject exists in the Inspector and fetch the Collider
        if (m_NewObject != null)
            m_Collider2 = m_NewObject.GetComponent<Collider>();
    }

    void Update()
    {
        //If the first GameObject's Bounds enters the second GameObject's Bounds, output the message
        if (m_Collider.bounds.Intersects(m_Collider2.bounds))
        {
            Debug.Log("Bounds intersecting");
        }
    }
}
