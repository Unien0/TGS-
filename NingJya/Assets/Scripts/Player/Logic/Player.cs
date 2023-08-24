using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    public int maxHp
    {
        //PlayerのMaxHPをゲットする
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    public int hp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }
    }
    private float damage
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
    private int maxBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.maxBullet; else return 1; }
    }
    private int nowBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.nowBullet; else return 1; }
        set { playerData.nowBullet = value; }
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
    public bool TimeInspect
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }
    #endregion 

    private float time;
    public float roteMax;
    private float inputY;
    private float MutekiTime;
    public bool Hit;
    public bool ATK;
    private bool maked;
    private float animSpeed;

    public Vector2 shotrote;
    [SerializeField]private Vector2 moveInput;

    private GameObject Enemyobj;    
    [SerializeField] private GameObject AttackArea;    
    public  GameObject KATANA;
    [SerializeField] private GameObject MoveEfect;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer SpR;
    public Collider2D PlayerCol2D;

    private PlayerInputActions controls;
    private Vector2 moveCon;
    private bool AlphaExchange;

    private AudioSource Audio;
    [SerializeField] private AudioClip DamageSE;
    [SerializeField] private GameObject Bullet;

    private void Awake()
    {
        PlayerCol2D = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SpR = GetComponentInChildren<SpriteRenderer>();
        Cursor.visible = false;
        controls = new PlayerInputActions();
        controls.GamePlay.Move.performed += ctx => moveCon = ctx.ReadValue<Vector2>();
        controls.GamePlay.Move.canceled += ctx => moveCon = Vector2.zero;

        controls.GamePlay.Attack.started += ctx => Attack();
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    void Start()
    {
        hp = maxHp;
        nowBullet = 3;
        anim.SetBool("DEAD",false);
        Audio = GetComponent<AudioSource>();
        isDead = false;
    }
    
    void Update()
    {
        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);

        if ((Input.GetKeyDown(KeyCode.T)) || Input.GetKeyDown("joystick button 5"))
        {
            SceneManager.LoadScene(0);
        }
       
        if (!isDead)
        {
            move();
            // 攻撃入力
            Attack();
            //PlayerInput();
            SwitchAnimation();
        }

        if (Hit)
        {
            if (!Mutekki)
            {
                hp--;
                Mutekki = true;
                MutekiTime = 0;
                Audio.clip = DamageSE;
                Audio.Play();
                if (hp <= 0)
                {
                    isDead = true;
                    rb2d.velocity = Vector2.zero;
                    PlayerCol2D.enabled = false;
                    GameOver.GAMEOVER = true;
                    anim.SetBool("DEAD", true);
                }
            }
            MutekiTime += Time.deltaTime;

            if (MutekiTime >= 3)
            {
                Hit = false;
                Mutekki = false;
                AlphaExchange = false;
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b,1);
            }
            else
            {
                if (!AlphaExchange)
                {
                    SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a - Time.deltaTime * 8);
                    if(SpR.color.a <= 0)
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

    }

    private void LateUpdate()
    {
        if (!isDead)
        {
            PlayerInput();
        }
    }

    void move()
    {
        // 移動入力 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        if (removable)
        {
            if (!maked)
            {
                if ((horizontal != 0) || (vertical != 0))
                {

                    maked = true;
                    Instantiate(MoveEfect, this.transform.position, MoveEfect.transform.rotation);
                }
            }
            // 移動処理
            rb2d.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
            //rb2d.velocity = new Vector2(moveCon.x * speed, moveCon.y * speed);
        }
        else
        {
            maked = false;
            if ((horizontal != 0) || (vertical != 0))
            {   // 入力に応じて、攻撃範囲を回転する
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
                if (moveCon.x == 1) SpR.flipX = true;
                else if (moveCon.x == -1) SpR.flipX = false;
                //if (moveCon.y == 1) SpR.flipX = !SpR.flipX;
            }
            else
            {moveInput = Vector2.zero;            }
            rb2d.velocity = new Vector2(0, 0);
            attackable = false;
            KATANA.GetComponent<Animator>().SetBool("ATK", false);
        }
    }

    private void Attack()
    {
        if (removable)
        {     
            if ((Input.GetKeyDown(KeyCode.E)) || Input.GetKeyDown("joystick button 1"))
            {
                if (!attackable)
                {
                    if (removable)
                    {
                        attackable = true;
                        KATANA.GetComponent<Animator>().SetBool("ATK", true);
                    }
                }
            }
            if ((Input.GetKeyDown(KeyCode.Q)) || Input.GetKeyDown("joystick button 2"))
            {
                if(nowBullet > 0)
                {
                    Instantiate(Bullet, this.transform.position, Bullet.transform.rotation);
                    nowBullet--;
                }
            }
        }
    }

    private void PlayerInput()
    {
        inputY = Input.GetAxisRaw("Vertical");
        if (inputY <= 0f && TimeInspect)
        {
            anim.SetBool("TimeInspect", true);
            //Debug.Log("back");
        }
        else
        {
            anim.SetBool("TimeInspect", false);
        }
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("InputY", inputY);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (!Hit)
            {
                if (col.gameObject.GetComponent<Enemy>().removable)
                {
                    if (col.gameObject.GetComponent<Enemy>().enemyAct != Enemy.enemyActSet.Dummy)
                    {
                        Hit = true;
                    }
                }
            }
        }
        if (col.gameObject.name == "END")
        {
            rb2d.velocity = Vector2.zero;
            PlayerCol2D.enabled = false;
            GameOver.GAMECLEAR = true;
        }
           
    }

}
