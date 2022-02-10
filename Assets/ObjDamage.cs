using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDamage : MonoBehaviour
{
    Rigidbody ObjRigid;

    // Start is called before the first frame update
    void Start()
    {
        ObjRigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollide(Collision collision)
    {
        float DMG = (ObjRigid.velocity.magnitude * 5);//scale damage based off of the bullet's speed.
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Boss")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
    }
}
