using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheack : MonoBehaviour
{
    void Update()
    {
        
        float hr = Input.GetAxis("Horizontal_Right");
        float vr = Input.GetAxis("Vertical_Right");
        //Debug.Log("RightX" + hr);
        //Debug.Log("RightY" + vr);
        if (hr != 0|| vr != 0)
        {
            Debug.Log("RightX" + hr);
            Debug.Log("RightY" + vr);
        }
    }
}
