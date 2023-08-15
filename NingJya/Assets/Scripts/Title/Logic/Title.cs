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

    public GameObject option;

    private PlayerInputActions controls;

    private void Awake()
    {
        controls = new PlayerInputActions();
        controls.GamePlay.ESC.started += ctx => Start();
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
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
        SceneManager.LoadScene(2);
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
