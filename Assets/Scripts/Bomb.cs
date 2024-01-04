using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float m_BombExplosionDuration = 2.0f;
    private float m_StartTime;
    // Start is called before the first frame update
    void Start()
    {
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - m_StartTime >= m_BombExplosionDuration)
        {
            Debug.Log("EXPLOSION!");
            Destroy(gameObject);
        }
    }
}
