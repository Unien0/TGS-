using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/GM Data")]
public class GMData_SO : ScriptableObject
{
    [Header("基本プロパティ")]
    public float rhythmTime;

    [Header("音量")]
    [Range(0, 1)]
    public float BgmVolume;
    [Range(0, 1)]
    public float SeVolume;

}
