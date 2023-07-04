using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/GM Data")]
public class GMData_SO : ScriptableObject
{
    [Header("��{�v���p�e�B")]
    public float rhythmTime;

    [Header("����")]
    [Range(0, 1)]
    public float BgmVolume;
    [Range(0, 1)]
    public float SeVolume;

}
