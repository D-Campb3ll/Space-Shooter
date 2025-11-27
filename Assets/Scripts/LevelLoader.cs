using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator _transition;
    public float _transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadSinglePlayer()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void BackToMenu()
    {
        Debug.Log("Main Menu Loading");
        StartCoroutine(LoadLevel(0));
    }

    public void LoadMultiplayer()
    {
        StartCoroutine(LoadLevel(2));
    }

    public void LoadBossFight()
    {
        StartCoroutine(LoadLevel(3));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
