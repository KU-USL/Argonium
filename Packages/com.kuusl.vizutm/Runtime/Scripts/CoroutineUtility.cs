using System.Collections;
using UnityEngine;

public class CoroutineUtility : MonoBehaviour
{
    private static CoroutineUtility instance;
    public static CoroutineUtility Instance
    {
        get 
        { 
            if (instance == null)
            {
                GameObject runner = new GameObject("CoroutineUtility");
                instance = runner.AddComponent<CoroutineUtility>();
                DontDestroyOnLoad(runner);
            }
            return instance; 
        } 
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
