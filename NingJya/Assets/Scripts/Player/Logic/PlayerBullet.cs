using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;               // Rigidbody2D�̎擾�E�i�[
    public GameObject conductObject;
    public bool Ready;
    private bool shot;
    public float BulletSpeed;
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;

    void Start()
    {
        // RigidBody2D�̏��i�[�E���
        rb2d = GetComponent<Rigidbody2D>();
        FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
        FindObjectOfType<ConductManeger>().conduct = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shot)
        {
            if (Ready)
            {
                if (conductObject != null)
                {
                    //����
                    shotrote = new Vector2(conductObject.transform.position.x - this.transform.position.x, conductObject.transform.position.y - this.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    //���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * BulletSpeed);
                    shot = true;
                }
                else
                {
                    //����
                    shotrote = new Vector2(this.transform.position.x - FindObjectOfType<Player>().transform.position.x, this.transform.position.y - FindObjectOfType<Player>().transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * BulletSpeed);
                    shot = true;
                }
            }
        }
    }
}
