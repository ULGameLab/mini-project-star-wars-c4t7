using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public Transform TheDestination;
    public Transform ThePlayer;
    void Update()
    {
        if ((TheDestination.transform.position - this.transform.position).sqrMagnitude < 4)
        {
            if (Input.GetKey(KeyCode.E))//if grab
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().detectCollisions = true;
                this.transform.position = TheDestination.position;
                this.transform.parent = GameObject.Find("ForcePoint").transform;
            }
            else// release it
            {
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
            }
            if (Input.GetKeyDown(KeyCode.F))// if push it
            {
                GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 700);
            }

            if (Input.GetKeyDown(KeyCode.R))// if pull it
            {
                GetComponent<Rigidbody>().AddForce(ThePlayer.transform.forward * 700 * -1);
            }
        }
    }
}
