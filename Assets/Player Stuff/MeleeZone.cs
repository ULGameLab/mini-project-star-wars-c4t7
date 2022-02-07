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

        float DMG = 50;
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }

    }
}
