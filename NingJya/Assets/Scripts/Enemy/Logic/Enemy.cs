using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //���L�f�[�^
    public PlayerData_SO playerData;
    #region PlayerData�̕ϐ�
    private float playerHp
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
    private bool removable
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

    private enum enemyActSet
    {
        move,
        shot,
        cannonball,
        end
    }
    //���v���C���[�̍U���͈͂̂ǂ��ɂ���
    private enum currentAttackRange
    {
        Null,
        Red,
        white,
        yellow,
        end
    }
    
    private float actTime;// �s���܂ł̑ҋ@����    
    [SerializeField] private float objctDistance;
    private float hp;

    //�̂̏�ԁiBool�j
    private bool inPlayerAttackRange = false;
    private bool isEnemyClash = false;
    private bool shotOk;
    private bool isShot;
    private bool shoted;//������΂��̏��
    private bool hit;
    private bool stop = false;// �ړ������̒�~
    private bool fix = false;
    [SerializeField] private bool exchange;

    private Rigidbody2D rb2d;
    enemyActSet enemyAct;
    private currentAttackRange area;

    private GameObject ClashEnemyObj;
    private GameObject PlayerObject;
    [SerializeField] private GameObject cannonball;
    [SerializeField] private GameObject sotBullet;

    private Vector2 clashRote;
    private Vector2 clashshotIt;
    private Vector2 moveint;    
    private Vector2 moveit;// moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;

    }

    void Start()
    {
        area = currentAttackRange.Null;
        hp = enemyHp;
    }

    // Update is called once per frame
    void Update()
    {
        //������΂����Ȃ���ԂɈړ�����
        if (!shoted)
        {
            Move();
        }
        if (hit)
        {
            shoted = true;
            hit = false;
           HitBlow();
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 4"))
        {
            exchange = !exchange;
        }

        //E�L�[�����������Aplayer���U���ł����
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1") )
        {
            if (inPlayerAttackRange)
            {
                // �W���X�g�A�^�b�N�̃^�C�~���O�Ȃ�
                if (blowable)
                {
                    shoted = true;
                    BlowAway();
                }
                else
                {
                    shoted = false;
                }
            }
        }

        //������΂�������A�~�܂鎞�Ԃ��v�Z����
        if (shoted)
        {
            ToStop();
        }        
        //StopBlow();

        switch (enemyAct)
        {
            case enemyActSet.move:
                break;
            case enemyActSet.shot:
                shotOk = true;
                break;
            case enemyActSet.cannonball:
                shotOk = true;
                rb2d.velocity = Vector3.zero;
                break;
        }

        if (shotOk)
        {
            if (enemyAct == enemyActSet.cannonball)
            {
                if (GameManeger.Tempo == 0)
                {
                    if (!isShot)
                    {
                        isShot = true;

                    }
                }
                else
                {
                    isShot = false;
                }
            }
            if (enemyAct == enemyActSet.shot)
            {
                if(GameManeger.Tempo == 1)
                {
                    if (!isShot)
                    {
                        isShot = true;
                        
                    }
                }
                else
                {
                    isShot = false;
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
            if (exchange)
            {
                // �S�G�̎擾
                // �U���͈͓��̓G�̎擾
                // �U���͈͓��ŁA�U�������ɂ���G���擾
                // ��̓G�擾�̒������ԋ߂��̓G���擾
                // ���̃I�u�W�F�N�g�ƖڕW�I�u�W�F�N�g�̍������߁A�A�h�t�H�[�X
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

    void HitBlow()
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

    /// <summary>
    /// ������΂���ԂɎ~�܂�̎���
    /// </summary>
    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (actTime >= blowTime + 0.5f)
            {
                if (hp <= 0)
                {
                    Destroy(this.gameObject);
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
                hp--;
            }
        }
        if (col.gameObject.CompareTag("HitObj"))
        {
            hit = true;
            hp--;
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
