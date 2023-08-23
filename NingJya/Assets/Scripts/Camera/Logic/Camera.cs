using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        Stage_1,
        Boss_1,
        Stage_2,
        Boss_2,
        end
    }
    [SerializeField] private StageName Process;
    [SerializeField] private int Order;
    [SerializeField] private GameObject[] Point;
    private bool Leave;

    void Start()
    {
        StartPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProcess();
        ShakeProcess();
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
        // ˆÚ“®“ü—Í 
        #region ˆÚ“®“ü—Í
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

        if (Mathf.Abs(playerObj.transform.position.x - this.transform.position.x) <= 10)
        {
            if (Mathf.Abs(playerObj.transform.position.y - this.transform.position.y) <= 10)
            {
                switch (Process)
                {
                    case StageName.Stage_1:
                        switch (Order)
                        {
                            case 0:
                                // ˆÚ“®ˆ—
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
                                        Order = 1;
                                        Leave = false;
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
                                // ˆÚ“®ˆ—
                                transform.position = Vector3.MoveTowards(transform.position, Point[1].transform.position, moveSpeed * input.x);

                                if (Leave)
                                {
                                    if (this.transform.position == Point[1].transform.position)
                                    {
                                        Order = 2;
                                        Leave = false;
                                    }
                                    if (this.transform.position == Point[0].transform.position)
                                    {
                                        input.x = 0;
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
                                // ˆÚ“®ˆ—
                                transform.position = Vector3.MoveTowards(transform.position, Point[2].transform.position, moveSpeed * -input.y);

                                if (Leave)
                                {
                                    if (this.transform.position == Point[2].transform.position)
                                    {
                                        Order = 3;
                                        Leave = false;
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
                            case 3:
                                // ˆÚ“®ˆ—
                                if (this.transform.position != Point[3].transform.position)
                                {
                                    transform.position = Vector3.MoveTowards(transform.position, Point[5].transform.position, moveSpeed);
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }

}
