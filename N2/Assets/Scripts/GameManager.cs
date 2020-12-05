using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public SaveManager _saveManager;
    public static GameManager m_instance;

    // TODO: improve this later
    public GameObject m_canvasIngame;
    public GameObject m_menuIngame;
    public GameObject m_menuEnd;
    public GameObject m_menuMain;
    public Text m_menuEndText;
    public Slider m_healthSlider;

    public AudioClip m_sfxWon;
    public AudioClip m_sfxLost;
    private AudioSource m_audioSource;

    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        MakeSingleton();
    }

    public void SetHealthPercent(float value)
    {
        m_healthSlider.value = value;
    }

    public void ResetHealthPercent()
    {
        m_healthSlider.value = 1f;
    }

    public void NewGame()
    {
        _saveManager.ResetCheckpoint();
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        ResetHealthPercent();
        m_menuIngame.SetActive(false);
        m_menuEnd.SetActive(false);
        m_menuMain.SetActive(false);
        m_canvasIngame.SetActive(true);
    }

    public void PauseGame()
    {
        if (m_menuIngame.activeInHierarchy) {
            ResumeGame();
            return;
        }

        Time.timeScale = 0f;
        m_menuIngame.SetActive(true);
    }

    public void ResumeGame()
    {
        m_menuIngame.SetActive(false);
        Time.timeScale = 1f; // TODO: change this, may be different if the player is dying
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndGame(bool won = false)
    {
        if(won) {
            m_audioSource.PlayOneShot(m_sfxWon);
            m_menuEndText.text = "You won! For now.\nEnd of the demo :)";
        } else {
            m_audioSource.PlayOneShot(m_sfxLost);
            m_menuEndText.text = "You lost!\nTry again?";
        }
        Time.timeScale = 0f;
        m_menuEnd.SetActive(true);
    }

    private void MakeSingleton()
    {
        if (m_instance != null)
            Destroy(gameObject);
        else {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
