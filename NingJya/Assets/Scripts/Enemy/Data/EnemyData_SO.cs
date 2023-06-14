using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Enemy Data")]
public class EnemyData_SO : ScriptableObject
{
    [Header("基本プロパティ")]
    //HP
    public float Hp;
    //スピード
    public float Speed;
    //ダメージ
    public float damage;

    [Header("吹っ飛ばすに関するデータ")]
    //吹っ飛ばすの制限時間
    public float blowTime;
    //吹っ飛ばすの制限距離
    public float blowDistance;
    //存在時間
    public float existenceTime;

    [Header("ロールステータス")]
    //死亡したかどうか
    public bool isDead;
    //攻撃できるかどうか
    public bool attackable;
    //移動可能かどうか
    public bool removable;
    //吹っ飛ばすできるかどうか
    public bool blowable;
    //飛ばされているかどうか
    public bool beingBlow;

}
