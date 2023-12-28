using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    public float m_MovementSpeed;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 forward = new Vector3(0, 0, 1);
        Vector3 right = new Vector3(-1.0f, 0.0f, 0.0f);
        
        Vector3 velocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            velocity += forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += -forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += -right;
        }
        velocity =velocity.normalized * m_MovementSpeed;

        m_RigidBody.velocity = velocity;
    }
}
