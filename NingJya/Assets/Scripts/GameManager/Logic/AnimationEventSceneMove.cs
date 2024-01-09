using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEventSceneMove : MonoBehaviour
{
    public void ToStage1()
    {
        SceneManager.LoadScene("1stStage_Remake");
    }

    public void ToStage2()
    {
        SceneManager.LoadScene("2ndStage");
    }
}
