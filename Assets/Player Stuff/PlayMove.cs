    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f; //12f;
    public float gravity = -9.81f;
    public Vector3 velocity;
    public float jumpHeight = 3f;

    public Transform GroundCheck;
    public float dropDist = 0.4f;
    public LayerMask groundMask;

    public Transform body;
    bool onGround;
    bool isCrouched = false;

    // Start is called before the first frame update
    void Start()
    {

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

        // Sprint: 
        if (onGround && isCrouched == false){
            if (Input.GetKey(KeyCode.LeftShift)) {
                speed = 12f;
            } else {
                speed = 8f;
            }
        }
        //Crouch:
        if (Input.GetKeyDown(KeyCode.C)) {
            speed = 3f;
            body.localScale = new Vector3(1f,.50f,1f);
            isCrouched = true;
        } if (Input.GetKeyUp(KeyCode.C)) {
            speed = 8f;
            body.localScale = new Vector3(1f,1f,1f);
            isCrouched = false;
            controller.Move(Vector3.up * .2f);
        }

        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement.normalized * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && onGround) { velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool GetOnGround()
    {
        return onGround;
    }

}
