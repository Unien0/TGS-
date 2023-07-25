using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
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

    private float ForcePoint = 700;
    private float actTime;

    private bool inPlayerAttackRange = false;
    private bool shoted;//吹っ飛ばすの状態

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private GameObject PlayerObject;

    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;

    [SerializeField] private bool exchange;
    private bool isEnd;
    public bool conductIt;
    public GameObject conductObject;
    public bool Ready;
    private bool isBlow;

    private enum ObjectType
    {
        Cushion,
        end
    }
    [SerializeField] private ObjectType ObjNAME;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 4"))
        {
            exchange = !exchange;
        }


        // ジャストアタックのタイミングの上
        if (blowable)
        {
            //Eキーを押した時、playerも攻撃できるなら
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1"))
            {
                if (inPlayerAttackRange)
                {
                    shoted = true;
                    BlowAway();
                    actTime = 0;
                    col2d.isTrigger = false;

                    conductIt = true;
                    FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                    FindObjectOfType<ConductManeger>().conduct = true;
                    FindObjectOfType<Player>().KATANA.GetComponent<Animator>().SetBool("ATK", true);
                }
            }
        }
        else
        {
            shoted = false;
        }

        if (!shoted)
        {
            ToStop();
        }
    }

    private void BlowAway()
    {

        // プレイヤーの攻撃範囲にいるとき
        if (inPlayerAttackRange)
        {
            // オブジェクトタイプを確認
            switch (ObjNAME)
            {
                case ObjectType.Cushion:
                    #region
                    if (exchange)
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
                    #endregion
                    break;
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
            col2d.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("wall")) || (col.CompareTag("HitObj")))
        {
            col2d.isTrigger = true;
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
    }
}
