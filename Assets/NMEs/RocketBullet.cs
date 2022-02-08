using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : MonoBehaviour
{

//use case
//EmptyObject  (Holder that holds script, collider, and rigidbody)
//  Sphere     (basic primitive with a clear material for the explosion visual effect. needs to be the first child for this to work)
//  Model      (this can be anything you imported)
//alternate use case
//Model        (whatever you imported)
//  Sphere     (same as above)
    [Tooltip("this is the reference to the bullet gameobject")]
    public GameObject Bullet;
    Rigidbody BRb;
    float lifeSpan = 5;//this is fixed as no real situation hsould take bullet travel time over 5 seconds.
    float DMG;//this is the damage that the bullet will do, calculated off current speed, mass, and damage modifyers from the gun.

    GameObject ExplosionBall;//this is the reference to the transform of the explosion ball
    bool IsExploded = false;

    void Start()
    {

        BRb = Bullet.GetComponent<Rigidbody>();
        ExplosionBall = Bullet.transform.GetChild(0).gameObject;
        BRb.mass = 1;
        Bullet.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(KillBullet(lifeSpan));
    }
    private void Update()
    {
        if (Bullet != null) { DMG = 7.5f; }
    }
    IEnumerator KillBullet(float temp)
    {
        yield return new WaitForSeconds(temp);
        Destroy(Bullet);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
        if (!IsExploded)
        {
            IsExploded = true;//flipped so that it only explodes once
            float tempS = ExplosionBall.transform.localScale.x;//this gets the scale of the explosion ball inside the grenade to scale it upwards for simpler math
            float temp = (tempS * 5000) + (DMG * .15f);
            ExplosionBall.transform.localScale = new Vector3(temp, temp, temp);//this make sthe explosion visible with a simple effect

            Collider[] colliders = Physics.OverlapSphere(ExplosionBall.transform.position, temp * tempS);//this gets a reference to all colliders within a radius
            for (int i = 0; i < colliders.Length; i++)
            {
                //this is the explosive force of the grenade, applied to all rigidbodies within the explosion radius
                Rigidbody RB = colliders[i].GetComponent<Rigidbody>();
                if (RB != null) { RB.AddExplosionForce(DMG * 3 , ExplosionBall.transform.position, temp, 0, ForceMode.Impulse); }

                if (collision.collider.tag == "Player")
                {
                    HeathBar HB = colliders[i].GetComponent<HeathBar>();
                    if (HB != null) { HB.TakeDamage(DMG * 1.75f); }
                }
                /*
                //this gets the healthbar script from any collider caught inside of the radius, and deals damage to them
                HeathBar HB = colliders[i].GetComponent<HeathBar>();
                if (HB != null) { HB.TakeDamage(DMG * 1.75f); }
                */
            }
        }
        StartCoroutine(KillBullet(.04f));
    }
}
