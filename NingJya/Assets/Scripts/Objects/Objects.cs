using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region
    public bool IsAttack
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion
    public EnemyData_SO SeatClothData;
    #region SeatClothDataの変数
    private float blowTime
    {
        //吹っ飛ばすの状態に直すの時間
        get { if (SeatClothData != null) return SeatClothData.blowTime; else return 0; }
        set { SeatClothData.blowTime = value; }
    }
    private bool removable
    {
        //移動可能かどうかを判断する
        get { if (SeatClothData != null) return SeatClothData.removable; else return false; }
    }
    private bool blowable
    {
        //打ち飛べるかどうかを判断する
        get { if (SeatClothData != null) return SeatClothData.blowable; else return false; }
        set { SeatClothData.blowable = value; }
    }
    private bool beingBlow
    {
        //飛ばされているかどうかを判断する
        get { if (SeatClothData != null) return SeatClothData.beingBlow; else return false; }
    }
    #endregion

    private float ForcePoint;
    private float actTime;

    private bool inPlayerAttackRange = false;
    public bool shoted;//吹っ飛ばすの状態

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private Animator animator;

    private GameObject PlayerObject;

    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;

    [SerializeField] private bool exchange;
    private bool isEnd;
    public bool conductIt;
    public GameObject conductObject;
    public bool Ready;
    private bool isBlow;


    // トラップ用変数
    private float ResetTime;
    public bool Activate;
    private bool Motion;

    public enum ObjectType
    {
        Cushion,
        Table,
        BambooTrap,
        end
    }
    public ObjectType ObjNAME;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        switch (ObjNAME)
        {
            case ObjectType.Cushion:
                ForcePoint = 800; break;
            case ObjectType.Table:
                ForcePoint = 400; break;
        }
    }


    void Update()
    {
        // 共通処理
        // ジャストアタックのタイミングの上
        if (blowable)
        {
            if (inPlayerAttackRange)
            {
                if (IsAttack)
                {
                    Debug.Log("Get E");
                    shoted = true;
                    BlowAway();
                    actTime = 0;
                    if (ObjNAME == ObjectType.Cushion)
                    { col2d.isTrigger = false; }
                    else if (ObjNAME == ObjectType.BambooTrap)
                    { Destroy(this.gameObject); }
                    conductIt = true;
                    FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                    FindObjectOfType<ConductManeger>().conduct = true;
                    FindObjectOfType<Player>().KATANA.GetComponent<Animator>().SetBool("ATK", true);
                    IsAttack = false;
                }
            }
        }
        else
        {
            ToStop();
        }

        if (Activate)
        {
            switch (ObjNAME)
            {
                case ObjectType.BambooTrap:
                    if (!Motion)
                    {
                        animator.SetBool("Boot", true);
                        PlayerObject.GetComponent<Player>().Hit = true;
                        Motion = true;
                    }
                    else
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("wait"))
                        {
                            Activate = false;
                            Motion = false;
                            animator.SetBool("Boot", false);
                        }
                    }
                    break;
            }
        }
    }

    private void BlowAway()
    {

        // プレイヤーの攻撃範囲にいるとき
        if (inPlayerAttackRange)
        {
            if (Ready)
            {
                if (conductObject != null)
                {
                    //方向
                    shotrote = new Vector2(conductObject.transform.position.x - this.transform.position.x, conductObject.transform.position.y - this.transform.position.y);

                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    //現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * ForcePoint);
                    Debug.Log("[" + this.gameObject.name + "] Go To [" + conductObject.gameObject.name + "]");
                }
                else
                {
                    //方向
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * ForcePoint);
                }
            }
        }
    }

    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            shoted = false;
            if (ObjNAME == ObjectType.Cushion)
            {
                col2d.isTrigger = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (ObjNAME == ObjectType.Table)
        {
            // プレイヤーの攻撃範囲に入っている
            if (col.gameObject.CompareTag("Player"))
            {
                inPlayerAttackRange = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (ObjNAME == ObjectType.Table)
        {
            // プレイヤーの攻撃範囲から出る
            if (col.gameObject.CompareTag("Player"))
            {
                inPlayerAttackRange = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("wall")) || (col.CompareTag("HitObj")))
        {
            if (ObjNAME == ObjectType.Cushion)
            {
                col2d.isTrigger = true;
            }
        }
        if (col.CompareTag("Enemy"))
        {
            col2d.isTrigger = false;
        }
        if (col.gameObject.name == "！Player")
        {

            if (ObjNAME == ObjectType.BambooTrap)
            {
                Activate = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = false;
        }
        if (col.CompareTag("Enemy"))
        {
            col2d.isTrigger = true;
        }
    }
}
