using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Force : MonoBehaviour
{
    public Transform TheDestination;
    public Transform ThePlayer;
    
    public ForceLevelStatus forceStatus;
    private int forceReduction = 20;
    public int theForce;
    


    void Update()
    {
        theForce = forceStatus.getForce();
        if ((TheDestination.transform.position - this.transform.position).sqrMagnitude < 4)
        {
            if (Input.GetKeyDown(KeyCode.E) && theForce >= forceReduction)//if grab
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().detectCollisions = true;
                this.transform.position = TheDestination.position;
                this.transform.parent = GameObject.Find("ForcePoint").transform;
                forceStatus.AddForce(-forceReduction);
            }
            /*
            if (Input.GetKeyUp(KeyCode.E))// release it
            {
                GetComponent<Rigidbody>().useGravity = true;
                this.transform.parent = null;
                //GetComponent<Rigidbody>().useGravity = true;
            }
            */
            if (Input.GetKeyDown(KeyCode.F) && theForce >= forceReduction)// if push it
            {
                if (theForce > 100)
                {
                    GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2800); //1400
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().detectCollisions = true;
                    this.transform.parent = null;
                }
                else
                {
                    GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 1400); //700
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().detectCollisions = true;
                    this.transform.parent = null;
                }
                forceStatus.AddForce(-forceReduction);
            }
            if (Input.GetKeyDown(KeyCode.R) && theForce >= forceReduction)// if pull it
            {
                if (theForce > 100)
                {
                    GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 2800 * -1);
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().detectCollisions = true;
                    this.transform.parent = null;
                }
                else
                {
                    GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 1400 * -1);
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().detectCollisions = true;
                    this.transform.parent = null;
                }
                forceStatus.AddForce(-forceReduction);
            }

        }
        if (Input.GetKeyUp(KeyCode.E))// release it
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().detectCollisions = true;
            this.transform.parent = null;
            //GetComponent<Rigidbody>().useGravity = true;
        }


    }
    
}
