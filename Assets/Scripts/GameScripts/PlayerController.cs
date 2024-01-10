using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    public float m_MovementSpeed;

    public ArenaBuilder m_ArenaBuilderScript;

    private GameObject m_LastBombPlaced;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);


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
        velocity = velocity.normalized * m_MovementSpeed;

        m_RigidBody.velocity = velocity;

        Vector3 pos = gameObject.transform.position;
        if (m_LastBombPlaced)
        {
            Vector3 bombPos = m_LastBombPlaced.transform.position;
            if(Mathf.Pow(pos.x - bombPos.x, 2) + Mathf.Pow(pos.z - bombPos.z, 2) > 0.6)
            {
                Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), false);
                m_LastBombPlaced = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = (int)(gameObject.transform.position.x + 0.5f);
            int z = (int)(gameObject.transform.position.z + 0.5f);

            GameObject b = m_ArenaBuilderScript.TryPlaceBomb(x, z);

            if(b != null)
            {
                Debug.Log("Bomb dropped " + x + " " + z);
                // Enable collsion for last bomb
                if(m_LastBombPlaced)
                    Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), false);

                m_LastBombPlaced = b;
                Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            m_MovementSpeed += 5.0f;
            Debug.Log("PowerUp Hit");
        }

        if (other.tag == "Explosion")
        {
            Debug.Log("Player died!");
        }
    }
}
