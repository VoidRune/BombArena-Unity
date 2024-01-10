using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCystalMovement : MonoBehaviour
{
    private float m_RotationSpeed;
    private float m_RandomOffset;
    void Start()
    {
        gameObject.transform.Rotate(0, Random.Range(0.0f, 180.0f), 0);
        m_RotationSpeed = 30.0f;
        m_RandomOffset = Random.Range(0.0f, 3.1415f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y = Mathf.Sin(Time.time + m_RandomOffset);
        gameObject.transform.position = pos;
        gameObject.transform.Rotate(0, Time.deltaTime * m_RotationSpeed, 0);
    }
}
