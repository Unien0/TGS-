using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Animator anim;
    public static bool GAMECLEAR;
    public static bool GAMEOVER;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GAMECLEAR)
        {
            anim.SetBool("isClear", true);
        }
        else if (GAMEOVER)
        {
            anim.SetBool("isEnd", true);
        }
        else
        {
            anim.SetBool("isClear", false);
            anim.SetBool("isEnd", false);
        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("imageStay")) && (Input.anyKeyDown))
        {
            SceneManager.LoadScene(1);
            GAMECLEAR = false;
            FindObjectOfType<Player>().hp = FindObjectOfType<Player>().maxHp;
        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("GameOver")) && (Input.anyKeyDown))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GAMEOVER = false;
            ReStartManeger.CanReSporn = true;
        }
    }
}
