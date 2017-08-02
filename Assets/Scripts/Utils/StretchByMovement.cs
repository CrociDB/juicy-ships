using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchByMovement : MonoBehaviour
{
    public float m_Intensity;

    private Vector3 m_LastPosition;
    private Vector3 m_Scale;

    private void Update()
    {
        if (Time.frameCount % 2 == 0)
        {
            var mov = transform.position - m_LastPosition;
            mov = new Vector3(Mathf.Abs(mov.x), Mathf.Abs(mov.y), Mathf.Abs(mov.z));
            var mag = mov.magnitude * m_Intensity;
            var dir = mov.normalized - Vector3.one * .2f * mag;
            m_Scale = Vector3.one + (dir * mag);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, m_Scale, Time.deltaTime * 10f);
        m_LastPosition = transform.position;
    }
}
