using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just toss this onto the bullet model directly
public class BasicBulletOld : MonoBehaviour
{
    GameObject Bullet;
    Rigidbody BRb;
    GameObject GunHldr;
    OldGun GS;
    float lifeSpan = 5;//this is fixed as no real situation hsould take bullet travel time over 5 seconds.
    float DMG;//this is the damage that the bullet will do, calculated off current speed, mass, and damage modifyers from the gun.

    void Start()
    {
        Bullet = this.gameObject;
        GunHldr = FindClosest();
        GS = GunHldr.GetComponent<OldGun>();

        BRb = Bullet.GetComponent<Rigidbody>();
        BRb.mass = GS.FiredM;

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
        DMG = GS.FiredD * GS.GridMultD * (BRb.velocity.magnitude * .45f) * (GS.FiredM * Mathf.Pow(GS.GridMultS, 3));//scale damage based off of the bullet's grid multiplyers, speed, and mass in that order.
        if (collision.collider.tag == "Enemy")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
    }
    public GameObject FindClosest()
    {
        //this is yoinked DIRECTLY from https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html, with NO SHAME in a direct copya s writing it myself would have been realisticly the same thing anyway. 
        //i used this as i know way later on i want to have enemies with their own guns with their own grids. meaning the player won't be the only one with these scripts or tags.
        //also stupidly far down the line multiplayer is an option, so multiple players have the same argument as enemies.
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
}