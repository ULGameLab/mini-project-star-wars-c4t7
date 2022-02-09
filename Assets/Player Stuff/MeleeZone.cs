using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        float DMG = 25;
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Boss")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
        */

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        {
            HeathBar HB = other.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(25f); }
        }
        
    }
}
