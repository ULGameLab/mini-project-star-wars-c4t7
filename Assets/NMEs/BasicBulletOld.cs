using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just toss this onto the bullet model directly
//just tag the objects that you want the bullet to damage as "Player" or "Enemy"
public class BasicBulletOld : MonoBehaviour
{
    GameObject Bullet;
    Rigidbody BRb;
    GameObject GunHldr;
    float lifeSpan = 5;//this is fixed as no real situation should take bullet travel time over 5 seconds.
    public float Damage;
    float DMG;//this is the damage that the bullet will do, calculated off current speed, mass, and damage modifyers from the gun.

    void Start()
    {
        Bullet = this.gameObject;
        BRb = Bullet.GetComponent<Rigidbody>();
        StartCoroutine(KillBullet(lifeSpan));
    }

    IEnumerator KillBullet(float temp)
    {
        yield return new WaitForSeconds(temp);
        Destroy(Bullet);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //deal damage to enemies here, change the tag to layer detection. make another clolider to expand a transform for explosives.
        //i realized that i can jsut parrot my layers to my tags, and have the benifits of having both systems without having to remember which uses what.
        //because trying to figure out collision of a layer is PAIN and if (collision.collider.gameObject.layer == enemyL) does NOTHING!!
        DMG = (BRb.velocity.magnitude * Damage);//scale damage based off of the bullet's speed.
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
    }
    /*
    public GameObject FindClosest()
    {
        //this is yoinked DIRECTLY from https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html, its an interesting method that can be yoinked for anyone to use if they want it
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("GunHolder");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = Bullet.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    */
}