using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInputForForcePoint : MonoBehaviour {

    public float mSens = 600f;
    float xRotation = 0f;
    public Transform playerBody;
    [SerializeField] private Transform UserCamera;

    // Repulsor
    public GameObject Cone;
    //Collider RepulsorCollider = Cone.GetComponent<Collider>();
    //public float ReflectMagnifier;
    //public float Force;
    //float RepulseTimer = 1;
    bool RepulsorReady;
    public float Wait;

    // Start is called before the first frame update
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;

        RepulsorReady = true;
    }

    // Update is called once per frame
    void Update(){
        float mouseX = Input.GetAxis("Mouse X") * mSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mSens * Time.deltaTime;
        if (mouseY>=0) {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Repulsor
        // if (RepulseTimer >= 1 && Input.GetButtonDown("Fire2"))
        /*
        if (RepulsorReady && Input.GetButtonDown("Fire2"))
        { 
            StartCoroutine(Repulsor()); 
            StartCoroutine(RepulsorReadying());
        }
        */
        /*
        Physics.Raycast(UserCamera.position, UserCamera.forward.normalized);
        Debug.DrawRay(transform.position, UserCamera.forward.normalized, Color.red);
        */

    }

    private void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(UserCamera.position, UserCamera.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, UserCamera.forward * hit.distance, Color.red);
            
        }
        else
        {
            Debug.DrawRay(transform.position, UserCamera.forward * 1000, Color.blue);
            
        }

    }

    IEnumerator Repulsor()
    {
        RepulsorReady = false;
        Cone.SetActive(true);
        //RepulseTimer = 0f;
        yield return new WaitForSeconds(.20f);
        Cone.SetActive(false);
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet") 
        { 
            other.attachedRigidbody.velocity = Vector3.Reflect(other.attachedRigidbody.velocity, transform.forward) * ReflectMagnifier;
        }
        else
        {
            if (other.attachedRigidbody != null)
            { 
                other.attachedRigidbody.AddForce(transform.forward * Force, ForceMode.Impulse); 
            }
        }
    }
    */

    IEnumerator RepulsorReadying()
    {
        yield return new WaitForSeconds(Wait);
        RepulsorReady = true;

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
        return RepulsorReady;
    }
    
    public float GetWait()
    {
        return Wait/100;
    }

}
