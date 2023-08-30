using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;               // Rigidbody2D�̎擾�E�i�[
    [SerializeField] private float speed;
    private float time;
    void Start()
    {
        // RigidBody2D�̏��i�[�E���
        rb2d = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0,0,FindObjectOfType<Player>().roteMax);
    }

    // Update is called once per frame
    void Update()
    {
        // �O�������Ɍ������Đi��
        rb2d.velocity = transform.up * speed;
        time += Time.deltaTime;
        if (time >= 2)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == "Tilemap_outside_wall") || (collision.gameObject.name == "Tilemap_wall"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
