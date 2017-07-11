using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Section : MonoBehaviour
    {
        public float m_Speed = 1f;

	    void Update ()
        {
            transform.position += transform.forward * -(m_Speed * Time.deltaTime);	
	    }
    }
}

