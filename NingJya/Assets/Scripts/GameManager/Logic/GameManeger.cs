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


    public float BPM;
    public static int Tempo;
    private double time;
    public double OneTempo;
    // 全スクリプトが参照にするテンポ変化確認
    public static bool TempoExChange;
    // 全スクリプトが参照にするアニメーション速度
    public static float AnimSpeed;

    private AudioSource Audio;
    [SerializeField] private AudioClip MetronomeSE;
    bool isSound;

    // スコア関係
    public int Score;
    private string Scoretext;
    [SerializeField] private TextMeshProUGUI TMPui;

    // 倒した敵の数
    public static int KillEnemy;
    // ぶつけて倒した敵の数
    public static int hitEnemy;
    // 手にしたコインの数
    public static int GetCoin;
    // 倒したボスの数
    public static int KillBOSS;

    //コンボオブジェクト
    [SerializeField] private GameObject CombosObjct;
    // コンボテキスト
    [SerializeField] private TextMeshProUGUI ComboText;
   // コンボの確認
   public  static bool ComboCheck;
    // コンボ中(ステータス)
    private bool isCombo;
    // コンボの制限時間
    [SerializeField]private float ComboLimit;
    // 現在コンボ数
    public static int ComboCount;
    // 最大コンボ数
    [SerializeField] private int ComboMax;
    public static bool TempoReset;
    private void Awake()
    {
        TempoChange();
    }

    private void Start()
    {        
        Audio = GetComponent<AudioSource>();
        enemyRemovable = false;
    }
    private void Update()
    {
        ScoreCheck();
        Metronome();
        ComboChecker();
        Function();
    }

    void TempoChange()
    {
        OneTempo = 60 / BPM;
        AnimSpeed = BPM / 140;
    }

    private void Metronome()
    {
        time += Time.deltaTime;
        if (time >= OneTempo)
        {
            Tempo++;            
            time -= OneTempo;
            TimeInspect = true;
            Audio.clip = MetronomeSE;
            Audio.Play();
            isDead = true;
            FindObjectOfType<BossEnemy>().ExChange = true;
            TempoExChange = true;
        }
        else
        {
            TempoExChange = false;
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
        if ((time > (OneTempo - (OneTempo / 4))) || 
            (time < (OneTempo / 4)))
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

    public void ScoreCheck()
    {
        Score = ((KillEnemy * 100)
               + (hitEnemy * 200) 
               + (GetCoin * 50) 
               + (KillBOSS)
               + (ComboMax * 100));
        TMPui.text = Score.ToString();
    }

    void ComboChecker()
    {
        if (ComboCheck)
        {
            if (!isCombo)
            { 
                isCombo = true;                
            }
            ComboLimit = 5;
            ComboCheck = false;
            ComboCount++;
        }
        if (isCombo)
        {
            CombosObjct.SetActive(true);
            ComboText.text = ComboCount.ToString();
            ComboLimit -= Time.deltaTime;
            if (ComboLimit <= 0)
            {
                if (ComboMax <= ComboCount)
                {
                    ComboMax = ComboCount;
                }
                isCombo = false;
            }
        }
        else
        {
            ComboCount = 0;
            CombosObjct.SetActive(false);
        }
    }

    void Function()
    {
        if ((Input.GetKeyDown(KeyCode.F1)) || (TempoReset))
        {
            // リズムリセットを行う。
            TempoChange();
            Tempo = 0;
            time = 0;
            TempoReset = false;
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
