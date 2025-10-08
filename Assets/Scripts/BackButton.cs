using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TO DO: Consolidate function with another class, and remove this class
public class BackButton : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Debug.Log("Pressed Back");
        SceneManager.LoadScene("MainMenu");
    }
}
