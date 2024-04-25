using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 
    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    public void OnNewGameButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene("HeightTest 1");
        Debug.Log("Reset button clicked");
    }

}
