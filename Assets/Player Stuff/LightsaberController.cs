using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberController : MonoBehaviour
{

    float Damage;
    GameObject BRb;

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
        /**
        float DMG = (BRb.velocity.magnitude * Damage);//scale damage based off of the bullet's speed.
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player")
        {
            HealthBar HB = collision.gameObject.GetComponent<HealthBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
        **/
    }

}