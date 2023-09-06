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
    public CameraShake shake;
    public static bool ShakeOrder;
    public enum StageName
    {
        Tutorial,
        Stage_1,
        Boss_1,
        Stage_2,
        Boss_2,
        end
    }
    [SerializeField] private StageName Process;
    public static float Order;
    public static float Ordermax;
    [SerializeField] private GameObject[] Point;
    private bool Leave;


    [SerializeField] private GameObject DoPObj;
    [SerializeField] private Slider DoP;
    private bool DoPFix;
    private bool PosFix = false;
    [SerializeField] private GameObject BOSSTEXT;

    void Start()
    {
        StartPos = this.transform.position;
        DoPFix = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProcess();
        ShakeProcess();
        Degreeofprogress();
        Debug.Log(Order);
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
            DoPObj.SetActive(true);
            BOSSTEXT.SetActive(true);
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

    void MoveProcess()
    {
        // 移動入力 
        #region 移動入力
        if (playerObj.transform.position != playerBeforePos)
        {
            if (playerObj.GetComponent<Rigidbody2D>().velocity.x == 0)
            {
                input.x = 0;
            }
            else if (playerObj.GetComponent<Rigidbody2D>().velocity.x >= 0)
            {
                input.x = 1;
            }
            else if (playerObj.GetComponent<Rigidbody2D>().velocity.x <= 0)
            {
                input.x = -1;
            }
            if (playerObj.GetComponent<Rigidbody2D>().velocity.y == 0)
            {
                input.y = 0;
            }
            else if (playerObj.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                input.y = 1;
            }
            else if (playerObj.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                input.y = -1;
            }
            playerBeforePos = playerObj.transform.position;
        }
        else
        {
            input = Vector2.zero;
        }
        #endregion


        switch (Process)
        {
            case StageName.Tutorial:
                Ordermax = 3;
                switch (Order)
                {
                    case 0:
                        Order = 1;
                        DoPFix = true;
                        break;
                    case 1:
                        transform.position = Vector3.MoveTowards(transform.position, Point[0].transform.position, moveSpeed * input.y);
                        if (this.transform.position == Point[0].transform.position)
                        {
                            Order = 2;
                            Leave = false;
                            DoPFix = true;
                        }
                        break;
                    case 2:
                        transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, moveSpeed * input.y);
                        if (this.transform.position == Point[1].transform.position)
                        {
                            Order = 3;
                            Leave = false;
                            DoPFix = true;
                        }
                        break;
                    case 3:
                        if (this.transform.position != Point[2].transform.position)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, moveSpeed * input.y);
                        }
                        break;

                }
                break;

            case StageName.Stage_1:
                Ordermax = 5;
                switch (Order)
                {
                    case 0:
                        Order = 1;
                        DoPFix = true;
                        break;
                    case 1:
                        // 移動処理
                        if (this.transform.position == StartPos)
                        {
                            if (input.y == -1)
                            {
                                input.y = 0;
                            }
                        }
                        transform.position = Vector3.MoveTowards(transform.position, Point[0].transform.position,moveSpeed * input.y);

                        if (Leave)
                        {
                            if (this.transform.position == Point[0].transform.position)
                            {
                                Order = 2;
                                Leave = false;
                                DoPFix = true;
                            }
                        }
                        else
                        {
                            if (this.transform.position != Point[0].transform.position)
                            {
                                Leave = true;
                            }
                        }
                        break;
                    case 2:
                        if ((playerObj.transform.position.x - this.transform.position.x) > 8)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position,new Vector3 (Point[0].transform.position.x+ 16, Point[0].transform.position.y, Point[0].transform.position.z), 0.125f);
                            if (this.transform.position == new Vector3(Point[0].transform.position.x + 16, Point[0].transform.position.y, Point[0].transform.position.z))
                            {
                                Order = 3;
                                PosFix = false;
                                Leave = false;
                                DoPFix = true;
                            }
                        }
                        break;

                    case 3:
                        // 移動処理
                        transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, moveSpeed * input.x);

                        if (this.transform.position == Point[1].transform.position)
                        {
                            Order = 4;
                            Leave = false;
                            DoPFix = true;
                        }
                        if (this.transform.position == Point[0].transform.position)
                        {
                            input.x = 0;
                        }
                        break;

                    case 4:
                        if ((playerObj.transform.position.y - this.transform.position.y) < -4)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Point[1].transform.position.x, Point[1].transform.position.y - 8, Point[1].transform.position.z), 0.125f);
                            if (this.transform.position == new Vector3(Point[1].transform.position.x, Point[1].transform.position.y - 8, Point[1].transform.position.z))
                            {
                                DoPFix = true;
                                Order = 5;
                                PosFix = false;
                                Leave = false;
                            }
                        }
                        break;

                    case 5:
                        // 移動処理
                        transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, moveSpeed * -input.y);

                        if (Leave)
                        {
                            if (this.transform.position == Point[2].transform.position)
                            {
                                Order = 6;
                                Leave = false;
                                DoPFix = true;
                            }
                            if (this.transform.position == Point[1].transform.position)
                            {
                                input.y = 0;
                            }
                        }
                        else
                        {
                            if (this.transform.position != Point[1].transform.position)
                            {
                                Leave = true;
                            }
                        }
                        break;
                    case 6:
                        // 移動処理
                        if (this.transform.position != Point[3].transform.position)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[3].transform.position, moveSpeed);
                        }
                        break;
                }
                break;
        }

        // プレイヤーがカメラ中央から離れていた場合

    }

}
