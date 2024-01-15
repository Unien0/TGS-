using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "ÅIPlayer")
        {
            Debug.Log("Do");
            FindObjectOfType<Player>().Stay = true;
            col.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            col.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            col.transform.position = this.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "ÅIPlayer")
        {
            FindObjectOfType<Player>().Stay = false;
        }
    }
}
