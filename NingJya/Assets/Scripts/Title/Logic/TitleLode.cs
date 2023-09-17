using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleLode : MonoBehaviour
{
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Anim.GetCurrentAnimatorStateInfo(0).IsName("End")) )
        {
            SceneManager.LoadScene(2);
            GameManeger.KillEnemy = 0;
            GameManeger.KillBOSS = 0;
            GameManeger.hitEnemy = 0;
            GameOver.GAMEOVER = false;
        }
    }
}
