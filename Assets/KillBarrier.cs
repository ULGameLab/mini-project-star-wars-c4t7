using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss" || other.gameObject.tag == "Player")
        {
            HeathBar HB = other.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(999999f); }
        }

    }
}
