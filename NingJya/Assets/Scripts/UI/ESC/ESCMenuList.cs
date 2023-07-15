using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenuList : MonoBehaviour
{
    public GameObject escPlanel;

    private bool escOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //キーを押して起動
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlanelCon();
        }
        
    }

    public void PlanelCon()
    {
            if (!escOn)
            {
                escPlanel.SetActive(true);
                escOn = true;
                Time.timeScale = (0);//時間止めて
            }
            else
            {
                escPlanel.SetActive(false);
                escOn = false;
                Time.timeScale = (1);
            }
     }    

    public void ToTitle()
    {
        escPlanel.SetActive(false);
        escOn = false;
        Time.timeScale = (1);
        SceneManager.LoadScene("Title");
    }

    public void EneGame()
    {
        Application.Quit();//GameOver
    }
}
