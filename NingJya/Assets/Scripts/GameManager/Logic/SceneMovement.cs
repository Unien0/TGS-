using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMovement : MonoBehaviour
{
    public static int MoveStageNum;
    private int GoSceneNum;
    private Animator anim;
    private float Times;
    private bool isClose;
    void Start()
    {
        anim = GetComponent<Animator>();
        switch (MoveStageNum)
        {
            case 0:
                GoSceneNum = 4;
                break;
            case 1:
                GoSceneNum = 5;
                anim.SetBool("ToStage1", true);
                break;
            case 2:
                GoSceneNum = 6;
                anim.SetBool("ToStage2", true);
                break;
        }
    }

    void Update()
    {
        if (isClose)
        {
            if (FindObjectOfType<TransitionRotate>().animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                Times += Time.deltaTime;
                if (Times > 1)
                {
                    Debug.Log(MoveStageNum);
                    SceneManager.LoadScene(GoSceneNum);
                }
            }
        }
        else
        {
            Times += Time.deltaTime;
            if (Times > 5)
            {
                TransitionRotate.isRotate = true;
                isClose = true;
                Times = 0;
            }
        }
    }
}
