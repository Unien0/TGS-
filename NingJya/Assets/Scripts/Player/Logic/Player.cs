using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    private int maxHp
    {
        //PlayerのMaxHPをゲットする
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    private int hp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }
    }
    private int damage
    {
        //Playerのダメージ値を取得する
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //Playerのスピード値を取得する
        get { if (playerData != null) return playerData.speed; else return 0; }

    } 
    private float attackCD
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.attackCD; else return 1; }

    }
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//プレイヤーが死亡した場合に更新
    }
    public bool attackable
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //プレイヤーが移動できるかどうかを判断する
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }
    }
    public bool Mutekki
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.mutekki; else return false; }
        set { playerData.mutekki = value; }
    }
    #endregion 

    private float time;
    private float roteMax;
    private float inputX;
    private float inputY;
    private float MutekiTime;


    public bool ATK;

    public Vector2 shotrote;
    [SerializeField]private Vector2 moveInput;

    private GameObject Enemyobj;    
    [SerializeField] private GameObject AttackArea;    
    public  GameObject KATANA;

    private Rigidbody2D rb2d;
    private Animator[] animators;
    private SpriteRenderer SpR;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        SpR = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.T)) || Input.GetKeyDown("joystick button 5"))
        {
            SceneManager.LoadScene(0);
        }
       
        if (!isDead)
        {
            move();
            PlayerInput();
            SwitchAnimation();
        }        
    }

    void move()
    {
        // 移動入力 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if ((horizontal != 0) || (vertical != 0))
        {
            // 入力に応じて、攻撃範囲を回転する
            #region roteMax変更
            // 入力に応じて、回転を変更する
            if ((horizontal >= 0.01f) && (vertical >= 0.01f))
            { roteMax = 315; moveInput = new Vector2(1, 1); }
            if ((horizontal == 0) && (vertical >= 0.01))
            { roteMax = 0; moveInput = new Vector2(0, 1); }
            if ((horizontal <= -0.01f) && (vertical >= 0.01f))
            { roteMax = 45; moveInput = new Vector2(-1, 1); }
            if ((horizontal <= -0.01f) && (vertical == 0))
            { roteMax = 90; moveInput = new Vector2(-1, 0); }
            if ((horizontal <= -0.1f) && (vertical <= -0.01f))
            { roteMax = 135; moveInput = new Vector2(-1, -1); }
            if ((horizontal == 0) && (vertical <= -0.01f))
            { roteMax = 180; moveInput = new Vector2(0, -1); }
            if ((horizontal >= 0.01f) && (vertical <= -0.01f))
            { roteMax = 225; moveInput = new Vector2(1, -1); }
            if ((horizontal >= 0.01f) && (vertical == 0))
            { roteMax = 270; moveInput = new Vector2(1, 0); }
            #endregion
            AttackArea.transform.rotation = Quaternion.Euler(0, 0, roteMax);

            // 入力に応じて スプライト変更をする
            // 横方向の回転
            if (moveInput.x == 1) SpR.flipX = true;
            else if (moveInput.x == -1) SpR.flipX = false;

        }
        else
        {
            moveInput = Vector2.zero;
            rb2d.velocity = new Vector2(0, 0);
        }
        if (removable)
        {
            // 移動処理
            rb2d.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
            Debug.Log("idou");
            // 攻撃入力
            if ((Input.GetKey(KeyCode.E)) || Input.GetKey("joystick button 1"))
            {
                attackable = true;
                KATANA.GetComponent<Animator>().SetBool("ATK", true);
            }

        }
        else
        {
            attackable = false;
            rb2d.velocity = Vector2.zero;
            KATANA.GetComponent<Animator>().SetBool("ATK", false);
        }
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            anim.SetFloat("InputX", inputX);
            anim.SetFloat("InputY", inputY);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (!Mutekki)
            {
                hp--;
                Mutekki = true;
                MutekiTime = 0;
            }
        }
    }
}
