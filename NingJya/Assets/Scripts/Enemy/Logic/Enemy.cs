using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //���L�f�[�^
    public PlayerData_SO playerData;
    #region PlayerData�̕ϐ�
    private int playerHp
    {
        //Player��HP���Q�b�g����
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//�_���[�W��^�����ꍇ��Player�̌��ʂ��C������
    }
    private float playerDamage
    {
        //Player�̃_���[�W�l���擾����
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float ForcePoint
    {
        //���̗�
        get { if (playerData != null) return playerData.force; else return 0; }
    }

    public bool playerAttackable
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.attackable; else return false; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData�̕ϐ�
    private float enemyHp
    {
        //enemy��hp���Q�b�g����
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemySpeed
    {
        //enemy��hp���Q�b�g����
        get { if (enemyData != null) return enemyData.Speed; else return 0; }
    }
    private float enemyDamage
    {
        //enemy�̃_���[�W���Q�b�g����
        get { if (enemyData != null) return enemyData.damage; else return 0; }

    }
    private float blowTime
    {
        //������΂��̏�Ԃɒ����̎���
        get { if (enemyData != null) return enemyData.blowTime; else return 0; }

    }
    private float ExistenceTime
    {
        //���̂̕ۑ�����
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }    
    private bool attackable
    {
        //�U���̉ۂ𔻒f����
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    public bool removable
    {
        //�ړ��\���ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.removable; else return false; }
    }
    private bool blowable
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    private bool beingBlow
    {
        //��΂���Ă��邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion

    public enum enemyActSet
    {
        move,
        notMove,
        cannonball,
        knockback,
        clockup,
        end
    }
    
    public float actTime;// �s���܂ł̑ҋ@����    
    [SerializeField] private float objctDistance;
    [SerializeField]private float hp;
    public float knockbackPoint;

    //�̂̏�ԁiBool�j
    private bool inPlayerAttackRange = false;
    private bool isEnemyClash = false;
    private bool shotOk;
    private bool isShot;
    private bool shoted;//������΂��̏��
    private bool hit;
    private double CoolDownTime;
    private double time = -1;
    private bool stop = false;// �ړ������̒�~
    private bool fix = false;
    [SerializeField] private bool exchange;
    private bool isEnd;
    public bool conductIt;
    public bool Ready;
    private bool isBlow;

    private Rigidbody2D rb2d;
    private SpriteRenderer SpR;
    private Collider2D col2d;
    private Animator anim;
    [SerializeField] private enemyActSet enemyAct;

    private GameObject ClashEnemyObj;
    private GameObject PlayerObject;
    public GameObject conductObject;
    [SerializeField] private GameObject HitEfect;
    [SerializeField] private GameObject DEADEfect;
    [SerializeField] private GameObject EnemyBullet;

    private Vector2 clashRote;
    private Vector2 clashshotIt;
    private Vector2 moveint;    
    private Vector2 moveit;// moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;


    private AudioSource Audio;
    [SerializeField] private AudioClip isBlowSE;
    [SerializeField] private AudioClip HITSE;
    private bool IsSound;
    [SerializeField] private AudioClip DeaDSE1;
    [SerializeField] private AudioClip DeaDSE2;

    private void Awake()
    {
        hp = enemyHp;
        rb2d = GetComponent<Rigidbody2D>();
        SpR = GetComponent<SpriteRenderer>();
        col2d = GetComponent<Collider2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
        Audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        knockbackPoint = ForcePoint;
    }

    void Start()
    {
        hp = enemyHp;
        if (enemyAct == enemyActSet.knockback)
        { knockbackPoint = knockbackPoint * 60; }
        CoolDownTime = FindObjectOfType<GameManeger>().OneTempo * 2;
        if (time == -1)
        {
            time = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd)
        {
            time += Time.deltaTime;

            if (isBlow)
            {                
                BlowAway();
                isBlow = false;
            }

            //������΂����Ȃ���ԂɈړ�����
            if (!shoted)
            {
                switch (enemyAct)
                {
                    case enemyActSet.move:
                        Move();
                        break;
                    case enemyActSet.notMove:
                        rb2d.velocity = Vector3.zero;
                        break;
                    case enemyActSet.cannonball:
                        shotOk = true;
                        rb2d.velocity = Vector3.zero;
                        break;
                    case enemyActSet.knockback:
                        Move();                        
                        break;

                }
            }

            if (hit)
            {                
                Instantiate(HitEfect, this.transform.position, this.transform.rotation);
                Audio.clip = HITSE;
                Audio.Play();
                shoted = true;
                hit = false;
                HitBlow();
            }

            //E�L�[�����������Aplayer���U���ł����
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1"))
            {
                if (inPlayerAttackRange)
                {
                    // �N�[���_�E�����񕜂������ǂ���
                    if (CoolDownTime <= time)
                    {
                        // �W���X�g�A�^�b�N�̃^�C�~���O�Ȃ�
                        if (blowable)
                        {
                            Instantiate(HitEfect, this.transform.position, this.transform.rotation);
                            Audio.clip = HITSE;
                            Audio.Play();
                            shoted = true;
                            hp = -1;
                            isBlow = true;
                            conductIt = true;
                            FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                            FindObjectOfType<ConductManeger>().conduct = true;
                            FindObjectOfType<Player>().KATANA.GetComponent<Animator>().SetBool("ATK", true);
                            time = 0;
                        }
                        else
                        {
                            shoted = false;
                        }
                    }                    
                }
            }

            //������΂�������A�~�܂鎞�Ԃ��v�Z����
            if (shoted)
            {
                ToStop();
            }

            if (shotOk)
            {
                if (enemyAct == enemyActSet.cannonball)
                {
                    #region
                    if (GameManeger.Tempo == 0)
                    {
                        if (!isShot)
                        {
                            PlayerObject = FindObjectOfType<Player>().gameObject;
                            moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                            objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
                            if (objctDistance <= 10)
                            {
                                Instantiate(EnemyBullet, this.transform.position, this.transform.rotation);
                            }
                            isShot = true;                            
                        }
                    }
                    else
                    {
                        isShot = false;
                    }
                    #endregion
                }
            }
        }
    }

    /// <summary>
    /// �ړ��Ɋւ���
    /// </summary>
    private void Move()
    {
        if (removable)
        {
            PlayerObject = FindObjectOfType<Player>().gameObject;
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
                if (objctDistance <= 10)
                {
                    moveit.x = Mathf.Sign(moveint.x);
                    moveit.y = Mathf.Sign(moveint.y);
                    rb2d.velocity = moveit * enemySpeed;
                }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
        }
    }

    /// <summary>
    /// ������΂��Ɋւ���v���O����
    /// </summary>
    private void BlowAway()
    {
        // �v���C���[�̍U���͈͂ɂ���Ƃ�
        if (inPlayerAttackRange)
        {
            if (Ready)
            {
                GameManeger.ComboCheck = true;
                if (conductObject != null)
                {
                    //����
                    shotrote = new Vector2(conductObject.transform.position.x - this.transform.position.x, conductObject.transform.position.y - this.transform.position.y);

                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * ForcePoint);
                    Debug.Log("[" + this.gameObject.name + "] Go To [" + conductObject.gameObject.name + "]");
                }
                else
                {
                    //����
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * ForcePoint);
                }
            }
        }
    }

    void HitBlow()
    {
        if (Ready)
        {            
            if (conductObject != null)
            {
                //����
                shotrote = new Vector2(this.transform.position.x - conductObject.transform.position.x, this.transform.position.y - conductObject.transform.position.y);

                if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(shotrote.x); }
                else
                { shotIt.x = 0; }
                if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(shotrote.y); }
                //���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                rb2d.AddForce(shotIt * ForcePoint);
            }
            else
            {
                //����
                shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(shotrote.x); }
                else
                { shotIt.x = 0; }
                if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(shotrote.y); }
                else
                { shotIt.y = 0; }
                //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                rb2d.AddForce(shotIt * ForcePoint);
            }
        }
    }

    /// <summary>
    /// ������΂���ԂɎ~�܂�̎���
    /// </summary>
    private void ToStop()
    {
        anim.SetBool("DEAD", true);
        shotOk = false;
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (GameManeger.TempoExChange)
            {
                if (hp <= 0)
                {
                    if (!IsSound)
                    {
                        IsSound = true;
                        Audio.clip = DeaDSE2;
                        Audio.Play();
                        // �G�t�F�N�g�쐬
                        Instantiate(DEADEfect, this.transform.position, this.transform.rotation);
                    }

                    SpR.enabled = false;
                    col2d.enabled = false;                   

                    if (actTime > blowTime + 1)
                    {
                        isEnd = true;
                        GameManeger.KillEnemy ++;
                    }
                }
                else
                {
                    actTime = 0;
                    shoted = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.GetComponent<Enemy>().shoted)
            {
                hit = true;
                GameManeger.ComboCheck = true;
                hp = 0;
                GameManeger.hitEnemy++;
            }
        }
        if ((col.gameObject.CompareTag("HitObj")) )
        {
            hit = true;
            GameManeger.hitEnemy++;
            hp = 0;
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
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
