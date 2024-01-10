using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{

    public Material m_PostProcessing;
    public Camera m_Camera;
    public Slider m_EpsilonSlider;

    [Range(0.0f, 10.0f)] public float m_Power = 8.0f;
    [Range(0.00002f, 0.05f)] public float m_Epsilon = 0.0006f;

    public Color m_ColorA;
    public Color m_ColorB;

    [Range(1.0f, 10.0f)] public float m_MaxDistance = 4.0f;
    [Range(1, 500)] public int m_MaxIterations = 200;

    [Range(0.0f, 500.0f)] public float m_Darkness = 0.0f;
    [Range(0.0f, 1.0f)] public float m_BlackWhite = 0.0f;

    [Range(1.0f, 2.5f)] public float m_Radius = 1.6f;

    private float x;
    private float y;

    void SetParameters()
    {
        m_PostProcessing.SetMatrix("_CameraInverseView", m_Camera.worldToCameraMatrix.inverse);
        m_PostProcessing.SetMatrix("_CameraInverseProjection", m_Camera.projectionMatrix.inverse);

        m_PostProcessing.SetVector("_LightDirection", new Vector3(-1, -1, -1));
        m_PostProcessing.SetVector("_ColourAMix", new Vector3(m_ColorA.r, m_ColorA.g, m_ColorA.b));
        m_PostProcessing.SetVector("_ColourBMix", new Vector3(m_ColorB.r, m_ColorB.g, m_ColorB.b));

        m_PostProcessing.SetFloat("_Power", m_Power);
        m_PostProcessing.SetFloat("_Epsilon", m_Epsilon);

        m_PostProcessing.SetFloat("_MaxDist", m_MaxDistance);
        m_PostProcessing.SetInt("_MaxIter", m_MaxIterations);

        m_PostProcessing.SetFloat("_Darkness", m_Darkness);
        m_PostProcessing.SetFloat("_BlackWhite", m_BlackWhite);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 mousePos = Input.mousePosition;
        x = 1.4f * ((mousePos.x / Screen.width) * 2.0f - 1.0f);
        y = 1.4f * ((mousePos.y / Screen.height) * 2.0f - 1.0f);

        SetParameters();

        m_EpsilonSlider.value = m_Epsilon;
        m_EpsilonSlider.onValueChanged.AddListener((v) =>
        {
            m_Epsilon = v;
        });
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        float desiredX = 1.4f * ((mousePos.x / Screen.width ) * 2.0f - 1.0f);
        float desiredY = 1.4f * ((mousePos.y / Screen.height) * 2.0f - 1.0f);

        x = x + (desiredX - x) * 0.1f;
        y = y + (desiredY - y) * 0.1f;

        float equatorX = Mathf.Cos(x);
        float equatorZ = Mathf.Sin(x);
        float sphereY = m_Radius * Mathf.Sin(y);
        float multiplier = Mathf.Cos(y);
        float sphereX = m_Radius * multiplier * equatorX;
        float sphereZ = m_Radius * multiplier * equatorZ;

        m_Camera.transform.position = new Vector3(sphereX, sphereY, sphereZ);
        m_Camera.transform.LookAt(Vector3.zero, Vector3.up);

        m_Power = Mathf.Sin(Time.time * 0.02f) * 3.0f + 5.0f;

        SetParameters();
    }
}
