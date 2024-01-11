using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_Transform;
    public Material m_PostProcessing;

    public float m_BackgroundScale = 1.0f;
    public float m_Time = 0.02f;

    // Start is called before the first frame update
    void Start()
    {

        m_PostProcessing.SetFloat("_BackgroundScale", m_BackgroundScale);
        m_PostProcessing.SetFloat("_TimeScale", m_Time);
    }

    // Update is called once per frame
    void Update()
    {
        //m_PostProcessing.SetFloat("_BackgroundScale", m_BackgroundScale);
        //m_PostProcessing.SetFloat("_TimeScale", m_Time);

        Vector3 offset = new Vector3(0, 6, -3.5f);
        transform.position = m_Transform.position + offset;


        transform.LookAt(m_Transform.position, Vector3.up);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_PostProcessing);
    }
}
