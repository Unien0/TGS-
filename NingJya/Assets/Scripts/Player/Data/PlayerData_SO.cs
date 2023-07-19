using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Player Data")]
public class PlayerData_SO : ScriptableObject
{
    [Header("��{�v���p�e�B")]
    //MaxHP
    public float maxHp;
    //HP
    public float hp;
    //�X�s�[�h
    public float speed;
    //�_���[�W
    public float damage;
    //�U��CD
    public float attackCD;

    [Header("���Y������")]
    //���Y������
    public float beatLengh;

    [Header("������΂��Ɋւ���f�[�^")]
    //������΂��̗�
    public float force;


    [Header("���[���X�e�[�^�X")]
    //���S�������ǂ���
    public bool isDead;
    //�U���ł��邩�ǂ���
    public bool attackable;
    //�ړ��\���ǂ���
    public bool removable;
    // ���G����
    public bool mutekki;
}
