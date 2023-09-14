using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //seを再生するとき、
    //AudioManager.Instance.PlaySE("xxx");
    //を追加してください（アニメのような）
    public static AudioManager Instance;

    public Sound[] musicSounds, seSounds;
    public AudioSource musicSounrce, seSource;

    // 応急処置
    public bool Ex = false;
    public GameObject PlayerObj;
    public int AudioNumber;
    [SerializeField] private AudioSource AudioSourceObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 応急処置
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        //ゲームのBGM
        //PlayMusic("BGM");
        Ex= true;
    }

    // 応急処置
    private void Update()
    {
        if (Ex)
        {
            switch (AudioNumber)
            {
                // BGM遷移用ボリュームダウン
                case 0:
                    if ()
                    {
                        AudioSourceObj.volume -= Time.deltaTime;
                    }
                    break;
                // チュートリアル
                case 1:
                    PlayMusic("BGM");
                    FindObjectOfType<GameManeger>().BPM = 126;
                    AudioSourceObj.volume = 1;
                    break;
                // インゲームBGM1
                case 2:
                    PlayMusic("BGM2");
                    FindObjectOfType<GameManeger>().BPM = 130;
                    AudioSourceObj.volume = 1;

                    break;
                // BossBGM1
                case 3:
                    PlayMusic("BGM3");
                    FindObjectOfType<GameManeger>().BPM = 136;
                    AudioSourceObj.volume = 1;
                    break;
            }
            GameManeger.TempoReset = true;
            Ex = false;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSounrce.clip = s.clip;
            musicSounrce.Play();
        }
     }

    public void PlaySE(string name)
    {
        Sound s = Array.Find(seSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("SE Not Found");
        }
        else
        {
            seSource.clip = s.clip;
            seSource.Play();
        }
    }
}
