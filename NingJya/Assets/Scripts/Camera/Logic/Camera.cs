using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // ��邱��
    // �v���C���[�Ƃ̋����̍��ɉ����āA�J�����ړ�����
    // ��ʒ[�ɋ߂Â�����A�v���C���[�𒆉��ɐ�����悤�Ɉړ�����B
    // �܂��̓v���C���[�̎擾�ϐ��ƍ������߂�ϐ�������if���𑢂�

    [SerializeField]private GameObject playerObj;
    private Vector2 PosGap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerObj = FindObjectOfType<Player>().gameObject;
        PosGap = new Vector2(playerObj.transform.position.x - this.gameObject.transform.position.x,
                            playerObj.transform.position.y - this.gameObject.transform.position.y);

        if (PosGap.x >= 2.5f)
        {
            Debug.Log("Right");
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * 5),transform.position.y,transform.position.z);
        }
        if (PosGap.x <= -2.5f)
        {
            Debug.Log("Left");
            transform.position = new Vector3(transform.position.x - (Time.deltaTime * 5), transform.position.y, transform.position.z);
        }
        if (PosGap.y >= 0.75f)
        {
            Debug.Log("Up");
            transform.position = new Vector3(transform.position.x , transform.position.y + (Time.deltaTime * 5), transform.position.z);
        }
        if (PosGap.y <= -0.75f)
        {
            Debug.Log("Down");
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 5), transform.position.z);
        }

    }
}
