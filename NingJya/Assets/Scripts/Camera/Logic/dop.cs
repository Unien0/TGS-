using Cinemachine;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dop : MonoBehaviour
{
    // �v���C���[���N���������ǂ����m�F����ϐ�
    private bool Check;
    // �N�����������I��������ǂ����m�F����ϐ�
    private bool Fixed; 
    
    // �G�L�������o���C�x���g���s�����ǂ����m�F����ϐ� 
    [SerializeField]
    private bool JumpScare;
    // �o��������G�̃I�u�W�F�N�g��ۑ�����ϐ�
    [SerializeField] 
    private GameObject[] ProceedBlocks;
    // �o�����G�t�F�N�g��ۑ�����ϐ�
    [SerializeField] 
    private GameObject Smoke;
    // �|�����G�̐����L�^����ϐ�
    private int BreakPoint;

    // �J�����̌Œ�ʒu��ۑ�����ϐ�
    [SerializeField] GameObject StopPoint;
    void Start()
    {

    }

    void Update()
    {
        if (Check)
        {
            if (!Fixed)
            {
                // �i�s�x���X�V����
                Camera.Order += 1;

                // �G�̃W�����v�X�P�A��ON�ɂȂ��Ă���ꍇ
                if (JumpScare)
                {
                    foreach (var Eventobj in ProceedBlocks)
                    {
                        Eventobj.SetActive(true);
                        Instantiate(Smoke, Eventobj.transform.position, Eventobj.transform.rotation);
                    }
                }
                // �����񏈗����s��Ȃ��悤�ɂ���if����񐬗��ɂ�����
                Fixed = true;
            }
            else
            {
                if (JumpScare)
                {
                    // �J�����̈ړ����~�߂�
                    FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().enabled = false;
                    // �J�����̈ʒu��ύX����
                    FindObjectOfType<Camera>().GetComponent<Transform>().transform.position = StopPoint.transform.position;
                    // �G�̌��j�����m�F����
                    foreach (var Eventobj in ProceedBlocks)
                    {
                        // �|����Ă����ꍇ
                        if (Eventobj.gameObject.GetComponent<SpriteRenderer>().enabled == false)
                        {
                            // ����1���₷
                            BreakPoint += 1;
                        }
                        else
                        {
                            // �|����Ă��Ȃ��ꍇ���̃��[�v��E����
                            break;
                        }
                    }

                    // �|���������G�̑����Ɠ����ꍇ
                    if (BreakPoint == ProceedBlocks.Length)
                    {
                        // ���̃C�x���g���I������
                        JumpScare = false;
                        // �J�����̈ړ����ĊJ������
                        FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().enabled = true;
                    }
                    else
                    {
                        // �����łȂ��ꍇ���[�v��������蒼��
                        // ���̂��߁A�_����0�ɂ���
                        BreakPoint = 0;
                    }
                }
                else
                {
                    // �폜����
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "�IPlayer")
        {
            Check = true;
        }
    }
}
