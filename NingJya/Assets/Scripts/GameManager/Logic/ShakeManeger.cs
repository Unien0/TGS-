using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeManeger : MonoBehaviour
{
    public bool isShake;
    public static int ShakeLevel;
    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        if (isShake)
        {
            switch (ShakeLevel)
            {
                case 1:
                    gamepad.SetMotorSpeeds(0.25f, 0.25f);
                    break;
                case 2:
                    gamepad.SetMotorSpeeds(0.5f, 0.5f);
                    break;
                case 3:
                    gamepad.SetMotorSpeeds(0.75f, 0.75f);
                    break;
                case 4:
                    gamepad.SetMotorSpeeds(1f, 1f);
                    break;
            }
        }
        else
        {
            InputSystem.PauseHaptics();
        }
    }
}