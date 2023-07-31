using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private enum ItemName
    {
        LIFEUP,
        SCOREUP,
        END
    }
    [SerializeField]private ItemName itemType;
    private Collider2D col2D;
    private SpriteRenderer SpR2D;
    private AudioSource SoundSource;
    [SerializeField] private AudioClip GetSound;

    void Start()
    {
        col2D = GetComponent<Collider2D>();
        SpR2D = GetComponent<SpriteRenderer>();
        SoundSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "ÅIPlayer")
        {
            col2D.enabled = false;
            SpR2D.enabled = false;
            switch (itemType)
            {
                case ItemName.LIFEUP:
                    FindObjectOfType<Player>().hp = FindObjectOfType<Player>().hp + 2;
                    break;
                case ItemName.SCOREUP:
                    GameManeger.GetCoin++;
                    break;
            }
            SoundSource.clip = GetSound;
            SoundSource.Play();
        }
    }
}
