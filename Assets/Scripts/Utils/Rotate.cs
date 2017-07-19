using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 m_EulerRotation;

    public void Update()
    {
        transform.Rotate(m_EulerRotation);
    }
}
