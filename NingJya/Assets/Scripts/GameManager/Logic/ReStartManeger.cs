using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartManeger : MonoBehaviour
{
    // ���X�|�[���|�C���g�̕ۑ�
    public Vector2 ReSpornPoint;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
