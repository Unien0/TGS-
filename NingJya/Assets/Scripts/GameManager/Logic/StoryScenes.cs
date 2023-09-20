using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryScenes : MonoBehaviour
{
    [SerializeField] private VideoPlayer VideoPlayer;
    [SerializeField] private VideoClip[] Videos;
    [SerializeField] private Sprite[] Images;
    [SerializeField] private SpriteRenderer Back;
    private AudioSource audioSource;
    public static int NextStageNum;
    private int LodeSceneNum;
    private bool isPlay;
    [SerializeField]private GameObject trans;
    private bool isClose;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.loopPointReached += FinishPlayingVideo;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<TransitionRotate>().Delay == false)
        {         
            if (!isPlay)
            {
                VideoPlayer.clip = Videos[NextStageNum];
                Back.sprite = Images[NextStageNum];
                
                switch (NextStageNum)
                {
                    case 0:
                        LodeSceneNum = 4;
                        break;                    
                    case 1:
                        LodeSceneNum = 5;
                        break;                    
                    case 2:
                        LodeSceneNum = 6;
                        break;
                }

                isPlay = true;
                VideoPlayer.Play();
            }
        }


        if (isClose)
        {
            if (FindObjectOfType<TransitionRotate>().animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                audioSource.volume -= Time.deltaTime/2.5f;
                if (!audioSource.isPlaying)
                {                    
                    SceneManager.LoadScene(LodeSceneNum);
                }
            }
        }
    }
    public void FinishPlayingVideo(VideoPlayer vp)
    {
        VideoPlayer.Stop();
        TransitionRotate.isRotate = true;
        isClose = true;
    }
}
