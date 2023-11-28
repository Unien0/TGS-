using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    [SerializeField] private GameObject optionClosedButton;
    private float lodeTime;
    public GameObject option;

    private PlayerInputActions controls;
    private AudioSource audioSource;
    [SerializeField]private AudioClip startClip;
    private bool LoadIt;

    private float DemoLodeTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controls = new PlayerInputActions();
        controls.GamePlay.ESC.started += ctx => Start();
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    private void Update()
    {
        if (LoadIt)
        {
            DemoLodeTime = 0;
            lodeTime += Time.deltaTime;
            if (lodeTime >= 3)
            {
                lodeTime = 0;
                TransitionRotate.isRotate = true;
            }
            if (!audioSource.isPlaying)
            {
                StoryScenes.NextStageNum = 0;
                GameManeger.Score = 0;
                SceneManager.LoadScene("StoryScene");
            }
        }
        else
        {
            DemoLodeTime += Time.deltaTime;
            if (DemoLodeTime >= 20)
            {
                SceneManager.LoadScene("DemoScene");
            }
        }
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    public void NewGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        audioSource.clip = startClip;
        audioSource.Play();
        LoadIt = true;
    }

    public void Option()
    {
        if (!option.activeInHierarchy)
        {
            option.SetActive(true);
            AudioManager.Instance.PlaySE("Button1");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
        }
        else
        {
            option.SetActive(false);
            AudioManager.Instance.PlaySE("Button2");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }
        
    }

    public void GameOver()
    {
        Application.Quit();//GameOver
    }

}
