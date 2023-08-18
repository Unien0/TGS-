using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgmchange : MonoBehaviour
{
    [SerializeField]private bool No;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (No)
        {
            if (FindObjectOfType<AudioManager>().AudioNumber == 3)
            {
                FindObjectOfType<AudioManager>().Ex = true;
            }
        }
        else
        {
            if (FindObjectOfType<AudioManager>().AudioNumber == 4)
            {
                FindObjectOfType<AudioManager>().Ex = true;
            }
        }
    }
}
