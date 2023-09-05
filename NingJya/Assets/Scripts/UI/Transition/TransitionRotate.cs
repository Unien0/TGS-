using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionRotate : MonoBehaviour
{
    public Animator animator;
    public static bool isRotate;
    private bool nowRotate = true;
    [SerializeField]private bool isMove;
    [SerializeField] private GameObject Player;
    public bool Delay;
    [SerializeField] private float DelayTime;
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += SceneLoaded;
        if (!Delay)
        {
            DelayTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DelayTime -= Time.deltaTime;
        if (!(DelayTime >0))
        {
            Delay = false;
            animator.enabled = true;
            if (isRotate)
            {
                animator.SetBool("isRotate", !nowRotate);
                nowRotate = !nowRotate;
                isRotate = false;
            }

            if (isMove)
            {
                GetComponent<RectTransform>().position = Player.transform.position;
            }
        }
        else
        {
            animator.enabled = false;
        }
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        Camera.Order = 0;
    }
}
