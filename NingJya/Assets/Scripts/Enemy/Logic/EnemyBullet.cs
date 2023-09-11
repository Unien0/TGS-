using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;

public class EnemyBullet : MonoBehaviour
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
    public EnemyData_SO enemyData;
    #region
    private bool blowable
    {
        //打ち飛べるかどうかを判断する
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    #endregion

    private Rigidbody2D rb2d;               // Rigidbody2Dの取得・格納
    public float BulletSpeed;
    private GameObject objectPL;            // 目的地(Player)の情報を格納
    private float targetPL;
    float time;

    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;
    public bool conductIt;
    public bool Ready;
    public bool isBlow;
    [SerializeField] private bool exchange;
    private AudioSource Audio;
    [SerializeField] private AudioClip isBlowSE;
    private GameObject PlayerObject;
    public GameObject conductObject;
    private bool inPlayerAttackRange = false;
    [SerializeField] private GameObject Hit_Efect;

    void Start()
    {
        Audio = GetComponent<AudioSource>();
        // RigidBody2Dの情報格納・代入
        rb2d = GetComponent<Rigidbody2D>();
        // Playerオブジェクトを検索 → objectPLに情報格納
        objectPL = FindObjectOfType<Player>().gameObject;
        // objectPLからtransform.position.x,yの情報を取得、
        // このオブジェクトとの位置の差をtargetPLに代入

    }

    void Update()
    {

        time += Time.deltaTime;
        if(time >= 5)
        {
            Destroy(this.gameObject);
        }

        if (inPlayerAttackRange)
        {
            if (IsAttack)
            {
                Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                rb2d.velocity = Vector3.zero;
                isBlow = true;
                conductIt = true;
                FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                FindObjectOfType<ConductManeger>().conduct = true;
                Audio.clip = isBlowSE;
                Audio.Play();
                GameManeger.SKillScore = + 200;
                IsAttack = false;
            }
        }

        if (isBlow)
        {
            BlowAway();
        }
        else
        {
            rb2d.velocity = transform.up * BulletSpeed;
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
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    //現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * BulletSpeed * 15);                
                }
                else
                {
                    //方向
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y).normalized;
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * BulletSpeed * 15);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "！Player")
        {
            Destroy(this.gameObject);
            FindObjectOfType<Player>().Hit = true;
        }
        if ((collision.gameObject.name == "Tilemap_outside_wall") ||(collision.gameObject.name == "Tilemap_wall"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
            PlayerObject = FindObjectOfType<Player>().gameObject;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isBlow)
            {
                Destroy(this.gameObject);                
            }
        }
        if (collision.gameObject.CompareTag("HitObj"))
        {
            if (collision.gameObject.GetComponent<Objects>().ObjNAME == ObjectType.Table)
            {
                Destroy(this.gameObject);
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
}
