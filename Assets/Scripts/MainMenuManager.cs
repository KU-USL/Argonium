using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Pressed Start");
        SceneManager.LoadScene("Runtime");
    }

    public void ShowSettings()
    {
        Debug.Log("Pressed Settings");
        SceneManager.LoadScene("Settings");
    }

    public void ShowCredits()
    {
        Debug.Log("Pressed Credits");
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Debug.Log("Pressed Exit");
        Application.Quit();
    }

    public void SetURL(string url)
    {
        APIEnv.argonURL = url;
    }
}
