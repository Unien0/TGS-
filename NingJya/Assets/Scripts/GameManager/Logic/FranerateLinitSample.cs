using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FranerateLinitSample : MonoBehaviour
{
   public enum LimitType
    {
        NoLimit = -1,
        Limit5 = 5,
        Limit15 = 15,
        Limit30 = 30,
        Limit70 = 70,
        Limit120 = 120,
    }
    public LimitType limitType;

    private void Awake()
    {
        Application.targetFrameRate = (int)limitType;
    }
}
