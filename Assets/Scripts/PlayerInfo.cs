using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public GameObject menuScreenUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            menuScreenUI.SetActive(!menuScreenUI.activeSelf);
            if (!menuScreenUI) {
                Cursor.lockState = CursorLockMode.Locked;

            } else { Cursor.lockState = CursorLockMode.None;}
            Debug.Log("Menu open");
        } 

     
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    public void OnRestartGameButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset button clicked");
    }
}

