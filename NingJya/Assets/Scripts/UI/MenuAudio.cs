using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public void MenuSelectAudio()
    {
        AudioManager.Instance.PlaySE("MenuSelect");
    }
}
