using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider m_VolumeSlider;
    void Awake()
    {
        Time.timeScale = 1.0f;
        m_VolumeSlider.value = GlobalVariables.MasterVolume;

        AudioListener.volume = m_VolumeSlider.value;
        m_VolumeSlider.onValueChanged.AddListener((v) =>
        {
            Debug.Log("New volume: " + v);
            GlobalVariables.MasterVolume = v;
            AudioListener.volume = v;
        });
    }
    public void PlayGame(int index)
    {
        GlobalVariables.ArenaMapIndex = index;
        Debug.Log("Starting game!");
        SceneManager.LoadScene("PlayScene");
    }

    public void StartMapEditor()
    {
        GlobalVariables.ArenaMapIndex = 4;
        Debug.Log("Starting map editor!");
        SceneManager.LoadScene("Editor");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
