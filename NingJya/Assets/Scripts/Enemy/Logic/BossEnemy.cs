using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region
    private bool blowable
    {
        //打ち飛べるかどうかを判断する
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    #endregion
    private Rigidbody2D rb2d;
    private SpriteRenderer SpR;
    private Collider2D Col2D;
    private AudioSource Audio;
    private enum BossNumber
    {
        No1,
        No2,
        end
    }
    [SerializeField] private BossNumber BossType;
    [SerializeField] int BossHP;

    #region ノックバック関係
    private bool inPlayerAttackRange = false;
    private GameObject PlayerObject;
    private Vector2 shotrote;
    private double CoolDownTime;
    private double time = -1;
    #endregion
    [SerializeField] private Vector2 shotIt;
    [SerializeField] private float ForcePoint;
    public bool ActPermission;
    private int ActCounter;
    private bool fix;
    private int RandumCount;
    private Vector2 MoveInt;
    public bool ExChange;
    [SerializeField] GameObject EnemyBullet;
    [SerializeField] private Sprite[] BossSprite;
    [SerializeField] private AudioClip isBlowSE;
    [SerializeField] private GameObject Hit_Efect;
    [SerializeField] private GameObject DEAD_EFECT;
    private float MutekiTime = 5;
    private bool AlphaExchange;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SpR = GetComponent<SpriteRenderer>();
        Col2D = GetComponent<Collider2D>();
        Audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (FindObjectOfType<BossStartFlag>().ActStart)
        {
            CoolDownTime = FindObjectOfType<GameManeger>().OneTempo * 2;
            time += Time.deltaTime;
            if (time == -1)
            {
                time = 10;
            }         
            switch (BossType)
            {
                case BossNumber.No1:
                    FirstBoss();
                    break;
            }
            Damage();
        }
    }

    void Damage()
    {
        MutekiTime += Time.deltaTime;
        if (MutekiTime > 1)
        {
            AlphaExchange = false;
            SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, 1);
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1")))
            {
                if (inPlayerAttackRange)
                {
                    // ジャストアタックのタイミングなら
                    if (blowable)
                    {
                        // クールダウンが回復したかどうか
                        if (CoolDownTime <= time)
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
                            // 現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                            rb2d.AddForce(shotIt * ForcePoint);

                            Audio.clip = isBlowSE;
                            Audio.Play();
                            Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                            BossHP = BossHP - 1;
                            time = 0;
                            MutekiTime = 0;
                            Camera.ShakeOrder = true;
                            if (BossHP <= 0)
                            {
                                FindObjectOfType<BossStartFlag>().ActEnd = true;
                                ActPermission = false;
                                SpR.enabled = false;
                                Col2D.enabled = false;
                                rb2d.velocity = Vector3.zero;
                                GameManeger.KillBOSS += 1000;
                                Instantiate(DEAD_EFECT, this.transform.position, this.transform.rotation);

                                GameManeger.shakeTime = 0.25f;
                                ShakeManeger.ShakeLevel = 4;
                            }
                            else
                            {
                                GameManeger.shakeTime = 0.125f;
                                ShakeManeger.ShakeLevel = 2;
                            }

                        }
                    }
                }
            }

        }
        else
        {
            if (!AlphaExchange)
            {
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a - Time.deltaTime * 8);
                if (SpR.color.a <= 0)
                {
                    AlphaExchange = true;
                }
            }
            else
            {
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a + Time.deltaTime * 8);
                if (SpR.color.a >= 1)
                {
                    AlphaExchange = false;
                }
            }
        }    
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
            PlayerObject = FindObjectOfType<Player>().gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = false;
        }
    }

    void FirstBoss()
    {
        // 行動許可が降りているのか
        if (ActPermission)
        {
            // アクションタイミングに入ったら
            if (ExChange)
            {
                fix = false;                
                ExChange = false;

                // アクションカウントに応じて行動を変化させる
                switch (ActCounter)
                {
                    case 1:
                        rb2d.velocity = MoveInt * 4;
                        SpR.sprite = BossSprite[0];
                        break;
                    case 2:
                        rb2d.velocity = MoveInt * 4;
                        SpR.sprite = BossSprite[1];
                        break;
                    case 3:
                        rb2d.velocity = Vector2.zero;
                        Instantiate(EnemyBullet,this.transform);
                        SpR.sprite = BossSprite[2];
                        break;
                    case 4:
                        rb2d.velocity = Vector2.zero;
                        SpR.sprite = BossSprite[1];
                        break;
                }
            }
            else
            {
                // 初期化・値の再設定
                if (!fix)
                {
                    // アクションカウントを上昇させる
                    ActCounter++;
                    if (ActCounter == 5)
                    {ActCounter = 1;}
                    fix = true;

                    RandumCount = Random.Range(1, 5);
                    Debug.Log(RandumCount);

                    switch (RandumCount)
                    {
                        case 1:
                            MoveInt = new Vector2(1, 0);
                            break;
                        case 2:
                            MoveInt = new Vector2(-1, 0);
                            break;
                        case 3:
                            MoveInt = new Vector2(0,1);
                            break;
                        case 4:
                            MoveInt = new Vector2(0, -1);
                            break;
                    }
                }
            }
        }

    }
        private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnemyBullet"))
        {
            if (col.gameObject.GetComponent<EnemyBullet>().isBlow == true)
            {
                // クールダウンが回復したかどうか
                if (CoolDownTime <= time)
                {
                    //方向
                    shotrote = new Vector2(this.transform.position.x - col.gameObject.transform.position.x, this.transform.position.y - col.gameObject.transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    // 現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * ForcePoint);

                    Audio.clip = isBlowSE;
                    Audio.Play();
                    Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                    BossHP = BossHP - 1;
                    time = 0;
                    MutekiTime = 0;
                    if (BossHP <= 0)
                    {
                        FindObjectOfType<BossStartFlag>().ActEnd = true;
                        ActPermission = false;
                        SpR.enabled = false;
                        Col2D.enabled = false;
                        rb2d.velocity = Vector3.zero;
                        GameManeger.KillBOSS += 1000;
                        Instantiate(DEAD_EFECT, this.transform.position, this.transform.rotation);
                    }

                }
            }
        }
    }
}
