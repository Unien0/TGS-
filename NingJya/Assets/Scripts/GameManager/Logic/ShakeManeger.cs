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
            gamepad.SetMotorSpeeds(1.0f, 0.0f);
        }
        else
        {
            InputSystem.PauseHaptics();
        }
    }
}