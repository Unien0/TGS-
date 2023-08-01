using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectDestory : MonoBehaviour
{
    [SerializeField] private float DeadTime;
    private float time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= DeadTime)
        {
            Destroy(gameObject);
        }
    }
}
