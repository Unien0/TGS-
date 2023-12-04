using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;

    public CameraShake shake;
    public static bool ShakeOrder;
    // �X�e�[�W���X�e�[�^�X�̍쐬
    public enum StageName
    {
        Tutorial,
        Stage_1,
        Stage_2,
        Stage_3,
        end
    }
    // ���X�e�[�W�̐ݒ�
    [SerializeField] private StageName Process;
    // ���ݒn
    public static float Order;
    // �ő�l
    public static float Ordermax;

    [SerializeField] private GameObject DoPObj;
    [SerializeField] private Slider DoP;
    [SerializeField] private GameObject BOSSTEXT;

    void Start()
    {
        switch (Process)
        {
            case StageName.Tutorial:
                Ordermax = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShakeProcess();
        Degreeofprogress();
    }

    void Degreeofprogress()
    {
        // �i�s�x�̎擾�E�X�V
        DoP.value = Order / Ordermax;

        if (DoP.value == 1)
        {
            if (FindObjectOfType<BossStartFlag>().ActStart)
            {
                DoPObj.SetActive(true);
                BOSSTEXT.SetActive(true);
            }
        }
    }

    void ShakeProcess()
    {
        if ((ShakeOrder) || (Input.GetKeyDown(KeyCode.Y)))
        {
            shake.Shake(0.25f, 0.1f);
            ShakeOrder = false;
        }
    }

}
