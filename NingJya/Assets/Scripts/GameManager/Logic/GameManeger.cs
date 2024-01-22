using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region EnemyData�̕ϐ�
    private bool enemyRemovable
    {
        //�ړ��\���ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.removable; else return false; }
        set { enemyData.removable = value; }
    }
    private bool blowable
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.blowable; else return false; }
        set { enemyData.blowable = value; }
    }
    private bool isDead
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.isDead; else return false; }
        set { enemyData.isDead = value; }
    }
    #endregion
    public PlayerData_SO playerData;
    #region playerData�̕ϐ�
    public bool attackable
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //�v���C���[���ړ��ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }
    }
    public bool TimeInspect
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }
    #endregion


    public float BPM;                           // BGM���̐ݒ�
    public static int Tempo;                    // �����̎擾
    private float time;                         // �o�ߕb��
    public float OneTempo;                      // 1�e���|�Ԃ̕b�����擾
    public static bool TempoExChange;           // �e���|�̕ω����m�F����ϐ�(�S�X�N���v�g���Q��)
    public static float AnimSpeed;              // BPM�ɉ������A�j���[�V�������x(�S�X�N���v�g���Q��)

    [SerializeField] private GameObject[] animSet;

    private AudioSource Audio;                  // AudioSource�R���|�[�l���g��ۑ�����ϐ�
    private bool isSound;                       // �T�E���h��炵�����m�F����ϐ�
    // ���g���m�[���̉�����ۑ�����ϐ�
    [SerializeField] private AudioClip MetronomeSE;

    // =================
    // �X�R�A�֌W
    // =================
    private bool isFix;                         // �X�R�A���C���������ǂ����m�F����ϐ�
    private int PlusScore;                      // ���Z����X�R�A�̒l
    public static int Score ;                     // ���݂̃X�R�A��ۑ�����ϐ�

    // ���Z�X�R�A���L�ڂ���e�L�X�g�I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private TextMeshProUGUI ScorePlusTMPui;
    // �ŏI�X�R�A���L�ڂ���I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private TextMeshProUGUI ScoreBordTMPui;
    //  ���݃X�R�A���L�ڂ���I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private TextMeshProUGUI TMPui;


    public static int KillEnemy;                // �|�����G�̐�
    public static int hitEnemy;                 // �Ԃ��ē|�����G�̐�
    public static int KillBOSS;                 // �|�����{�X�̓_��
    public static int SKillScore;               // �X�L���{�[�i�X


    // =================
    // �R���{�֌W
    // =================
    // �R���{�\�L�I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private GameObject CombosObjct;
    // �R���{�Q�[�W(��������)�I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] private Slider ComboGaugeObjct;
    // �R���{����\�L����I�u�W�F�N�g��ۑ�����ϐ�
    // 1�̌��̃I�u�W�F�N�g
    [SerializeField] private Image ComboText_1;
    // 10�̌��̃I�u�W�F�N�g
    [SerializeField] private Image ComboText_2;
    // �R���{���X�v���C�g��ۑ�����ϐ�
    [SerializeField] private Sprite[] Numbers;


    public static bool ComboCheck;              // �R���{�������m�F����ϐ�
    private bool isCombo;                       // �R���{��Ԃł��邱�Ƃ��m�F����ϐ�
    private float ComboLimit;                   // �R���{�̌p������
    public static int ComboCount;               // ���݂̃R���{��
    public static int ComboMax;                 // �ő�R���{��


    public static bool TempoReset;              // �e���|�̃��Z�b�g�������󂯕t���邽�߂̕ϐ�
    public ShakeManeger shakeManeger;           // �Q�[����ʂ�h�炷�X�N���v�g�̎擾
    public static float shakeTime;              // �Q�[����ʂ�h�炵������p������

    // �X�L�������C���I�u�W�F�N�g���擾����ϐ�
    [SerializeField] private GameObject ScanLineObj;
    // �X�L�������C���I�u�W�F�N�g�̐F�̔Z����ύX����ϐ�
    private int ScanLineLevel = 1;



    private void Awake()
    {
        // �e���|��������������
        TempoChange();
        Audio = GetComponent<AudioSource>();
        enemyRemovable = false;
    }

    private void Start()
    {
        // ���X�|�[���̏����m�F
        if (ReStartManeger.CanReSporn == true)
        {
            
            // �v���C���[�̈ړ�
            FindObjectOfType<Player>().transform.position = ReStartManeger.ReSpornPoint;

            // �J�����̈ړ�(�ړ������̓J�������ōs��)
            // �J�������䐔�l�̕ύX(���M���ꂽ���l�����ɕύX)


            // ���X�^�[�g�����̏I��
            ReStartManeger.CanReSporn = false;
        }
    }
    private void Update()
    {
        ScoreCheck();
        Metronome();
        ComboChecker();
        Function();
        ShakeCheck();

        foreach (var Setobj in animSet)
        {
            Setobj.GetComponent<Animator>().SetFloat("AnimSpeed", GameManeger.AnimSpeed);
        }
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
            //Debug.Log("Tempo" + Tempo);
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
        /// �e���|�ɉ����ď������s��
        /// </summary> 
        //�@���@�G�̈ړ�
        if (Tempo == 3)
        {
            StartCoroutine(ToRemoveable());
        }

        //�W���X�g�A�^�b�N
        if ((time > (OneTempo - (OneTempo / 3))) || 
            (time < (OneTempo / 2f)))
        {
            removable = true;
            // Enemy�̃t�b�g�o�V������������
            blowable = true;
            // �����X�v���C�g�ɕύX
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


        // �R���{���Z����
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
            int data_1 = (int)(ComboCount % 10);              // 1�̈�
            int data_2 = (int)((ComboCount / 10) % 10);       // 2�̈�

            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:     // �ꌅ��
                        ComboText_1.sprite = GetSprite(data_1);
                        break;
                    case 1:     // �񌅖�
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


        int data1 = (int)(ComboCount % 10);              // 1�̈�
        int data2 = (int)((ComboCount / 10) % 10);       // 2�̈�

        for (int i = 0; i < 2; i++)
        {
            switch (i)
            {
                case 0:     // �ꌅ��
                    ComboText_1.sprite = GetSprite(data1);
                    break;
                case 1:     // �񌅖�
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
            // ���Y�����Z�b�g���s���B
            TempoChange();
            Tempo = 0;
            time = 0;
            TempoReset = false;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {// �U�������̍Đݒ���s��            
            FindObjectOfType<ConductManeger>().Enemys = new GameObject[0];
            FindObjectOfType<ConductManeger>().Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {// �v���C���[�̓����蔻��������E�Đݒ肷��
            if (FindObjectOfType<Player>().PlayerCol2D.enabled == true)
            {
                FindObjectOfType<Player>().PlayerCol2D.enabled = false;
            }
            else
            {
                FindObjectOfType<Player>().PlayerCol2D.enabled = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.F4))
        {// �V�[���̃����[�h
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*
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
        */

        /*if(Input.GetKeyUp(KeyCode.F5))
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
        }*/
        if (Input.GetKey(KeyCode.F12))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("Title");
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene("0_Tutorial");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("1stStage_Remake");
            }            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("2ndStage");
            }
        }

    }
}
