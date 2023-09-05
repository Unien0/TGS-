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
    public static int NextStageNum;
    private int LodeSceneNum;
    [SerializeField]private GameObject trans;
    private bool isClose;

    void Awake()
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.loopPointReached += FinishPlayingVideo;
    }

    void Start()
    {
        VideoPlayer.clip = Videos[NextStageNum];
        Back.sprite = Images[NextStageNum];
        switch (NextStageNum)
        {
            case 0:
                LodeSceneNum = 4;                
                break;
        }
        VideoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            if (FindObjectOfType<TransitionRotate>().animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                SceneManager.LoadScene(LodeSceneNum);
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
