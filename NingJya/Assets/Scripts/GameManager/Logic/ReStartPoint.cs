using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // �v���C���[���|�C���g�ɐN��������
        if(col.gameObject.name == "�IPlayer")
        {
            // �e�@(ReStartManeger)�ɂ��̃I�u�W�F�N�g�̏��𑗐M����
            ReStartManeger.ReSpornPoint = transform.position;
            ReStartManeger.CanReSporn = true;
        }
    }
}
