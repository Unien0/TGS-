using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionRotate : MonoBehaviour
{
    private Animator animator;
    public static bool isRotate;
    private bool nowRotate = true;
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            animator.SetBool("isRotate",!nowRotate);
            nowRotate = !nowRotate;
            isRotate = false;
        }

    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        Camera.Order = 0;
    }
}
