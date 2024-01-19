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
    // �^�C�g���ɖ߂邩�ǂ����̏���
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
            
            TransitionObj.GetComponent<Animator>().SetBool("isRotate", false);
            if (TransitionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                anim.SetBool("isEnd", true);
            }
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
                SceneMovement.MoveStageNum = NextScene;
                SceneManager.LoadScene(3);
                //SceneManager.LoadScene("SceneMovement");
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
