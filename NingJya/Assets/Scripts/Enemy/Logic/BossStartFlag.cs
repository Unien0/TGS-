using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartFlag : MonoBehaviour
{
    public bool ActStart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActStart = true;
        }
    }
}
