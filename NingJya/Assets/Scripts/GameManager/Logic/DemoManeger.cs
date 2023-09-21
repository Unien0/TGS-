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
    private bool isPlay;
    void Start()
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.loopPointReached += FinishPlayingVideo;
    }

    void Update()
    {
        if (!isPlay)
        {
            RunbumInt = Random.Range(0, Videos.Length + 1);
            VideoPlayer.clip = Videos[RunbumInt];
            VideoPlayer.Play();
            isPlay = true;
        }

        if (Input.anyKeyDown)
        {
            itTime = true;

        }

        if (itTime)
        {
            TransitionRotate.isRotate = true;
            isClose = true;
            itTime = false;
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
