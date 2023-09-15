using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //se���Đ�����Ƃ��A
    //AudioManager.Instance.PlaySE("xxx");
    //��ǉ����Ă��������i�A�j���̂悤�ȁj
    public static AudioManager Instance;

    public Sound[] musicSounds, seSounds;
    public AudioSource musicSounrce, seSource;

    // ���}���u
    public bool Ex = false;
    public GameObject PlayerObj;
    public int AudioNumber;
    [SerializeField] private AudioSource AudioSourceObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // ���}���u
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        //�Q�[����BGM
        //PlayMusic("BGM");
        Ex= true;
    }

    // ���}���u
    private void Update()
    {
        if (Ex)
        {
            switch (AudioNumber)
            {
                // BGM�J�ڗp�{�����[���_�E��
                case 0:
                    AudioSourceObj.volume -= Time.deltaTime;
                    if (AudioSourceObj.volume <= 0.1f)
                    {
                        Ex = false;
                    }
                    break;
                // �`���[�g���A��
                case 1:
                    PlayMusic("BGM");
                    FindObjectOfType<GameManeger>().BPM = 126;
                    AudioSourceObj.volume = 0.8f;
                    GameManeger.TempoReset = true;
                    Ex = false;
                    break;
                // �C���Q�[��BGM1
                case 2:
                    PlayMusic("BGM2");
                    FindObjectOfType<GameManeger>().BPM = 130;
                    AudioSourceObj.volume = 0.8f;
                    GameManeger.TempoReset = true;
                    Ex = false;
                    break;
                // BossBGM1
                case 3:
                    PlayMusic("BGM3");
                    FindObjectOfType<GameManeger>().BPM = 136;
                    AudioSourceObj.volume = 0.8f;
                    GameManeger.TempoReset = true;
                    Ex = false;
                    break;
            }

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
