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
    public int AudioNumber = 3;

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
        PlayMusic("BGM");        
    }

    // ���}���u
    private void Update()
    {
        if (AudioNumber == 3)
        {         
            if (Ex)
            {
                AudioNumber = 4;
                PlayMusic("BGM2");
                FindObjectOfType<GameManeger>().BPM = 136;
                GameManeger.TempoReset = true;
                Ex = false;
            }
        }
        if (AudioNumber == 4)
        {
            if (Ex)
            {
                AudioNumber = 5;
                PlayMusic("BGM3");
                FindObjectOfType<GameManeger>().BPM = 136;
                GameManeger.TempoReset = true;
                Ex=false;
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
