using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Player Data")]
public class PlayerData_SO : ScriptableObject
{
    [Header("��������")]
    //MaxHP
    public float maxHp;
    //HP
    public float hp;
    //�Ƅ��ٶ�
    public float speed;
    //����`��
    public float damage;

    [Header("�ꥺ����v����")]
    //�ꥺ����L��
    public float beatLengh;


    [Header("����饯���`״�B")]
    //������Τ�
    public bool isDead;
    //���ĤǤ���
    public bool attackable;
    //�ƄӤǤ���
    public bool removable;
}
