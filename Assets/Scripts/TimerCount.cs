using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class TimerCount : MonoBehaviour
{
    private TMPro.TextMeshProUGUI m_TextMesh;
    private float m_StartTime;
    void Start()
    {
        m_TextMesh = GetComponent<TMPro.TextMeshProUGUI>();
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        m_TextMesh.text = (Time.time - m_StartTime).ToString("0.0");
    }
}
