using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartPoint : MonoBehaviour
{
    // ���Ԗڂ̃Z�[�u�|�C���g�ł��邩���m�F����(1���琔���n�߂�)
    [SerializeField] private float SaveOrderNumber;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �v���C���[���|�C���g�ɐN��������
        if(col.gameObject.name == "�IPlayer")
        {
            // �e�@(ReStartManeger)�ɂ��̃I�u�W�F�N�g�̏��𑗐M����
            ReStartManeger.ReSpornPoint = transform.position;
            ReStartManeger.SaveOrder = SaveOrderNumber;
        }
    }
}
