using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberController : MonoBehaviour
{
    public AudioSource sounds;
    public AudioClip swish;
    public GameObject BaseLightsaber;
    //float Damage;
    public GameObject StrikeZone;
    bool AttackReady;
    public GameObject DeflectZone;
    bool DeflectReady;
    public float DeflectWait;

    // Start is called before the first frame update
    void Start()
    {
        AttackReady = true;
        DeflectReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackReady && Input.GetButtonDown("Fire1"))
        {
            sounds.PlayOneShot(swish);
            StartCoroutine(Attack());
            StartCoroutine(AttackReadying());
        }
        /*
        if (DeflectReady && Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Deflect());
            StartCoroutine(DeflectReadying());
        }
        */
        if (DeflectReady && Input.GetButtonDown("Fire2"))
        {
            //DeflectReady = false;
            BaseLightsaber.SetActive(false);
            DeflectZone.SetActive(true);
            AttackReady = false;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            DeflectZone.SetActive(false);
            BaseLightsaber.SetActive(true);
            AttackReady = true;
        }

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        
        float DMG = Damage;//scale damage based off of the bullet's speed.
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player")
        {
            HeathBar HB = collision.gameObject.GetComponent<HeathBar>();
            if (HB != null) { HB.TakeDamage(DMG); }
        }
        
    }
    */
    /*
    
    IEnumerator Deflect()
    {
        DeflectReady = false;
        DeflectZone.SetActive(true);
        //RepulseTimer = 0f;
        yield return new WaitForSeconds(.20f);
        DeflectZone.SetActive(false);
    }

    IEnumerator DeflectReadying()
    {
        yield return new WaitForSeconds(DeflectWait);
        DeflectReady = true;

        /*
        while (RepulseTimer != 1)
        {
            RepulseTimer += .01f;
            yield return new WaitForSeconds(.01f);
        }
        */
    //}

    IEnumerator Attack()
    {
        DeflectReady = false;
        AttackReady = false;
        BaseLightsaber.SetActive(false);
        StrikeZone.SetActive(true);
        //RepulseTimer = 0f;
        yield return new WaitForSeconds(.05f);
        StrikeZone.SetActive(false);
        BaseLightsaber.SetActive(true);
    }

    IEnumerator AttackReadying()
    {
        yield return new WaitForSeconds(DeflectWait);
        DeflectReady = true;
        AttackReady = true;

        /*
        while (RepulseTimer != 1)
        {
            RepulseTimer += .01f;
            yield return new WaitForSeconds(.01f);
        }
        */
    }

    public bool GetRepulsorReady()
    {
        return DeflectReady;
    }

    public float GetWait()
    {
        return DeflectWait / 100;
    }

}
