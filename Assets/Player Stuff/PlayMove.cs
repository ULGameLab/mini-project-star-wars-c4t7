    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    
    public CharacterController controller;
    public float speed = 8f; //12f;
    private float Ospeed;
    public float gravity = -9.81f;
    public Vector3 velocity;
    public float jumpHeight = 3f;

    public Transform GroundCheck;
    public float dropDist = 0.4f;
    public LayerMask groundMask;

    public float DashSpeed;
    public float DashTime;
    public float DashRecharge = 1f;
    float dashTime;

    public Transform body;
    bool onGround;
    bool isCrouched = false;
    bool DashReady;

    public ForceLevelStatus status;
    private int forceReduction = 5;
    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        Ospeed = speed;
        dashTime = DashTime/10f; /// 10f;
        DashReady = true;
        DashSpeed = 2 * Ospeed;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.CheckSphere(GroundCheck.position, dropDist, groundMask);
        if(onGround && velocity.y < 0)
        {
            velocity.y = -1; //To ensure velocity is not set to 0 a few seconds before ground contact is made with the actually player model.
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;
        /*
        // Sprint: 
        if (onGround && isCrouched == false){
            if (Input.GetKey(KeyCode.LeftShift)) {
                speed = Ospeed*1.5f;
            } else {
                speed = Ospeed; //8f;
            }
        } */
        //Crouch:
        /*
        if (Input.GetKeyDown(KeyCode.C)) {
            speed = Ospeed*0.375f;
            body.localScale = new Vector3(1f,.50f,1f);
            isCrouched = true;
        } if (Input.GetKeyUp(KeyCode.C)) {
            speed = Ospeed; //8f;
            body.localScale = new Vector3(1f,1f,1f);
            isCrouched = false; 
            controller.Move(Vector3.up * .2f);
        }
        */

        //Dash
        if (DashReady && Input.GetKey(KeyCode.LeftShift) && status.getForce() >= forceReduction)
        {
            status.AddForce(-forceReduction);
            StartCoroutine(Dash());
            StartCoroutine(DashReadying());
        }

        //if (/*onGround &&*/ DashReady == true)
        /*{
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
            else
            {
                speed = Ospeed; //8f;
            }
        } */

        //Executing movement
        //Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement /*movement.normalized*/ * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && onGround) { velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        DashReady = false;

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            if(status.getForce() > 100)
                controller.Move(movement * 1.2f * DashSpeed * Time.deltaTime);
            else
                controller.Move(movement * DashSpeed * Time.deltaTime);

            yield return null;
        }

    }

    IEnumerator DashReadying()
    {
        //DashReady = false;
        yield return new WaitForSeconds(DashRecharge);
        DashReady = true;
    }

    public bool GetDashReady()
    {
        return DashReady;
    }

    public float GetDashRecharge()
    {
        return DashRecharge / 100;
    }

    public bool GetOnGround()
    {
        return onGround;
    }

}
