using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 shotrote;
    private GameObject Enemyobj;
    public bool ATK;
    private float time;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ATK = true;
        }

        if (ATK)
        {
            time += Time.deltaTime;
            shotrote = new Vector2(Enemyobj.transform.position.x - this.transform.position.x, Enemyobj.transform.position.y - this.transform.position.y);
            
            if(time >= 1)
            {
                time = 0;
                ATK = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemyobj = col.gameObject;
        }
    }
}
