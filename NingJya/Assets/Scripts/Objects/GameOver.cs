using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Animator anim;
    public static bool GAMEOVER;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GAMEOVER)
        {
            anim.SetBool("isEnd", true);
        }
        else
        {
            anim.SetBool("isEnd", false);
        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("imageStay")) && (Input.anyKeyDown))
        {
            SceneManager.LoadScene(1); 
            anim.SetBool("isEnd", false);
            GAMEOVER = false;
            FindObjectOfType<Player>().hp = FindObjectOfType<Player>().maxHp;
        }
    }
}
