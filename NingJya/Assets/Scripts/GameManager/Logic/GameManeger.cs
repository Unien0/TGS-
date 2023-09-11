using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    private bool isFix;
    [SerializeField] private int PlusScore;
    public int Score = 000;
    [SerializeField] private TextMeshProUGUI ScorePlusTMPui;
    [SerializeField] private TextMeshProUGUI ScoreBordTMPui;
    [SerializeField] private TextMeshProUGUI TMPui;

    // 倒した敵の数
    public static int KillEnemy;
    // ぶつけて倒した敵の数
    public static int hitEnemy;
    // 倒したボスの点数
    public static int KillBOSS;
    // スキルボーナス
    public static int SKillScore;

    //コンボオブジェクト
    [SerializeField] private GameObject CombosObjct;
    // コンボゲージオブジェクト
    [SerializeField] private Slider ComboGaugeObjct;
    // コンボテキスト
    [SerializeField] private Image ComboText_1;
    [SerializeField] private Image ComboText_2;
    [SerializeField] private Sprite[] Numbers;
   // コンボの確認
   public  static bool ComboCheck;
    // コンボ中(ステータス)
    private bool isCombo;
    // コンボの制限時間
    [SerializeField]private float ComboLimit;
    // 現在コンボ数
    public static int ComboCount;
    // 最大コンボ数
    public static int ComboMax;
    public static bool TempoReset;
    public ShakeManeger shakeManeger;
    public static float shakeTime;

    // スキャンラインオブジェクト
    [SerializeField] private GameObject ScanLineObj;
    [SerializeField] private int ScanLineLevel = 1;



    private void Awake()
    {
        TempoChange();
    }

    private void Start()
    {        
        Audio = GetComponent<AudioSource>();
        enemyRemovable = false;
        Score = 0;
    }
    private void Update()
    {
        ScoreCheck();
        Metronome();
        ComboChecker();
        Function();
        ShakeCheck();
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
            //Audio.clip = MetronomeSE;
            //Audio.Play();
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
        if ((time > (OneTempo - (OneTempo / 3))) || 
            (time < (OneTempo / 3)))
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
        TMPui.text = Score.ToString();
        ScoreBordTMPui.text = Score.ToString();


        // コンボ精算処理
        if (!isCombo)
        {
            if (!isFix)
            {
                Score = Score + PlusScore;
                isFix = true;
            }
            else
            {
                PlusScore = 0;

                KillEnemy = 0;
                hitEnemy = 0;
                ComboMax = 0;
                ScorePlusTMPui.text = " ";
            }
        }
        else
        {
            isFix = false;
            PlusScore = ((KillEnemy * 100)
                        + (hitEnemy * 200)
                        + (ComboMax * 100));
            ScorePlusTMPui.text = ("+" + PlusScore.ToString());
        }

        if (KillBOSS > 0)
        {
            Score = Score + 100;
            KillBOSS = KillBOSS - 100;
        }

        if (SKillScore >0)
        {
            Score = Score + 50;
            SKillScore = SKillScore - 50;
        }
    }

    void ComboChecker()
    {
        if (ComboCheck)
        {
            if (!isCombo)
            { 
                isCombo = true;                
            }
            ComboLimit = 2.5f;
            ComboCheck = false;
            ComboCount++;
        }
        if (isCombo)
        {
            int data_1 = (int)(ComboCount % 10);              // 1の位
            int data_2 = (int)((ComboCount / 10) % 10);       // 2の位

            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:     // 一桁目
                        ComboText_1.sprite = GetSprite(data_1);
                        break;
                    case 1:     // 二桁目
                        if (data_2 == 0)
                        {
                            ComboText_2.enabled = false;
                        }
                        else
                        {
                            ComboText_2.enabled = true;
                            ComboText_2.sprite = GetSprite(data_2);
                        }
                        break;
                }
            }

            ComboLimit -= Time.deltaTime;

            if (ComboLimit <= 2f)
            {
                ComboGaugeObjct.value = ComboLimit / 2f;
            }

            if (ComboLimit <= 0)
            {
                if (ComboMax <= ComboCount)
                {
                    ComboMax = ComboCount;
                }

                if (ComboLimit <= 0)
                {
                    isCombo = false;
                }
            }
        }
        else
        {
            ComboCount = 0;
        }


        int data1 = (int)(ComboCount % 10);              // 1の位
        int data2 = (int)((ComboCount / 10) % 10);       // 2の位

        for (int i = 0; i < 2; i++)
        {
            switch (i)
            {
                case 0:     // 一桁目
                    ComboText_1.sprite = GetSprite(data1);
                    break;
                case 1:     // 二桁目
                    if (data2 == 0)
                    {
                        ComboText_2.enabled = false;
                    }
                    else
                    {
                        ComboText_2.enabled = true;
                        ComboText_2.sprite = GetSprite(data2);
                    }
                    break;
            }
        }
    }

    Sprite GetSprite(int number)
    {
        Sprite img = Numbers[0];
        switch (number)
        {
            case 0:
                img = Numbers[0];
                break;
            case 1:
                img = Numbers[1];
                break;
            case 2:
                img = Numbers[2];
                break;
            case 3:
                img = Numbers[3];
                break;
            case 4:
                img = Numbers[4];
                break;
            case 5:
                img = Numbers[5];
                break;
            case 6:
                img = Numbers[6];
                break;
            case 7:
                img = Numbers[7];
                break;
            case 8:
                img = Numbers[8];
                break;
            case 9:
                img = Numbers[9];
                break;
        }
        return img;
    }

    void ShakeCheck()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            shakeTime = 1;
        }
        shakeTime -= Time.deltaTime;
        if (shakeTime > 0)
        {
            shakeManeger.isShake = true;
        }
        else
        {
            shakeManeger.isShake = false;
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
        if (Input.GetKey(KeyCode.F5))
        {
            if ((Input.GetKeyDown(KeyCode.C)))
            {
                ScanLineLevel = ScanLineLevel + 1;
            }
            else if ((Input.GetKeyDown(KeyCode.V)))
            {

                    ScanLineLevel = ScanLineLevel - 1;

            }
        }
        if(Input.GetKeyUp(KeyCode.F5))
        {
            switch (ScanLineLevel)
            {
                case 0:
                    ScanLineObj.GetComponent<Animator>().SetFloat("Level", -1.1f);
                    break;
                case 1:
                    ScanLineObj.GetComponent<Animator>().SetFloat("Level", 0.0f);
                    break;
                case 2:
                    ScanLineObj.GetComponent<Animator>().SetFloat("Level", 1.1f);
                    break;
                case 3:
                    ScanLineObj.GetComponent<Animator>().SetFloat("Level", 2.2f);
                    break;
            }
        }
        if (Input.GetKey(KeyCode.F12))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SceneManager.LoadScene("1stStage");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene("2ndStage");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("3rdStage");
            }
        }

    }
}
