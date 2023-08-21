using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;               // Rigidbody2D�̎擾�E�i�[
    [SerializeField] private float speed;

    void Start()
    {
        // RigidBody2D�̏��i�[�E���
        rb2d = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0,0,GameObject.Find("AttackArea").transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        // �O�������Ɍ������Đi��
        rb2d.velocity = transform.up * speed;
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
