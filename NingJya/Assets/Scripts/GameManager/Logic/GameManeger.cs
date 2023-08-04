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


    [SerializeField] private float BPM;
    public static int Tempo;
    private double time;
    public double OneTempo;
    public static double one_eighthTenpo;
    public static double one_eighthCount;
    private AudioSource Audio;
    [SerializeField] private AudioClip MetronomeSE;
    bool isSound;

    // �X�R�A�֌W
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
        /// �e���|�ɉ����ď������s��
        /// </summary> 
        //�@���@�G�̈ړ�
        if (Tempo == 3)
        {
            StartCoroutine(ToRemoveable());
        }

        //�W���X�g�A�^�b�N
        if ((time > (OneTempo - (OneTempo / 5))) || 
            (time < (OneTempo / 2)))
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
            // ���Y�����Z�b�g���s���B
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
