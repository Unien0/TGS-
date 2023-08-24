using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeManeger : MonoBehaviour
{
    public bool isShake;

    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        if (isShake)
        {
            gamepad.SetMotorSpeeds(0.25f, 0.25f);
        }
        else
        {
            InputSystem.PauseHaptics();
        }
    }
}