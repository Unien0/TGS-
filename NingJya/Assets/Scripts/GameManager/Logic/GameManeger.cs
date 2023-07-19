using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
