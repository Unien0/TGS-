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
    [SerializeField] private GameObject ExchangePoint1;
    [SerializeField] private GameObject ExchangePoint2;
    Vector2 Gap;
    public GameObject PlayerObj;
    private int AudioNumber = 3;

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

    // 応急処置
    private void Update()
    {
        if (AudioNumber == 3)
        {
            
            Gap = new Vector2(ExchangePoint1.transform.position.x - PlayerObj.transform.position.x, ExchangePoint1.transform.position.y - PlayerObj.transform.position.y);
            float vec1 = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y);
            if (vec1 <= 5)
            {
                
                AudioNumber = 4;
                PlayMusic("BGM2");
                FindObjectOfType<GameManeger>().BPM = 136;
                GameManeger.TempoReset = true;
            }
        }
        if (AudioNumber == 4)
        {
            Gap = new Vector2(ExchangePoint2.transform.position.x - PlayerObj.transform.position.x, ExchangePoint2.transform.position.y - PlayerObj.transform.position.y);
            float vec2 = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y);
            if (vec2 <= 5)
            {
                AudioNumber = 5;
                PlayMusic("BGM3");
                FindObjectOfType<GameManeger>().BPM = 136;
                GameManeger.TempoReset = true;
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
