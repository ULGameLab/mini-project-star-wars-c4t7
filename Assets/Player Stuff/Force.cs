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
    public Rigidbody temp;
    public Vector3 GforceV;
    public float GforveM;
    Vector3 Acc;
    Vector3 Vel;
     void Start()
    {
        
    }
    void attract()
    {
        //while((TheDestination.transform.position - this.transform.position).sqrMagnitude > 4 && (TheDestination.transform.position - this.transform.position).sqrMagnitude < 24)
       // {
            Rigidbody BodyToattract = GetComponent<Rigidbody>();
            Vector3 direction = TheDestination.position - BodyToattract.position;
            float distance = direction.magnitude;

            GforveM = (0.0000000000667408f * TheDestination.GetComponent<Rigidbody>().mass * BodyToattract.mass) / (Mathf.Pow(distance, 2));
            GforceV = direction.normalized * GforveM;
            BodyToattract.AddForce(GforceV);
            Debug.Log("goingggS");
       // }
 
    }
    void Update()
    {
        theForce = forceStatus.getForce();
        if ((TheDestination.transform.position - this.transform.position).sqrMagnitude < 24)
        {
            if (Input.GetKeyDown(KeyCode.E) && theForce >= forceReduction)//if grab
            {
                //GetComponent<Rigidbody>().isKinematic = false;
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
                //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                //  GetComponent<Rigidbody>().useGravity = false;
                //GetComponent<Rigidbody>().detectCollisions = true;
                //  GetComponent<Rigidbody>().isKinematic = false;
                // this.transform.position = TheDestination.position;
                attract();
                this.transform.parent = GameObject.Find("ForcePoint").transform;
                forceStatus.AddForce(-forceReduction);

                //InvokeRepeating("Reposition1", 0.0f, 0.1f);
                //StartCoroutine(Reposition());
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
      //  else
       // {
            //GetComponent<Rigidbody>().isKinematic = true;
           // GetComponent<Rigidbody>().velocity = Vector3.zero;
            //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
          //  GetComponent<Rigidbody>().useGravity = true;
         //   GetComponent<Rigidbody>().detectCollisions = true;
          //  GetComponent<Rigidbody>().isKinematic = true;
        //    this.transform.parent = null;
      //  }
        if (Input.GetKeyUp(KeyCode.E))// release it
        {
            //CancelInvoke("Reposition1");
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().detectCollisions = true;
            //GetComponent<Rigidbody>().isKinematic = true;
            this.transform.parent = null;
            //GetComponent<Rigidbody>().useGravity = true;
        }
        /*
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.position = TheDestination.position;
        }
        */

    }
    Vector3 seek(Transform goTo, Transform goFrom)
    {
        return Vector3.ClampMagnitude(Vector3.ClampMagnitude(goTo.position - goFrom.position, 10f) - Vel, 1000f);
    }
    void NowMoveYaDumbCube()
    {
        Acc = Vector3.zero;
        Acc = Acc + seek(TheDestination, this.transform);
        Vel = Vector3.ClampMagnitude(Vel + Acc * Time.fixedDeltaTime, 1000f);
        this.transform.position = this.transform.position + Vel * Time.fixedDeltaTime;
    }
    public void Reposition1()
    {
        while (this.transform.position != TheDestination.position)
        {
            this.transform.position = TheDestination.position;
        }
    }
    IEnumerator Reposition()
    {
        this.transform.position = TheDestination.position;
        yield return null;
    }
}
