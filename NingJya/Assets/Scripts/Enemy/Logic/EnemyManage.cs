using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{
    List<GameObject> objList = new List<GameObject>();  
    
    public void Set(GameObject obj)
    {
        objList.Add(obj);
    }

    public void Delte(GameObject obj)
    {
        if(objList.Count > 0)
        {
            //if(objList.)
        }
    }
}
