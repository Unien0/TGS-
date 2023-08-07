using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;

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
        SceneManager.LoadScene("SampleScene");
    }

    public void Option()
    {
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    public void GameOver()
    {
        Application.Quit();//GameOver
    }

}
