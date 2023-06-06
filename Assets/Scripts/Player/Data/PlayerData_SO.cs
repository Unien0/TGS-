using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Player Data")]
public class PlayerData_SO : ScriptableObject
{
    [Header("基本属性")]
    //MaxHP
    public float maxHp;
    //HP
    public float hp;
    //移動速度
    public float speed;
    //ダメージ
    public float damage;

    [Header("リズムに関する")]
    //リズムの長さ
    public float beatLengh;


    [Header("キャラクター状態")]
    //死んだのか
    public bool isDead;
    //攻撃できる
    public bool attackable;
    //移動できる
    public bool removable;
}
