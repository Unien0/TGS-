using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgmchange : MonoBehaviour
{
    [SerializeField] private int NextAudioNumber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ÅIPlayer")
        {
            FindObjectOfType<AudioManager>().Ex = true;
            FindObjectOfType<AudioManager>().AudioNumber = NextAudioNumber;
        }
    }
}
