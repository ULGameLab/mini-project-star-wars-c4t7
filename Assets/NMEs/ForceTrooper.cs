using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTrooper : MonoBehaviour
{
    public Transform TheDestination;
    public Transform ThePlayer;
    bool PushReady = true;
    bool PullReady = true;

    // Update is called once per frame
    void Update()
    {
        if ((TheDestination.transform.position - this.transform.position).sqrMagnitude < 4)
        {
            if (Input.GetKey(KeyCode.E))//if grab
            {
                GetComponent<Rigidbody>().useGravity = false;
                //GetComponent<Rigidbody>().isKinematic = false;
                this.transform.position = TheDestination.position;
                this.transform.parent = GameObject.Find("ForcePoint").transform;
            }
            else// release it
            {
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                //GetComponent<Rigidbody>().isKinematic = true;
            }
            if (PushReady && Input.GetKeyDown(KeyCode.F))// if push it
            {
                StartCoroutine(Push());
                //GetComponent<Rigidbody>().isKinematic = false;
                //GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 3000);
                //GetComponent<Rigidbody>().isKinematic = true;
            }

            if (PullReady && Input.GetKeyDown(KeyCode.R))// if pull it
            {
                StartCoroutine(Pull());
                //GetComponent<Rigidbody>().isKinematic = false;
                //GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2000 * -1);
                //GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    IEnumerator Push()
    {
        PushReady = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 3000);
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        PushReady = true;
    }

    IEnumerator Pull()
    {
        PullReady = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2000 * -1);
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        PullReady = true;
    }
}

