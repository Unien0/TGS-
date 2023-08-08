using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ESCMenuList : MonoBehaviour
{
    public GameObject escPlanel;

    private bool escOn;

    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;

    public GameObject option;

    private PlayerInputActions controls;

    private void Awake()
    {
        controls = new PlayerInputActions();
        controls.GamePlay.ESC.started += ctx => PlanelCon();
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }
  
    public void PlanelCon()
    {
            if (!escOn)
            {
            AudioManager.Instance.PlaySE("Button1");
            escPlanel.SetActive(true);
            escOn = true;
            Time.timeScale = (0);//ŽžŠÔŽ~‚ß‚Ä
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
            }
            else
            {
            AudioManager.Instance.PlaySE("Button2");
            escPlanel.SetActive(false);
                escOn = false;
                Time.timeScale = (1);
            EventSystem.current.SetSelectedGameObject(null);
        }
     }    

    public void ToTitle()
    {
        AudioManager.Instance.PlaySE("Button1");
        escPlanel.SetActive(false);
        escOn = false;
        Time.timeScale = (1);
        SceneManager.LoadScene("Title");
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

    public void EneGame()
    {
        Application.Quit();//GameOver
    }
}
