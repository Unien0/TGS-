using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData�ϐ�
    public int maxHp
    {
        //Player��MaxHP���Q�b�g����
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    public int hp
    {
        //Player��HP���Q�b�g����
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }
    }
    private float damage
    {
        //Player�̃_���[�W�l���擾����
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //Player�̃X�s�[�h�l���擾����
        get { if (playerData != null) return playerData.speed; else return 0; }

    } 
    private float attackCD
    {
        //Player��CD���擾����
        get { if (playerData != null) return playerData.attackCD; else return 1; }

    }
    public int maxBullet
    {
        //Player��CD���擾����
        get { if (playerData != null) return playerData.maxBullet; else return 1; }
    }
    public int nowBullet
    {
        //Player��CD���擾����
        get { if (playerData != null) return playerData.nowBullet; else return 1; }
        set { playerData.nowBullet = value; }
    }
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//�v���C���[�����S�����ꍇ�ɍX�V
    }
    public bool attackable
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //�v���C���[���ړ��ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }
    }
    public bool taking
    {
        //�v���C���[���ړ��ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.taking; else return false; }
        set { playerData.taking = value; }
    }
    public bool Mutekki
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.mutekki; else return false; }
        set { playerData.mutekki = value; }
    }
    public bool TimeInspect
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }
    public bool IsAttack
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion 

    [SerializeField] private GameManeger gameManegerCS;

    private float inputX;                   // �����̈ړ����͂��m�F����ϐ�
    private float inputY;                   // �c���̈ړ����͂��m�F����ϐ�
    private Vector2 TemporaryInput;         // �ꎞ�I�Ɉړ����͂�ۑ�����ϐ�
    private float inputFixH;                // TemporaryInput.x�̒l������������܂ł̎���
    private float inputFixV;                // TemporaryInput.y�̒l������������܂ł̎���
    private Vector2 moveInput;              // 1�e���|�����̈ړ����͂�ۑ�����ϐ�
    public float moveDirection;             // �ړ������̕ۑ�
    private bool maked;                     // �ړ��G�t�F�N�g�̐������m�F����ϐ�
    // �ړ��G�t�F�N�g��ۑ�����ϐ�
    [SerializeField] private GameObject MoveEfect;
    private bool ActFix;

    private float MutekiTime;               // �_���[�W����̖��G���Ԃ��Ǘ�����ϐ�
    public bool Hit;                        // �_���[�W��H��������̊m�F�p�ϐ�

    // �U���͈̓I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private GameObject AttackArea;
    private bool InputATK;                  // �U�����͂̊m�F�p�ϐ�
    public float roteMax;                   // �U���͈͂̉�]����ۑ�����ϐ�

    private float animSpeed;                // �A�j���[�V�������x�̒l��ۑ�����ϐ�

    private Rigidbody2D rb2d;               // Rigidbody2D�R���|�[�l���g��ۑ�����ϐ�
    private Animator anim;                  // Animator�R���|�[�l���g��ۑ�����ϐ�
    private SpriteRenderer SpR;             // SpriteRenderer�R���|�[�l���g��ۑ�����ϐ�
    public Collider2D PlayerCol2D;          // Collider2D�R���|�[�l���g��ۑ�����ϐ�
    private PlayerInputActions controls;    // PlayerInputActions���g�p���邽�߂̕ϐ�
    private Vector2 moveCon;                // �R���g���[���[�̃X�e�B�b�N�����F�����邽�߂̕ϐ�
    private bool AlphaExchange;

    private AudioSource Audio;
    [SerializeField] private AudioClip DamageSE;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject[] NearEnemys;
    private Vector2 Gap;
    [SerializeField] private GameObject[] TargetEnemys;
    private float RotePass;
    private float TargetRote;

    private void Awake()
    {
        // �S�G�̏����擾
        NearEnemys = GameObject.FindGameObjectsWithTag("Enemy");

        // Rigidbody2D�R���|�[�l���g��ۑ�����
        rb2d = GetComponent<Rigidbody2D>();
        // Animator�R���|�[�l���g��ۑ�����
        anim = GetComponent<Animator>();
        // SpriteRenderer�R���|�[�l���g��ۑ�����
        SpR = GetComponentInChildren<SpriteRenderer>();
        // Collider2D�R���|�[�l���g��ۑ�����
        PlayerCol2D = GetComponent<Collider2D>();

        // �}�E�X�J�[�\�����\���ɂ���
        Cursor.visible = false;

        // �R���g���[���[�̃X�e�B�b�N����ɉ����ď������s���悤�ɏ���������
        controls = new PlayerInputActions();
        // ������͂ɉ�����moveCon�ɒl��ۑ�����悤�ɏ���������
        controls.GamePlay.Move.performed += ctx => moveCon = ctx.ReadValue<Vector2>();
        controls.GamePlay.Move.canceled += ctx => moveCon = Vector2.zero;
        // �U�����͂ɉ����čU���������s���悤�ɏ���������
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
        // ���X�e�[�W�Ȃ�
        if (SceneManager.GetActiveScene().name == "1stStage_Remake")
        {
            nowBullet = maxBullet;
            hp = maxHp;
        }
        anim.SetBool("DEAD",false);
        Audio = GetComponent<AudioSource>();
        isDead = false;
    }
    
    void Update()
    {
        // �`���[�g���A���X�e�[�W�Ȃ�
        if (SceneManager.GetActiveScene().name == "0_Tutorial")
        {
            hp = maxHp;
        }

        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }
       
        if (!isDead)
        {
            if (!taking)
            {
                move();
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }
            
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
        // �ړ����� 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0)
        {
            inputFixH += Time.deltaTime;
            if (inputFixH > gameManegerCS.OneTempo)
            {
                TemporaryInput.x = 0;
            }
        }
        else
        {
            // ���͂��L�^����
            inputFixH = 0;
            TemporaryInput.x = horizontal;
        }
        if(vertical == 0)
        {
            inputFixV += Time.deltaTime;
            if (inputFixV > gameManegerCS.OneTempo)
            {
                TemporaryInput.y = 0;
            }
        }
        else
        {   
            // ���͂��L�^����
            inputFixV = 0;
            TemporaryInput.y = vertical;
        }
        
        if (removable && !taking)
        {
            if (!maked)
            {
                if ((moveInput.x != 0) || (moveInput.y != 0))
                {
                    maked = true;
                    Instantiate(MoveEfect, this.transform.position, MoveEfect.transform.rotation);
                }
                TemporaryInput = Vector2.zero;
            }

            // �ړ�����
            rb2d.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);


            ActFix = false;
        }
        else
        {
            if (!ActFix)
            {
                ActFix = true;

                // �ēx�A�N�V�����������ł���悤�ɏ���������
                IsAttack = false;                             // �U�������̏�����
                maked = false;                                // �ړ��G�t�F�N�g�����t���O�̏�����
                rb2d.velocity = new Vector2(0, 0);            // �L�����̈ړ����~������
                attackable = false;                           // �U�����̎�����
                anim.SetBool("Attack", false);                // �U���A�j���[�V�����̒�~
                anim.SetBool("Attack", false);                // �U���A�j���[�V�����̒�~
            }
            else
            {
                #region �ړ����͂̏��� �@
                if ((TemporaryInput.x != 0) || (TemporaryInput.y != 0))
                {   // ���͂ɉ����āA�U���͈͂���]����
                    #region roteMax�ύX
                    // ���͂ɉ����āA��]��ύX����
                    if ((TemporaryInput.x >= 0.425f) && (TemporaryInput.x <= 0.8f) && (TemporaryInput.y >= 0.01f))
                    {
                        moveInput = new Vector2(1, 1);
                        moveDirection = 315;
                    }

                    if ((TemporaryInput.x == 0) && (TemporaryInput.y >= 0.01))
                    {
                        moveInput = new Vector2(0, 1);
                        moveDirection = 0;
                    }

                    if ((TemporaryInput.x <= -0.425f) && (TemporaryInput.x >= -0.8f) && (TemporaryInput.y >= 0.01f))
                    {
                        moveInput = new Vector2(-1, 1);
                        moveDirection = 45;
                    }

                    if ((TemporaryInput.x <= -0.01f) && (TemporaryInput.y == 0))
                    {
                        moveInput = new Vector2(-1, 0);
                        moveDirection = 90;
                    }

                    if ((TemporaryInput.x <= -0.425f) && (TemporaryInput.x >= -0.8f) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(-1, -1);
                        moveDirection = 135;
                    }

                    if ((TemporaryInput.x == 0) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(0, -1);
                        moveDirection = 180;
                    }

                    if ((TemporaryInput.x >= 0.425f) && (TemporaryInput.x <= 0.8f) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(1, -1);
                        moveDirection = 225;
                    }

                    if ((TemporaryInput.x >= 0.01f) && (TemporaryInput.y == 0))
                    {
                        moveInput = new Vector2(1, 0);
                        moveDirection = 270;
                    }
                    #endregion

                    // ���͂ɉ����ăA�j���[�V������ύX����
                    #region �A�j���[�V�����ύX 
                    // ��������
                    if (moveInput.x >= 0.1f)
                    {
                        SpR.flipX = true;
                        anim.SetBool("Side", true);
                    }
                    else if (moveInput.x <= -0.1f)
                    {
                        SpR.flipX = false;
                        anim.SetBool("Side", true);
                    }
                    else
                    {
                        anim.SetBool("Side", false);
                    }
                    // �c������
                    if (moveInput.y >= 0.1f)
                    {
                        anim.SetBool("Top", true);
                        anim.SetBool("Bottom", false);

                    }
                    else if (moveInput.y <= -0.1f)
                    {
                        anim.SetBool("Top", false);
                        anim.SetBool("Bottom", true);
                    }
                    else
                    {
                        anim.SetBool("Top", false);
                        anim.SetBool("Bottom", false);
                    }
                    #endregion
                }
                else
                { moveInput = Vector2.zero; }
                #endregion
            }
        }
    }

    private void Attack()
    {
        float hr_R = Input.GetAxis("Horizontal_Right");
        float vr_R = Input.GetAxis("Vertical_Right");

        if ((hr_R != 0) || (vr_R != 0))
        {   // ���͂ɉ����āA�U���͈͂���]�A�U���A�j���[�V�������Đ�����
            #region roteMax�ύX
            // �����
            if ((hr_R == 0) && (vr_R >= 0.01))
            {
                roteMax = 0;
                anim.SetBool("Right_R", false);
                anim.SetBool("Left_R", false);
                anim.SetBool("Up_R", true);
                anim.SetBool("Down_R", false);
            }
            // ������
            if ((hr_R <= -0.01f) && (vr_R == 0))
            {
                roteMax = 90;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }

            }
            // �E����
            if ((hr_R >= 0.01f) && (vr_R == 0))
            {
                roteMax = 270;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
            }
            // ������
            if ((hr_R == 0) && (vr_R <= -0.01f))
            {
                roteMax = 180;
                anim.SetBool("Right_R", false);
                anim.SetBool("Left_R", false);
                anim.SetBool("Up_R", false);
                anim.SetBool("Down_R",true);
            }
            // �E�����
            if ((hr_R >= 0.01f) && (vr_R >= 0.01f))
            {
                roteMax = 315;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }

            }
            // �������
            if ((hr_R <= -0.01f) && (vr_R >= 0.01f))
            {
                roteMax = 45;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
            }
            // ��������
            if ((hr_R <= -0.1f) && (vr_R <= -0.01f))
            {
                roteMax = 135;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
            }
            // �E��
            if ((hr_R >= 0.01f) && (vr_R <= -0.01f))
            {
                roteMax = 225;
                // �t���b�v�̏�Ԃɍ��킹��SetBool��ύX����
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
            }
            #endregion
            // �U���͈͂���]������
            AttackArea.transform.rotation = Quaternion.Euler(0, 0, roteMax);
            InputATK = true;
        }
        else
        {
            InputATK = false;
        }

        // 
        if (removable)
        {
            if ((Input.GetKeyDown(KeyCode.Q)) || (Input.GetAxis("L_R_Trigger") > 0) || (Input.GetAxis("L_R_Trigger") < 0))
            {
                if (nowBullet > 0)
                {
                    if (InputATK)
                    {
                        // ���o�^
                        // �S�G�̏�񂩂�
                        foreach (var ctobj in NearEnemys)
                        {
                            // ���͈͂ɋ���I�u�W�F�N�g���m�F����
                            Gap = new Vector2(ctobj.transform.position.x - this.transform.position.x, ctobj.transform.position.y - this.transform.position.y);
                            float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y);
                            // ���͈͓��ɂ���G��
                            if (vec < 9)
                            {
                                // �m�b�N�o�b�N or ���S��Ԃ���Ȃ��Ȃ�
                                if(ctobj.GetComponent<Collider2D>().enabled == true)
                                {
                                    // �U���Ώۂɒǉ�����
                                    Array.Resize(ref TargetEnemys, TargetEnemys.Length + 1);
                                    TargetEnemys[TargetEnemys.Length - 1] = ctobj.gameObject;
                                }

                            }
                        }

                        if (TargetEnemys.Length != 0)
                        {
                            // �U���ΏۂɃo���b�g�𔭎˂���
                            foreach (var Atobj in TargetEnemys)
                            {
                                RotePass = Mathf.Atan2((Atobj.transform.position.x - this.transform.position.x), (Atobj.transform.position.y - this.transform.position.y));
                                TargetRote = RotePass * Mathf.Rad2Deg;
                                AttackArea.transform.rotation = Quaternion.Euler(0, 0, TargetRote * -1);
                                Instantiate(Bullet, this.transform.position, AttackArea.transform.rotation);
                            }
                            nowBullet--;
                            Array.Resize(ref TargetEnemys, 0);
                        }                  
                    }
                }
            }
            else if (!attackable && removable)
            {
                if (InputATK)
                {
                    attackable = true;
                    anim.SetBool("Attack", true);
                    IsAttack = true;
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
        if (col.gameObject.CompareTag("EnemyBullet"))
        {
            Hit = true;
        }
        if (col.gameObject.name == "END")
        {
            rb2d.velocity = Vector2.zero;
            PlayerCol2D.enabled = false;
            GameOver.GAMECLEAR = true;
        }
           
    }

}
