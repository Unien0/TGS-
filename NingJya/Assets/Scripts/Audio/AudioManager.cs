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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //ゲームのBGM
        PlayMusic("BGM");

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
