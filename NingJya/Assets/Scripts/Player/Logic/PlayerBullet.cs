using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;               // Rigidbody2D�̎擾�E�i�[
    private GameObject animObj;
    private Animator anim;                  // Animator�R���|�[�l���g��ۑ�����ϐ�
    private Collider2D col2d;
    [SerializeField] private float speed;
    private float time;
    private bool Stop;
    private string TagName;
    void Start()
    {
        // RigidBody2D�̏��i�[�E���
        rb2d = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0,0,FindObjectOfType<Player>().roteMax);
        animObj = transform.GetChild(0).gameObject;
        anim = animObj.GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!Stop)
        {
            // �O�������Ɍ������Đi��
            rb2d.velocity = transform.up * speed;

            if (time >= 2)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            anim.speed = 0;
            if (GameManeger.TempoExChange)
            {
                if (TagName == "Enemy")
                {
                    Destroy(this.gameObject,0.2f);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == "Tilemap_outside_wall") || (collision.gameObject.name == "Tilemap_wall") || (collision.gameObject.CompareTag("Enemy")))
        {
            col2d.enabled = false;
            Stop = true;
            time = 0;
            TagName = collision.gameObject.tag;
        }
    }
}
