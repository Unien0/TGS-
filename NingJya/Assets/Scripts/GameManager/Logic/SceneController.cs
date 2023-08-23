using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class SceneController : MonoBehaviour
{
    private Flowchart flowchart;
    public string sceneName;
    private void Awake()
    {
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    private void Update()
    {
        if (flowchart.GetBooleanVariable("ToInGame"))
        {
            SceneChange();
            
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }
}
