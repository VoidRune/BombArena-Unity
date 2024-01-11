using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMovement : MonoBehaviour
{
    public float m_MovementSpeed = 5.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
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
        velocity = velocity.normalized * m_MovementSpeed * Time.deltaTime;

        gameObject.transform.position += velocity;
    }
}
