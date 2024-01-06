using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float m_ExplosionDuration = 2.0f;
    public int m_FrameCountdown = 4;
    private float m_StartTime;
    // Start is called before the first frame update
    void Start()
    {
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - m_StartTime >= m_ExplosionDuration)
        {
            Destroy(gameObject);
        }

        if(m_FrameCountdown <= 0)
        {
            Destroy(gameObject.GetComponent<SphereCollider>());
        }
        m_FrameCountdown--;
    }
}
