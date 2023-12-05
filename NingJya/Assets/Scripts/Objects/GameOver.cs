using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    private Animator anim;
    public static bool StageCLEAR;
    public static bool GAMEOVER;
    [SerializeField]private int NextScene;
    // タイトルに戻るかどうかの処理
    [SerializeField] private bool GameClear;
    [SerializeField] private GameObject TransitionObj;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StageCLEAR)
        {
            anim.SetBool("isClear", true);
        }
        else if (GAMEOVER)
        {
            anim.SetBool("isEnd", true);
            TransitionObj.GetComponent<Animator>().SetBool("isRotate", false);
        }
        else
        {
            anim.SetBool("isClear", false);
            anim.SetBool("isEnd", false);
        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("imageStay")) && (Input.anyKeyDown))
        {
            if (GameClear)
            {
                SceneManager.LoadScene(2);
                StageCLEAR = false;
            }
            else
            {
                StoryScenes.NextStageNum = NextScene;
                SceneManager.LoadScene(3);
                StageCLEAR = false;
            }

        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("GameOver")) && (Input.anyKeyDown))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GAMEOVER = false;
            GameManeger.Score = GameManeger.Score / 2;
        }
    }
}
