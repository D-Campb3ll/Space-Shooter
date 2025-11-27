using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonSoundClip;
    private AudioSource _audioSource;
    [TextArea(3, 10)]
    public string _developerNotes;


    private void Start()
    {

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Main Menu is NULL");
        }
    }
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void LoadMultiplayerGame()
    {
        SceneManager.LoadScene(2);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayButtonSound()
    {
        _audioSource.PlayOneShot(_buttonSoundClip);
    }

}
