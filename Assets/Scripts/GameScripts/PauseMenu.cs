using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource m_PauseSound;
    public AudioSource m_ResumeSound;

    public AudioSource m_BackgroundMusic;

    private bool m_IsPause = false;
    void Start()
    {
        ContinueGame();
        m_BackgroundMusic.Play();
        m_BackgroundMusic.loop = true;
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(!m_IsPause)
            {
                m_BackgroundMusic.Pause();
                m_PauseSound.Play();
                PauseGame();
            }
            else
            {
                m_ResumeSound.Play();
                ContinueGame();
                m_BackgroundMusic.Play();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        m_IsPause = true;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1.0f;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        m_IsPause = false;
    }

    public void ToMainMenu()
    {
        ContinueGame();
        Debug.Log("Back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}