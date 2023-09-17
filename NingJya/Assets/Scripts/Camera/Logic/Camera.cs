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
    private bool Leave;


    [SerializeField] private GameObject DoPObj;
    [SerializeField] private Slider DoP;
    private bool DoPFix;
    private bool PosFix = false;
    [SerializeField] private GameObject BOSSTEXT;
   
    private Vector2 StayPoint;
    private bool fixX;
    private bool fixY;

    void Start()
    {
        StartPos = this.transform.position;
        DoPFix = true;

        if (ReStartNumber != 0)
        {
            switch (Process)
            {
                case StageName.Stage_1:
                    switch(ReStartNumber)
                    {
                        case 1:
                            Order = 1;
                            transform.position = Point[0].transform.position;
                            break;
                        case 2:
                            Order = 4;
                            transform.position = new Vector3(Point[1].transform.position.x, Point[1].transform.position.y - 8, Point[1].transform.position.z);
                            break;
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveProcess();
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
                Ordermax = 2;
                StayPoint = new Vector2(this.transform.position.x, this.transform.position.y - 3f);
                switch (Order)
                {
                    case 0:
                        fixX = false;
                        fixY = true;
                        transform.position = Vector3.MoveTowards(transform.position, Point[0].transform.position, moveSpeed * input.y);
                        if (this.transform.position == Point[0].transform.position)
                        {
                            Order = 1;
                            Leave = false;
                            DoPFix = true;
                        }
                        break;
                    case 1:
                        fixX = false;
                        fixY = true;
                        transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, moveSpeed * input.y);
                        if (this.transform.position == Point[1].transform.position)
                        {
                            Order = 2;
                            Leave = false;
                            DoPFix = true;
                        }
                        break;
                    case 2:
                        fixX = false;
                        fixY = false;
                        if (this.transform.position != Point[2].transform.position)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, moveSpeed * input.y);
                        }
                        break;

                }
                break;

            case StageName.Tutorial_New:
                Ordermax = 7;
                switch (Order)
                {
                    case 0:
                        fixX = false;
                        fixY = true;
                        StayPoint = new Vector2(this.transform.position.x, this.transform.position.y - 1.35f);
                        if (this.transform.position == StartPos)
                        {
                            if (input.y == -1)
                            {
                                input.y = 0;
                            }
                        }
                        transform.position = Vector3.MoveTowards(transform.position, Point[0].transform.position, moveSpeed * input.y);
                        if (Leave)
                        {
                            if (this.transform.position == Point[0].transform.position)
                            {
                                StayPoint = Vector2.zero;
                                Order = 1;
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
                    case 1:
                        if ((Mathf.Abs(playerObj.transform.position.x) - Mathf.Abs(this.transform.position.x)) < -4f)
                        {
                            PosFix = true;

                        }
                        else
                        {
                            fixX = false;
                            fixY = false;
                            StayPoint = Vector2.zero;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, 0.125f);
                            if (this.transform.position == Point[1].transform.position)
                            {
                                StayPoint = Vector2.zero;
                                Order = 2;
                                Leave = false;
                                DoPFix = true;
                                PosFix = false;
                            }
                        }
                        break;
                    case 2:
                        if (ProceedBlocks[0].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, 0.125f);
                            if (this.transform.position == Point[2].transform.position)
                            {
                                Order = 3;
                                Leave = false;
                                DoPFix = true;
                            }
                        }
                        else
                        {
                            fixX = false;
                            fixY = false;
                            StayPoint = Vector2.zero;
                        }
                        break;
                    case 3:
                        if ((Mathf.Abs(playerObj.transform.position.y) - Mathf.Abs(this.transform.position.y)) > 4.7f)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[3].transform.position, 0.125f);
                            if(this.transform.position == Point[3].transform.position)
                            {
                                Order = 4;
                                Leave = false;
                                DoPFix = true;
                                PosFix = false;
                            }
                        }
                        break;
                    case 4:
                        if (ProceedBlocks[1].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                        {
                            if (ProceedBlocks[2].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, Point[4].transform.position, 0.125f);
                                if (this.transform.position == Point[4].transform.position)
                                {
                                    Order = 5;
                                    Leave = false;
                                    DoPFix = true;
                                }
                            }
                        }
                        break;
                    case 5:
                        if ((Mathf.Abs(playerObj.transform.position.x) - Mathf.Abs(this.transform.position.x)) < -4f)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[5].transform.position, 0.125f);
                            if (this.transform.position == Point[5].transform.position)
                            {
                                Order = 6;
                                Leave = false;
                                DoPFix = true;
                                PosFix = false;
                            }
                        }
                        break;
                    case 6:
                        if (ProceedBlocks[3].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[6].transform.position, 0.125f);
                            if (this.transform.position == Point[6].transform.position)
                            {
                                Order = 7;
                                Leave = false;
                                DoPFix = true;
                            }
                        }
                        break;
                    case 7:
                        if ((Mathf.Abs(playerObj.transform.position.y) - Mathf.Abs(this.transform.position.y)) < -4f)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            if (this.transform.position != Point[7].transform.position)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, Point[7].transform.position, 0.125f);
                            }
                        }
                        break;

                }
                break;

            case StageName.Stage_1:
                Ordermax = 5;
                switch (Order) 
                {                     
                    case 0:
                        fixX = false;
                        fixY = true;
                        StayPoint = new Vector2(this.transform.position.x, this.transform.position.y - 3f);
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
                                StayPoint = Vector2.zero;
                                Order = 1;
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
                    case 1:
                        fixX = false;
                        fixY = false;
                        StayPoint = Vector2.zero;
                        if ((playerObj.transform.position.x - this.transform.position.x) > 8)
                        {
                            PosFix = true;
                        }
                        if (PosFix)
                        {
                            transform.position = Vector3.MoveTowards(transform.position,new Vector3 (Point[0].transform.position.x+ 16, Point[0].transform.position.y, Point[0].transform.position.z), 0.125f);
                            StayPoint = new Vector2(Point[0].transform.position.x +10, Point[0].transform.position.y);
                            if (this.transform.position == new Vector3(Point[0].transform.position.x + 16, Point[0].transform.position.y, Point[0].transform.position.z))
                            {
                                Order = 2;
                                PosFix = false;
                                Leave = false;
                                DoPFix = true;
                            }
                        }
                        break;

                    case 2:
                        fixX = true;
                        fixY = false;
                        StayPoint = new Vector2(transform.position.x - 6, transform.position.y);
                        // 移動処理
                        transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, moveSpeed * input.x);

                        if (this.transform.position == Point[1].transform.position)
                        {
                            StayPoint = Vector2.zero;
                            Order = 3;
                            Leave = false;
                            DoPFix = true;
                        }
                        if (this.transform.position == Point[0].transform.position)
                        {
                            input.x = 0;
                        }
                        break;

                    case 3:
                        fixX = false;
                        fixY = false;
                        StayPoint = Vector2.zero;
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
                                Order = 4;
                                PosFix = false;
                                Leave = false;
                                StayPoint = new Vector2(transform.position.x, transform.position.y + 3f);
                            }
                        }
                        break;

                    case 4:
                        fixX = false;
                        fixY = true;
                        StayPoint = new Vector2(transform.position.x, transform.position.y + 3f);

                        // 移動処理
                        transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, moveSpeed * -input.y);

                        if (Leave)
                        {
                            if (this.transform.position == Point[2].transform.position)
                            {
                                Order = 5;
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
                    case 5:
                        fixX = false;
                        fixY = false;
                        // 移動処理
                        if (this.transform.position != Point[3].transform.position)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, Point[3].transform.position, moveSpeed);
                        }
                        break;
                }
                break;
        }

        // プレイヤーがカメラから離れていた場合
        // カメラを中央に寄せる
        if (fixX)
        {
            if ((playerObj.transform.position.x - StayPoint.x) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + (playerObj.transform.position.x - StayPoint.x), transform.position.y, transform.position.z), moveSpeed);
            }
            if ((playerObj.transform.position.x - StayPoint.x) < -1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - (playerObj.transform.position.x - StayPoint.x), transform.position.y, transform.position.z),moveSpeed);
            }
        }

        if (fixY)
        {
            if ((playerObj.transform.position.y - StayPoint.y) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + (playerObj.transform.position.y - StayPoint.y), transform.position.z),moveSpeed);
            }
            if ((playerObj.transform.position.y - StayPoint.y) < -1)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + (playerObj.transform.position.y - StayPoint.y), transform.position.z), moveSpeed);
            }
        }
    }
}
