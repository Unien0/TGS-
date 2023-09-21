using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoManeger : MonoBehaviour
{
    [SerializeField] private VideoPlayer VideoPlayer;
    [SerializeField] private VideoClip[] Videos;
    private bool isClose;
    private int RunbumInt;
    private bool itTime; 
    void Start()
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.loopPointReached += FinishPlayingVideo;
        RunbumInt = Random.Range(0, Videos.Length + 1);
        VideoPlayer.clip = Videos[RunbumInt];
        VideoPlayer.Play();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            itTime = true;

        }

        if (itTime)
        {
            TransitionRotate.isRotate = true;
            isClose = true;
        }

        if (isClose)
        {
            if (FindObjectOfType<TransitionRotate>().animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    public void FinishPlayingVideo(VideoPlayer vp)
    {
        itTime = true;
    }
}
