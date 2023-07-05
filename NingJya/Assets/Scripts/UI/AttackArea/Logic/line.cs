using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    private float startPos = -0.5f;
    private float endPos = 0.5f;
    private float move;
    void Start()
    {
        move = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        move += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, move, 0);


    }
    private void LateUpdate()
    {
        if (move >= endPos)
        {
            transform.position = new Vector3(transform.position.x, startPos, 0);
        }
    }
}
