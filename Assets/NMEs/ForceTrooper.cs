using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceTrooper : MonoBehaviour
{
    public Transform TheDestination;
    public Transform ThePlayer;
    bool PushReady = true;
    bool PullReady = true;

    public ForceLevelStatus forceStatus;
    private int forceReduction = 20;
    public int theForce;

    // Update is called once per frame
    void Update()
    {
        theForce = forceStatus.getForce();
        if ((TheDestination.transform.position - this.transform.position).sqrMagnitude < 4)
        {
            if (Input.GetKeyDown(KeyCode.F) && theForce >= forceReduction)// if push it
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
        /*
        PushReady = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 3000);
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        PushReady = true;
        */


        if (theForce > 100)
        {
            PushReady = false;
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 6000);
            yield return new WaitForSeconds(3f);
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<NavMeshAgent>().enabled = true;
            PushReady = true;
        }
        else
        {
            PushReady = false;
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 3000);
            yield return new WaitForSeconds(3f);
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<NavMeshAgent>().enabled = true;
            PushReady = true;
        }
        forceStatus.AddForce(-forceReduction);
    }

    IEnumerator Pull()
    {
        /*
        PullReady = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2000 * -1);
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        PullReady = true;
        */


        if (theForce > 100)
        {
            PushReady = false;
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 4000 * -1);
            yield return new WaitForSeconds(3f);
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<NavMeshAgent>().enabled = true;
            PushReady = true;
        }
        else
        {
            PushReady = false;
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            //GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2000 * -1);
            yield return new WaitForSeconds(3f);
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<NavMeshAgent>().enabled = true;
            PushReady = true;
        }
        forceStatus.AddForce(-forceReduction);
    }
}

