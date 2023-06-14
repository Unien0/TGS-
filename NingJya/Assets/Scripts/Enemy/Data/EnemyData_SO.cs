using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Enemy Data")]
public class EnemyData_SO : ScriptableObject
{
    [Header("��{�v���p�e�B")]
    //HP
    public float Hp;
    //�X�s�[�h
    public float Speed;
    //�_���[�W
    public float damage;

    [Header("������΂��Ɋւ���f�[�^")]
    //������΂��̐�������
    public float blowTime;
    //������΂��̐�������
    public float blowDistance;
    //���ݎ���
    public float existenceTime;

    [Header("���[���X�e�[�^�X")]
    //���S�������ǂ���
    public bool isDead;
    //�U���ł��邩�ǂ���
    public bool attackable;
    //�ړ��\���ǂ���
    public bool removable;
    //������΂��ł��邩�ǂ���
    public bool blowable;
    //��΂���Ă��邩�ǂ���
    public bool beingBlow;

}
