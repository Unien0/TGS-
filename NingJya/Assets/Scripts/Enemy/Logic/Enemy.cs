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
    private bool inPlayerAttackRange = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
    private Vector2 moveit;
    // �s���܂ł̑ҋ@����
    private float actTime;
    // �ړ������̒�~
    private bool stop = false;
    private bool fix = false;
    public Vector2 shotrote;
    private Vector2 shotIt;
    private bool shoted;//������΂��̏��

    //���v���C���[�̍U���͈͂̂ǂ��ɂ���
    private enum currentAttackRange
    {
        Null,
        Red,
        white,
        yellow,
        end
    }

   private currentAttackRange area;

    //���̐�����΂���
    private  float ForcePoint = 150;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        area = currentAttackRange.Null;
    }

    // Update is called once per frame
    void Update()
    {
        //������΂����Ȃ���ԂɈړ�����
        if (!shoted)
        {
            Move();
        }
        
        //E�L�[�����������Aplayer���U���ł���Ȃ�
        if (Input.GetKeyDown(KeyCode.E) && playerAttackable)
        {
            Debug.Log("�U��");
            shoted = true;
            BlowAway();
        }

        //������΂�������A�~�܂鎞�Ԃ��v�Z����
        if (shoted)
        {
            ToStop();
        }        
        //StopBlow();
    }

    /// <summary>
    /// �ړ��Ɋւ���
    /// </summary>
    private void Move()
    {
        if (removable)
        {
            moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
            moveit.x = Mathf.Sign(moveint.x);
            moveit.y = Mathf.Sign(moveint.y);
            rb2d.velocity = moveit * enemySpeed;
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
            //����
            shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
            shotIt.x = Mathf.Sign(shotrote.x);
            shotIt.y = Mathf.Sign(shotrote.y);

            //�[�[�[�[�[�[�[�[�[���_�iForcePoint�͂�����Q�b�g�ł��Ȃ��j�[�[�[�[�[�[�[�[�[
            //switch (area)
            //{
            //    case currentAttackRange.Red: ForcePoint = redForce; break;
            //    case currentAttackRange.white: ForcePoint = whiteForce; break;
            //    case currentAttackRange.yellow: ForcePoint = yellowForce; break;
            //}

            //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
            rb2d.AddForce(shotrote * ForcePoint);


            //�[�[�[�[�[�[�[�[�[�����̂��߂Ɂ[�[�[�[�[�[�[�[�[
            //rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);

            //// �ړ��������s��Ȃ��悤�ɂ���
            //stop = true;
            //    fix = false;

            //    //2�A�G�̈ʒu�����F�A���A�Ԃ̃G���A���ɂ��邩�ǂ����𔻒f���A�З͂�ς���B
            //    switch (area)
            //    {
            //        case currentAttackRange.Red: ForcePoint = redForce; break;
            //        case currentAttackRange.white: ForcePoint = whiteForce; break;
            //        case currentAttackRange.yellow: ForcePoint = yellowForce; break;
            //    }

            //    //3�A�v���C���[�Ƃ̈ʒu�ɂ���Đ�����΂����������߂�
            //    if (!shoted)
            //    {
            //    Debug.Log(3);
            //        // �ӂ��Ƃ΂�
            //        shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
            //        shotIt.x = Mathf.Sign(shotrote.x);
            //        shotIt.y = Mathf.Sign(shotrote.y);

            //        //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
            //        rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);
            //    shoted = true;
            //}
        }
    }

    /// <summary>
    /// ������΂���ԂɎ~�܂�̎���
    /// </summary>
    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            actTime = 0;
            shoted = false;
        }
    }

    /// <summary>
    /// ������΂��̏�ԂɎ~�܂�
    /// </summary>
    private void StopBlow()
    {
        // �ӂ��Ƃ΂���~����
        // �U�����͂�����Ă��Ȃ���
        if (!playerAttackable)
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
    }


    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    // �v���C���[�̍U���͈͂ɓ������ꍇ
    //    if (col.CompareTag("Player"))
    //    {
    //        inPlayerAttackRange = true;

    //        if (col.gameObject.name == "Red")
    //        {
    //            area = currentAttackRange.Red;
    //        }
    //        else if (col.gameObject.name == "white")
    //        {
    //            area = currentAttackRange.white;
    //        }
    //        else if (col.gameObject.name == "yellow")
    //        {
    //            area = currentAttackRange.yellow;
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
            if (col.gameObject.name == "Red")
            {
                area = currentAttackRange.Red;
            }
            else if (col.gameObject.name == "white")
            {
                area = currentAttackRange.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                area = currentAttackRange.yellow;
            }
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
