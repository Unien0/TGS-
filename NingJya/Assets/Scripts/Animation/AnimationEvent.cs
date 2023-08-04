using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public PlayerData_SO playerData;

    public bool TimeInspect
    {
        //ƒvƒŒƒCƒ„[‚ªUŒ‚‚Å‚«‚é‚©‚Ç‚¤‚©‚ğ”»’f‚·‚é
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }

    public  void Levelling()
    {
        TimeInspect = true;
    }
    public  void Off()
    {
        TimeInspect = false;
    }
}
