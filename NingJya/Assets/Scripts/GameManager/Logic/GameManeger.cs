using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region EnemyDataの変数
    private bool enemyRemovable
    {
        //移動可能かどうかを判断する
        get { if (enemyData != null) return enemyData.removable; else return false; }
        set { enemyData.removable = value; }
    }
    private bool blowable
    {
        //打ち飛べるかどうかを判断する
        get { if (enemyData != null) return enemyData.blowable; else return false; }
        set { enemyData.blowable = value; }
    }
    private bool isDead
    {
        //打ち飛べるかどうかを判断する
        get { if (enemyData != null) return enemyData.isDead; else return false; }
        set { enemyData.isDead = value; }
    }
    #endregion
    public PlayerData_SO playerData;
    #region playerDataの変数
    public bool attackable
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //プレイヤーが移動できるかどうかを判断する
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }
    }
    #endregion


    [SerializeField] private float BPM;
    public static double Tempo;
    private double time;
    public double OneTempo;
    public static double one_eighthTenpo;
    public static double one_eighthCount;
    private AudioSource Audio;
    [SerializeField] private AudioClip MetronomeSE;
    bool isSound;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        enemyRemovable = false;
        OneTempo = 60 / BPM;
        one_eighthTenpo = OneTempo / 8;
    }
    private void Update()
    {
        Metronome();
    }

    private void Metronome()
    {
        time += Time.deltaTime;
        if (time > OneTempo)
        {
            Tempo++;
            time = 0;
            Audio.clip = MetronomeSE;
            Audio.Play();
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        if (Tempo >= 4)
        {
            Tempo = 0;
        }

        /// <summary>
        /// テンポに応じて処理を行う
        /// </summary> 
        //　○　敵の移動
        if (Tempo == 3)
        {
            StartCoroutine(ToRemoveable());
        }

        //ジャストアタック
        if ((time > (OneTempo - (OneTempo / 5))) || 
            (time < (OneTempo / 2)))
        {
            removable = true;
            // Enemyのフットバシ処理を許可する
            blowable = true;
            // 抜刀スプライトに変更
        }
        else
        {
            isSound = false;
            removable = false;
            blowable = false;
        }
    }
    private IEnumerator ToRemoveable()
    {
        enemyRemovable = true;
        //Debug.Log("kai");
        yield return new WaitForSeconds(0.5f) ;
        //Debug.Log("guan");
        enemyRemovable = false;
    }
}
