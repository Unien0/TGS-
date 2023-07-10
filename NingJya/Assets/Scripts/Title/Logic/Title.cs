using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    }

    public void GameOver()
    {
        Application.Quit();//GameOver
    }
   
}
