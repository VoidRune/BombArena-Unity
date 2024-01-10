using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource m_PauseSound;
    public AudioSource m_ResumeSound;
    
    private bool m_IsPause = false;
    void Start()
    {
        ContinueGame();
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(!m_IsPause)
            {
                m_PauseSound.Play();
                PauseGame();
            }
            else
            {
                m_ResumeSound.Play();
                ContinueGame();
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
