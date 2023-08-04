using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public bool TimeInspect
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }
    #endregion


    [SerializeField] private float BPM;
    public static int Tempo;
    private double time;
    public double OneTempo;
    public static double one_eighthTenpo;
    public static double one_eighthCount;
    private AudioSource Audio;
    [SerializeField] private AudioClip MetronomeSE;
    bool isSound;

    // スコア関係
    public int Score;
    private string Scoretext;
    [SerializeField] private TextMeshProUGUI TMPui;
    public static int KillEnemy;
    public static int hitEnemy;
    public static int GetCoin;
    public static int KillBOSS;

    private void Start()
    {        
        Audio = GetComponent<AudioSource>();
        enemyRemovable = false;
        OneTempo =60 / BPM;
        one_eighthTenpo = OneTempo / 8;
    }
    private void Update()
    {
        ScoreCheck();
        Metronome();
        Function();
    }

    private void Metronome()
    {
        time += Time.deltaTime;
        if (time >= OneTempo)
        {
            Tempo++;            
            time -= OneTempo;
            TimeInspect = true;
            Debug.Log("timeto");
            //Audio.clip = MetronomeSE;
            //Audio.Play();
            isDead = true;
            FindObjectOfType<BossEnemy>().ExChange = true;
            //StartCoroutine(ToTimeInspect());            
        }
        else
        {
            isDead = false;
            TimeInspect = false;
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

    //private IEnumerator ToTimeInspect()
    //{
    //    TimeInspect = true;
    //    yield return new WaitForSeconds(0.1f);
    //    TimeInspect = false;
    //}

    public void ScoreCheck()
    {
        Score = ((KillEnemy * 100)
               + (hitEnemy * 200) 
               + (GetCoin * 50) 
               + (KillBOSS * 1000));
        TMPui.text = Score.ToString();
    }

    void Function()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // リズムリセットを行う。
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {// 誘導挙動の再設定を行う            
            FindObjectOfType<ConductManeger>().Enemys = new GameObject[0];
            FindObjectOfType<ConductManeger>().Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {// プレイヤーの当たり判定を解除・再設定する
            FindObjectOfType<Player>().PlayerCol2D.enabled = !enabled;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {// シーンのリロード
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
