using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRhythm : MonoBehaviour
{
    public GameObject rhythmObject;
    public GameObject ParentObject;
    // Update is called once per frame
    void Update()
    {
        if (GameManeger.TempoExChange)
        {
            Instantiate(rhythmObject, ParentObject.transform, false);
        }
    }
}
