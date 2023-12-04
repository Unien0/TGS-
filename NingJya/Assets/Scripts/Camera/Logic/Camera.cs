using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;

    public CameraShake shake;
    public static bool ShakeOrder;
    // ステージ名ステータスの作成
    public enum StageName
    {
        Tutorial,
        Stage_1,
        Stage_2,
        Stage_3,
        end
    }
    // 現ステージの設定
    [SerializeField] private StageName Process;
    // 現在地
    public static float Order;
    // 最大値
    public static float Ordermax;

    [SerializeField] private GameObject DoPObj;
    [SerializeField] private Slider DoP;
    [SerializeField] private GameObject BOSSTEXT;

    void Start()
    {
        switch (Process)
        {
            case StageName.Tutorial:
                Ordermax = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShakeProcess();
        Degreeofprogress();
    }

    void Degreeofprogress()
    {
        // 進行度の取得・更新
        DoP.value = Order / Ordermax;

        if (DoP.value == 1)
        {
            if (FindObjectOfType<BossStartFlag>().ActStart)
            {
                DoPObj.SetActive(true);
                BOSSTEXT.SetActive(true);
            }
        }
    }

    void ShakeProcess()
    {
        if ((ShakeOrder) || (Input.GetKeyDown(KeyCode.Y)))
        {
            shake.Shake(0.25f, 0.1f);
            ShakeOrder = false;
        }
    }

}
