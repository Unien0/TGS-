using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartManeger : MonoBehaviour
{
    // リスポーンポイントの保存(親機)
    public static Vector3 ReSpornPoint;

    public static float SaveOrder;

    // リスポーンの許可
    public static bool CanReSporn;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
