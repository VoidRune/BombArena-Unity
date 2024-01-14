using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_Transform1;
    public Transform m_Transform2;
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

        Vector3 midpoint = (m_Transform1.position + m_Transform2.position) * 0.5f;

        float distance = (Vector3.Distance(m_Transform1.position, m_Transform2.position) / 7) + 4;
        Vector3 offset = new Vector3(
            0,
            Mathf.Tan(Mathf.PI / 3) * distance * 1.2f,
            -Mathf.Tan(60 * Mathf.Deg2Rad / 2) * distance * 2
        );

        //Vector2 player1Viewport = Camera.main.WorldToViewportPoint(m_Transform1.position);
        //Vector2 focusViewport = Camera.main.WorldToViewportPoint(midpoint);
        //float viewportDistance = Vector2.Distance(player1Viewport, focusViewport);

        transform.position = midpoint + offset;

        transform.LookAt(midpoint, Vector3.up);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_PostProcessing);
    }
}
