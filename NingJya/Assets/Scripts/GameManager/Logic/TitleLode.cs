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
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Anim.GetCurrentAnimatorStateInfo(0).IsName("End")) || (Input.anyKeyDown))
        {
            SceneManager.LoadScene(1);
        }
    }
}
