using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    private float yellowForce
    {
        //���F�G���A�̗�
        get { if (playerData != null) return playerData.force; else return 0; }
    }
    private float whiteForce
    {
        //���F�G���A�̗�
        get { if (playerData != null) return playerData.force * 4; else return 0; }
    }
    private float redForce
    {
        //�ԃG���A�̗�
        get { if (playerData != null) return playerData.force * 8; else return 0; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData�̕ϐ�
    private float enemyHp
    {
        //enemy��hp���Q�b�g����
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemyDamage
    {
        //enemy�̃_���[�W���Q�b�g����
        get { if (enemyData != null) return enemyData.damage; else return 0; }

    }
    private float yellowExistenceTime
    {
        //���F�G���A�ł̕ۑ�����
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }    
    private float whiteExistenceTime
    {
        //���F�G���A�ł̕ۑ�����
        get { if (enemyData != null) return enemyData.existenceTime*4; else return 0; }

    }
    private float redExistenceTime
    {
        //�ԐF�G���A�ł̕ۑ�����
        get { if (enemyData != null) return enemyData.existenceTime*8; else return 0; }

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


    private Rigidbody2D rb2d;
    private bool ATKAREA = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
    private Vector2 moveit;
    // �s���܂ł̑ҋ@����
    private float actTime;
    // �ړ������̒�~
    private bool stop = false;
    private bool fix = false;
    [SerializeField] private float speed;
    public Vector2 shotrote;
    private Vector2 shotIt;
    private bool shoted;

    private enum ATKAREATYPE
    {
        Null,
        Red,
        white,
        yellow,
        end
    }

    ATKAREATYPE AREA = ATKAREATYPE.Null;
    float ForcePoint = 0;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        // �v���C���[�̍U���͈͂ɂ���Ƃ�
        if (ATKAREA)
        {
            // �v���C���[���U�����͂�������
            if (FindObjectOfType<Player>().ATK)
            {
                // �ړ��������s��Ȃ��悤�ɂ���
                stop = true;
                fix = false;
                if (!shoted)
                {
                    // �ӂ��Ƃ΂�
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    rb2d.AddForce(shotrote * 10, ForceMode2D.Impulse);
                }

            }
        }

        // �ӂ��Ƃ΂���~����
        // �U�����͂�����Ă��Ȃ���
        if (FindObjectOfType<Player>().ATK == false)
        {
            stop = false;
            // �C������������Ă��Ȃ��Ȃ��~����B
            if (!fix)
            {
                transform.eulerAngles = Vector3.zero;
                fix = true;
                shoted = false;
                rb2d.velocity = Vector2.zero;
                rb2d.angularVelocity = 0;
            }
        }
        Move();

        BlowAway();
    }

    /// <summary>
    /// �ړ��Ɋւ���
    /// </summary>
    private void Move()
    {
        actTime = GameManeger.Tempo;

        // ��b�o�ߎ�
        if (actTime >= 2.0f)
        {
            // ��~�������s���Ă��Ȃ��Ȃ�
            if (!stop)
            {
                // �ړ�����
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                moveit.x = Mathf.Sign(moveint.x);
                moveit.y = Mathf.Sign(moveint.y);
                rb2d.velocity = moveit * 5;

                // ��~�E����������
                if (actTime >= 2.25f)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.angularVelocity = 0;
                    actTime = 0;
                }
            }
            else actTime = 0; ;
        }
    }

    /// <summary>
    /// ������΂��Ɋւ���v���O����
    /// </summary>
    private void BlowAway()
    {
        // �v���C���[�̍U���͈͂ɂ���Ƃ�
        if (ATKAREA)
        {
            //1�A�v���C���[�͍U���������𔻒f����
            if (FindObjectOfType<Player>().ATK == true)
            {
                // �ړ��������s��Ȃ��悤�ɂ���
                stop = true;
                fix = false;

                //2�A�G�̈ʒu�����F�A���A�Ԃ̃G���A���ɂ��邩�ǂ����𔻒f���A�З͂�ς���B
                switch (AREA)
                {
                    case ATKAREATYPE.Red: ForcePoint = redForce; break;
                    case ATKAREATYPE.white: ForcePoint = whiteForce; break;
                    case ATKAREATYPE.yellow: ForcePoint = yellowForce; break;
                }

                //3�A�v���C���[�Ƃ̈ʒu�ɂ���Đ�����΂����������߂�
                if (!shoted)
                {
                    // �ӂ��Ƃ΂�
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);

                    //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ������ꍇ
        if (col.CompareTag("Player"))
        {
            ATKAREA = true;

            if (col.gameObject.name == "Red")
            {
                AREA = ATKAREATYPE.Red;
            }
            else if (col.gameObject.name == "white")
            {
                AREA = ATKAREATYPE.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                AREA = ATKAREATYPE.yellow;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
        if (col.CompareTag("Player"))
        {
            if (col.gameObject.name == "Red")
            {
                AREA = ATKAREATYPE.Red;
            }
            else if (col.gameObject.name == "white")
            {
                AREA = ATKAREATYPE.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                AREA = ATKAREATYPE.yellow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

    }
}
