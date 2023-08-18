using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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


    public float BPM;
    public static int Tempo;
    private double time;
    public double OneTempo;
    // �S�X�N���v�g���Q�Ƃɂ���e���|�ω��m�F
    public static bool TempoExChange;
    // �S�X�N���v�g���Q�Ƃɂ���A�j���[�V�������x
    public static float AnimSpeed;

    private AudioSource Audio;
    [SerializeField] private AudioClip MetronomeSE;
    bool isSound;

    // �X�R�A�֌W
    public int Score;
    private string Scoretext;
    [SerializeField] private TextMeshProUGUI TMPui;

    // �|�����G�̐�
    public static int KillEnemy;
    // �Ԃ��ē|�����G�̐�
    public static int hitEnemy;
    // ��ɂ����R�C���̐�
    public static int GetCoin;
    // �|�����{�X�̐�
    public static int KillBOSS;

    //�R���{�I�u�W�F�N�g
    [SerializeField] private GameObject CombosObjct;
    // �R���{�e�L�X�g
    [SerializeField] private TextMeshProUGUI ComboText;
   // �R���{�̊m�F
   public  static bool ComboCheck;
    // �R���{��(�X�e�[�^�X)
    private bool isCombo;
    // �R���{�̐�������
    [SerializeField]private float ComboLimit;
    // ���݃R���{��
    public static int ComboCount;
    // �ő�R���{��
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
        /// �e���|�ɉ����ď������s��
        /// </summary> 
        //�@���@�G�̈ړ�
        if (Tempo == 3)
        {
            StartCoroutine(ToRemoveable());
        }

        //�W���X�g�A�^�b�N
        if ((time > (OneTempo - (OneTempo / 4))) || 
            (time < (OneTempo / 4)))
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
            FindObjectOfType<Player>().PlayerCol2D.enabled = !enabled;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {// �V�[���̃����[�h
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
