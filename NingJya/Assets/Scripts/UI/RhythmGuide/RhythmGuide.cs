using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGuide : MonoBehaviour
{
    private float animSpeed;                // �A�j���[�V�������x�̒l��ۑ�����ϐ�
    private Animator anim;                  // Animator�R���|�[�l���g��ۑ�����ϐ�

    void Start()
    {
        // Animator�R���|�[�l���g��ۑ�����
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);
    }
}
