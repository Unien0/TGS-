using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    private Vector3 playerBeforePos;
    private Vector3 StartPos;
    [SerializeField] private float moveSpeed;
    private Vector2 input;

    public static float ReStartNumber;

    public CameraShake shake;
    public static bool ShakeOrder;
    public enum StageName
    {
        Tutorial,
        Tutorial_New,
        Stage_1,
        Stage_1_New,
        Stage_2,
        Stage_3,
        end
    }
    [SerializeField] private StageName Process;
    public static float Order;
    public static float Ordermax;
    [SerializeField] private GameObject[] Point;
    [SerializeField] private GameObject[] ProceedBlocks;
    [SerializeField] private GameObject Smoke;
    private bool Leave;


    [SerializeField] private GameObject DoPObj;
    [SerializeField] private Slider DoP;
    private bool DoPFix;
    private bool PosFix = false;
    [SerializeField] private GameObject BOSSTEXT;
   
    private Vector2 StayPoint;
    private bool fixX;
    private bool fixY;
    private bool fix_now;

    void Start()
    {
        StartPos = this.transform.position;
        DoPFix = true;
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
        if (DoPFix)
        {
            DoP.value = Order / Ordermax;
            DoPFix = false;
        }

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
