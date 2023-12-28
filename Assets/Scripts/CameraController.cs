using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_Transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0, 6, -3.5f);
        transform.position = m_Transform.position + offset;


        transform.LookAt(m_Transform.position, Vector3.up);
    }
}
