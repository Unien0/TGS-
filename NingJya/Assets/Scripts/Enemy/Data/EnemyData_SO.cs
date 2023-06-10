using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Enemy Data")]
public class EnemyData_SO : ScriptableObject
{
    [Header("��������")]
    //HP
    public float Hp;
    //�Ƅ��ٶ�
    public float Speed;
    //����`��
    public float damage;

    [Header("�����؄e����")]
    //�֤��w�Ф��Εr�g
    public float blowTime;
    //�֤��w�Ф��ξ��x
    public float blowDistance;
    //������δ��ڕr�g
    public float existenceTime;

    [Header("����饯���`״�B")]
    //������Τ�
    public bool isDead;
    //���ĤǤ���
    public bool attackable;
    //�ƄӤǤ���
    public bool removable;
    //�֤��w�Ф��Ǥ���
    public bool blowable;
    //�֤��w�Ф���
    public bool beingBlow;

}
