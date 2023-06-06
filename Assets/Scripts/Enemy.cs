using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Player>().ATK)
        {
            rb2d.AddForce(FindObjectOfType<Player>().shotrote,ForceMode2D.Impulse);
        }
        else
        {
            rb2d.velocity = Vector3.zero;
        }
    }
}
