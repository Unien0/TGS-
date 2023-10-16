using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region
    public bool IsAttack
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion
    public EnemyData_SO SeatClothData;
    #region SeatClothData�̕ϐ�
    private float blowTime
    {
        //������΂��̏�Ԃɒ����̎���
        get { if (SeatClothData != null) return SeatClothData.blowTime; else return 0; }
        set { SeatClothData.blowTime = value; }
    }
    private bool removable
    {
        //�ړ��\���ǂ����𔻒f����
        get { if (SeatClothData != null) return SeatClothData.removable; else return false; }
    }
    private bool blowable
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (SeatClothData != null) return SeatClothData.blowable; else return false; }
        set { SeatClothData.blowable = value; }
    }
    private bool beingBlow
    {
        //��΂���Ă��邩�ǂ����𔻒f����
        get { if (SeatClothData != null) return SeatClothData.beingBlow; else return false; }
    }
    #endregion

    private float ForcePoint;
    private float actTime;

    private bool inPlayerAttackRange = false;
    public bool shoted;//������΂��̏��

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private Animator animator;

    private GameObject PlayerObject;

    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;

    [SerializeField] private bool exchange;
    private bool isEnd;
    public bool conductIt;
    public GameObject conductObject;
    public bool Ready;
    private bool isBlow;


    // �g���b�v�p�ϐ�
    private float ResetTime;
    public bool Activate;
    private bool Motion;

    // �M�~�b�N�Ή��I�u�W�F�N�g(�P��)
    [SerializeField] private GameObject GimmickObj;
    // �M�~�b�N�Ή��I�u�W�F�N�g(����)
    [SerializeField] private GameObject[] GimmickObjSeries;
    // �M�~�b�N���͊m�F�ϐ�(A,B,X,Y�{�^���������ƕς��)
    [SerializeField]private int GimmickInput;
    // �M�~�b�N�����i�K���m�F����ϐ�
    [SerializeField]private int GimmickOrder;
    private int roteMax;

    public enum ObjectType
    {
        Cushion,
        Table,
        BambooTrap,
        RotatingFloor,
        MovableBridge,
        end
    }
    public ObjectType ObjNAME;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        switch (ObjNAME)
        {
            case ObjectType.Cushion:
                ForcePoint = 800; break;
            case ObjectType.Table:
                ForcePoint = 100; break;
        }
    }


    void Update()
    {
        // ���ʏ���
        // �W���X�g�A�^�b�N�̃^�C�~���O�̏�
        if (blowable)
        {
            if (inPlayerAttackRange)
            {
                if (IsAttack)
                {
                    shoted = true;
                    BlowAway();
                    actTime = 0;
                    if (ObjNAME == ObjectType.Cushion)
                    { col2d.isTrigger = false; }
                    else if (ObjNAME == ObjectType.BambooTrap)
                    { Destroy(this.gameObject); }
                    conductIt = true;
                    FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                    FindObjectOfType<ConductManeger>().conduct = true;
                    IsAttack = false;
                }
            }
        }
        else
        {
            if (ObjNAME != ObjectType.RotatingFloor)
            {
                ToStop();
            }
        }

        if (Activate)
        {
            switch (ObjNAME)
            {
                case ObjectType.BambooTrap:
                    if (!Motion)
                    {
                        animator.SetBool("Boot", true);
                        PlayerObject.GetComponent<Player>().Hit = true;
                        Motion = true;
                    }
                    else
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("wait"))
                        {
                            Activate = false;
                            Motion = false;
                            animator.SetBool("Boot", false);
                        }
                    }
                    break;
                case ObjectType.MovableBridge:
                    // ���v���Ƀ{�^������������
                    if (Input.GetKeyDown("joystick button 0"))
                    { // A�̏ꍇ
                        Debug.Log("A");

                        // B �� A 
                        if (GimmickInput == 1)
                        {
                            GimmickInput = 0;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 1"))
                    { // B�̏ꍇ
                        Debug.Log("B");
                        // Y �� B
                        if (GimmickInput == 3)
                        {
                            GimmickInput = 1;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 2"))
                    { // X�̏ꍇ
                        Debug.Log("X");
                        // A �� X
                        if (GimmickInput == 0)
                        {
                            GimmickInput = 2;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 3"))
                    { // Y�̏ꍇ
                        Debug.Log("Y");

                        // X �� Y 
                        if (GimmickInput == 2)
                        {
                            GimmickOrder++;
                            GimmickInput = 3;
                        }
                    }

                    // ��]�������񐔂ɉ����ċ����o��������
                    GimmickObjSeries[GimmickOrder-1].gameObject.SetActive(true);

                    break;
                case ObjectType.RotatingFloor:
                    if (Input.GetKeyDown("joystick button 0"))
                    { // A�̏ꍇ
                        Debug.Log("A");

                        // B �� A 
                        if (GimmickInput == 1)
                        {
                            roteMax -= 15;
                            GimmickInput = 0;
                        }
                        // X �� A 
                        if (GimmickInput == 2)
                        {
                            roteMax += 15;
                            GimmickInput = 0;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 1"))
                    { // B�̏ꍇ
                        Debug.Log("B");
                        // Y �� B
                        if (GimmickInput == 3)
                        {
                            roteMax -= 15;
                            GimmickInput = 1;
                        }
                        // A �� B
                        if (GimmickInput == 0)
                        {
                            roteMax += 15;
                            GimmickInput = 1;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 2"))
                    { // X�̏ꍇ
                        Debug.Log("X");
                        // A �� X
                        if (GimmickInput == 0)
                        {
                            roteMax -= 15;
                            GimmickInput = 2;
                        }
                        // Y �� X
                        if (GimmickInput == 3)
                        {
                            roteMax += 15;
                            GimmickInput = 2;
                        }
                    }
                    if (Input.GetKeyDown("joystick button 3"))
                    { // Y�̏ꍇ
                        Debug.Log("Y");

                        // X �� Y 
                        if (GimmickInput == 2)
                        {
                            roteMax -= 15;
                            GimmickInput = 3;
                        }
                        // B �� Y 
                        if (GimmickInput == 1)
                        {
                            roteMax += 15;
                            GimmickInput = 3;
                        }
                    }
                    GimmickObj.transform.rotation = Quaternion.Euler(0, 0, roteMax);
                    break;
            }
        }
    }

    private void BlowAway()
    {

        // �v���C���[�̍U���͈͂ɂ���Ƃ�
        if (inPlayerAttackRange)
        {
            if (Ready)
            {
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

    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            shoted = false;
            if (ObjNAME == ObjectType.Cushion)
            {
                col2d.isTrigger = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (ObjNAME == ObjectType.Table)
        {
            // �v���C���[�̍U���͈͂ɓ����Ă���
            if (col.gameObject.CompareTag("Player"))
            {
                inPlayerAttackRange = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (ObjNAME == ObjectType.Table)
        {
            // �v���C���[�̍U���͈͂���o��
            if (col.gameObject.CompareTag("Player"))
            {
                inPlayerAttackRange = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("wall")) || (col.CompareTag("HitObj")))
        {
            if (ObjNAME == ObjectType.Cushion)
            {
                col2d.isTrigger = true;
            }
        }
        if (col.CompareTag("Enemy"))
        {
            if (ObjNAME != ObjectType.RotatingFloor)
            {
                col2d.isTrigger = false;
            }
        }
        if (col.gameObject.name == "�IPlayer")
        {
            Activate = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = false;
        }
        if (col.CompareTag("Enemy"))
        {
            col2d.isTrigger = true;
        }
        if (col.gameObject.name == "�IPlayer")
        {
            if (ObjNAME == ObjectType.RotatingFloor)
            {
                Activate = false;
            }
        }
    }
}
