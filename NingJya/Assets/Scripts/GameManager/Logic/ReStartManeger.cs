using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartManeger : MonoBehaviour
{
    // ���X�|�[���|�C���g�̕ۑ�(�e�@)
    public static Vector2 ReSpornPoint;

    // ���X�|�[���̋���
    public static bool CanReSporn;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
