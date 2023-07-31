using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
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
    private Vector2 targetPL;
    float time;

    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;
    public bool conductIt;
    public bool Ready;
    private bool isBlow;
    [SerializeField] private bool exchange;
    private AudioSource Audio;
    [SerializeField] private AudioClip isBlowSE;
    private GameObject PlayerObject;
    public GameObject conductObject;
    private bool inPlayerAttackRange = false;

    void Start()
    {
        Audio = GetComponent<AudioSource>();
        // RigidBody2Dの情報格納・代入
        rb2d = GetComponent<Rigidbody2D>();
        // Playerオブジェクトを検索 → objectPLに情報格納
        objectPL = FindObjectOfType<Player>().gameObject;
        // objectPLからtransform.position.x,yの情報を取得、
        // このオブジェクトとの位置の差をtargetPLに代入
        targetPL = new Vector2((objectPL.transform.position.x - this.transform.position.x), (objectPL.transform.position.y - this.transform.position.y));
        // targetPLの位置に向かって移動する
        rb2d.AddForce(targetPL * BulletSpeed);
    }

    void Update()
    {       
        time += Time.deltaTime;
        if(time >= 5)
        {
            Destroy(this.gameObject);
        }

        //Eキーを押した時、playerも攻撃できる上
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1"))
        {
            if (inPlayerAttackRange)
            {
                rb2d.velocity = Vector3.zero;
                isBlow = true;
                conductIt = true;
                FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                FindObjectOfType<ConductManeger>().conduct = true;
                FindObjectOfType<Player>().KATANA.GetComponent<Animator>().SetBool("ATK", true);
                Audio.clip = isBlowSE;
                Audio.Play();
            }
        }

        if (isBlow)
        {
            BlowAway();
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
                    else
                    { shotIt.y = 0; }
                    //現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * BulletSpeed);
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
                    rb2d.AddForce(shotIt * BulletSpeed);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "！Player")
        {
            Debug.Log("Trg");
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
