using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
so i started this one by trying to addapt the controller from PA2, but that kept running into some missing assembly errors.
i had to nuke that whole version of the project just to get that to go away, luckily i'm starting with the player controller coversion and ran into this very early.
this honestly came out rather well despite the headache of relearning character controllers with rigidbodies

use case for this script [showing the object heirarchy this is applied to
  -EmptyObject     (place this script and RigidBody on this)
      -BodyModel   (place mesh collider here, can be any model you want, potential for animated body as well, but i haven't tested that)
                   (also place the player physics material with all zeros onto all model components, to handle sticking to walls)
      -Camera      (placed to line up with the 'head' of the model you chose to use for the player)
          -EmptyObject (this holds the gun. discussed in further detail in the gun script)
      -EmptyObject (placed towards the bottom of the BodyModel's body,a s this is the reference location for checking if you're on the ground)
*/
//finding this(https://answers.unity.com/questions/634397/crouching-weapon-hidden-solution-weapon-gets-squas.html)i realized that my nieve crouching isin't that good, but i don't feel its
//that bad of a problem to fix in the demo the problem lines are ~120 with full player scaling, and then trying to unscale the camera based off of that, but looking up and down
//still warps the gun regardless. it sucks, but that's just how it is, and i don't feel like breaking the cam and gunholder into their own hierarchy. maybe i'll fix it with a proper crouch animation if i have time.

public class RPCcopy : MonoBehaviour
{
    //Movement
    [HideInInspector]//public but hidden incase i need to reference this at any other point in another script
    public Vector3 MovDir;

    [Header("Player's transforms")]
    [Tooltip("transform of the empty parent object")]
    public Transform PlyrT;
    [Tooltip("transform of the model")]
    public Transform BodyT;
    Rigidbody PlyrRb;

    [Header("Movement Sliders")]
    [Tooltip("this is for the walking speed. Recomended:8")]
    [Range(.01f, 10)]
    public float spd;
    [Tooltip("this is for the crouching speed multiplyer. Recomended:.6")]
    [Range(.5f, .8f)]
    public float Cmult;
    [Tooltip("this is for the sprinting speed multiplyer. Recomended:2.2")]
    [Range(1, 3)]
    public float Smult;
    [Tooltip("this is for force of jumping. Recomended:7.5")]
    [Range(.3f, 12)]
    public float jmpF;
    [Tooltip("this is Max walking speed, adjusted at run time for sprinting and crouching. Recomended:8")]
    [Range(.01f, 10)]
    public float Mspd;
    [Tooltip("this is the drag that the player will be affected by. Recomended:4")]
    [Range(.01f, 12)]
    public float Drg;
    [Tooltip("this is the extra gravity when jumping. Recomended:4")]
    [Range(.01f, 8)]
    public float Grav;

    float crouch;
    float sprint;
    float Hrzn;
    float Vrti;

    [Header("Camera")]
    [Tooltip("this is the reference to the Camera")]
    public Camera cam;
    [Header("Camera sliders")]
    [Tooltip("this is the sensitivity of the camera. Recomended:2")]
    [Range(.01f, 10)]
    public float sens;

    float MouseX;
    float MouseY;
    float RotX;
    float RotY;

    [Header("\nGround detection")]
    [HideInInspector]//have a funny feeling that i might want to reference this later on, so making it public just in case...
    public bool isGrnd;
    public Transform GrndCheck;
    [Tooltip("place every layer in here that the player can jump off of")]
    public LayerMask Jumpable;

    //Slope Detection and handling
    bool isSlope;
    RaycastHit SlopeH;//realized that S hit wasen't the best name...
    Vector3 MovSlpDir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//lock the mouse
        PlyrRb = GetComponent<Rigidbody>();//get the rigidbody
        PlyrRb.freezeRotation = true;//stop the rigidbody's rotation, as that would just kill the math here so easily if left to flop around

        PlyrRb.drag = Drg;//apply starting drag
    }

    private void FixedUpdate()
    {
        //Move the player based on slope or not slope standing position
        if (isSlope) { PlyrRb.AddForce(MovSlpDir * 6f, ForceMode.Acceleration); }
        else { PlyrRb.AddForce(MovDir * 6f, ForceMode.Acceleration); }//the constant is to counteract the drag so you can crouch walk as an acceleration }
        PlyrRb.velocity = Vector3.ClampMagnitude(PlyrRb.velocity, Mspd * sprint * crouch);//this ensures you don't just constantly accelerate as if you were in a particle accelerator
    }
    void Update()
    {
        //ground detection and jumping
        isGrnd = Physics.CheckSphere(GrndCheck.position, .3f, Jumpable);
        if (isGrnd && Input.GetKey(KeyCode.Space)) { PlyrRb.AddForce(PlyrT.up * jmpF * crouch, ForceMode.Impulse); }//jump scaled by crouching, scaling by sprint made you jump to the MOON
        if (!isGrnd)
        {
            PlyrRb.drag = Drg / 4;//reduce drag while in the air, as there is significantly less friction in the air as compared to on the ground
            PlyrRb.AddForce(Vector3.down * Grav, ForceMode.Force);//also apply a small amount of gravity as reducing the drag is not enough for the jump
        }
        else { PlyrRb.drag = Drg; }//return the drag back to normal when you land on the ground

        //slope detection. this sets isSlope to true only if the player is standing on a surface with a non up normal to it. had to be nested instead of an AND because of the out hit.
        if (Physics.Raycast(GrndCheck.position, Vector3.down, out SlopeH, .3f))
        {
            if (SlopeH.normal != Vector3.up) { isSlope = true; }
            else { isSlope = false; }
        }
        else { isSlope = false; }

        //sprinting and crouching
        if (Input.GetKey(KeyCode.LeftControl)) { sprint = Smult; }
        else { sprint = 1f; }
        if (Input.GetKey(KeyCode.LeftShift)) { crouch = Cmult; PlyrT.localScale = new Vector3(1f, .65f, 1f); cam.transform.localScale = new Vector3(1f, (1 / .65f), 1f); }
        else { crouch = 1f; PlyrT.localScale = new Vector3(1f, 1f, 1f); cam.transform.localScale = new Vector3(1f, 1f, 1f); }

        //Movement Inputs
        Vrti = Input.GetAxisRaw("Vertical");
        Hrzn = Input.GetAxisRaw("Horizontal");

        //move acceleration vectors
        MovDir = (PlyrT.forward * Vrti + PlyrT.right * Hrzn).normalized * spd * sprint * crouch;
        MovSlpDir = Vector3.ProjectOnPlane(MovDir, SlopeH.normal);

        //Looking Inputs
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");

        //calculate the rotations for camera movement
        RotY += MouseX * sens;
        RotX -= MouseY * sens;
        RotX = Mathf.Clamp(RotX, -90f, 90f);

        //rotate the camera based on the mouse, as well as the model to keep shadows consistant, and the rigidbody to have controlls turn with the camera
        cam.transform.localRotation = Quaternion.Euler(RotX, 0f/*RotY*/, 0f);//cam rotates 1:1 to the mouse based on sensitivity
        BodyT.rotation = Quaternion.Euler(RotX * .35f, RotY, 0f);//body rotates 1:1 to mouse based on sensitivity, except for pitch, which only rotates a little (useful if i want a hat on the character)
        PlyrT.rotation = Quaternion.Euler(0f, RotY, 0f);//only rotate horizontaly for planar controls to work
    }
}