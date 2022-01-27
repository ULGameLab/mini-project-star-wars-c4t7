using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
use case
EmptyObject    (this is the gun holdre mentioned on the RigidPlayerController script)
  GunModel     (this is whatever model you want for the gun)
    EmptyObject(this is the gun tip, for where the bullts are spawned)
*/

//unfortunetly, i need to shelve the grapplehook for this demo. its 11-30, all my guys on our discord are popping off with minecraft 1.18, and i want to get somethign done today so i'm
//pushing off the grapple toa  later date and going to focus on the grid for the rest of my time on this project.
public class OldGun : MonoBehaviour
{
    [Header("Transforms and bullet prefabs")]
    public GameObject BulletPre;
    public GameObject GrenadePre;
    public GameObject RocketPre;
    //public GameObject GrappleHookPre;//impliment this later, also make their prefabs
    GameObject SelectedPre;//this holds the prefab selected by the grid, slider for testing pre-rgid
    public Transform GunTipT;
    public Transform PlyrT;

    //Base Values
    //--the base values before scaled by grid
    //--defults preset for each bullet type.
    float FiredF;//this is the default force of the bullet being fired
    [HideInInspector]
    public float FiredM;//this is the default mass of the bullet beign fired
    float FiredLobBump;//this is a slight bump for the grenade and grapple 'bullets', as they are ment to be lobbed and will get a slight upwards force on their launch to facilitate this lob.
    [HideInInspector]
    public float FiredD;//this is the default damage of the bullet being fired

    //Grid Modifyers
    //--basic modifyers ( starts as a 1 for empty grid, as there are no multiplyers active)
    float GridMultF;//this is the force multiplyer form mods to change the bulelt's speed.
    [HideInInspector]
    public float GridMultS;//this reads the scale of the bullets being shot from the grid.
    [HideInInspector]
    public float GridMultD;//this is the damage multiplyer from the grid, mostly affected by the damage mods

    public int GridBurst = 1;//this causes multiple bullets to fire from the scatter mods that give the guns a burst fire mode
    bool BurstReady;//by suggestion of Nicolas Fisher on the meta course unity forums, i'm trying a bool controlled coroutine call.
    float BurstDelay;//time between burst by default, reduced slightly by firing speed. set to 0 for a shotgun effect

    GameObject Fired;//only up here and allways accesible because of the grapplehook
    //changeBullet listener;  //from original project and defunkt here

    [Range(0, 3)]
    public int BeingFired;

    void Start()
    {
        //temp test values for dev
        BeingFired = 0;//this is the default of bullet that will be moved to the grid reading once i get to taht

        //start values. they all start at 1 because they are multiplyers from an empty grid.
        GridMultF = 1;
        GridMultS = 1;
        GridMultD = 1;
        //GridBurst = 1;//remove it from public when making grid

        BurstReady = true;
        BurstDelay = .095f;

        SelectBulletAndReadGridValues();//this reads the grid at the start to initialize the gun with the proper grid values
        //listener = GetComponent<changeBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) { BeingFired = listener.CycleBullet(BeingFired); }
        //if(gridOpen()){retract the grapple hook, just repeat the retract code from the firing block} not used for now, but isa  good diea to keep for later when implimentign that
        SelectBulletAndReadGridValues();//might place this into a grid close if statment 
        if (Input.GetMouseButton(0))
        {
            FIRE();
        }
    }
    IEnumerator BurstFire(float sec)//this method is how you actualy fire bullets with a time delay based on burst and hold for repeated fire
    {
        for (int i = 0; i < GridBurst; i++)//this spawns multiple bullets. set time delay to zero is you want a shotgun effect.
        {
            BurstReady = false;

            Fired = Instantiate(SelectedPre);
            Fired.transform.position = GunTipT.position;
            Fired.transform.rotation = GunTipT.rotation;
            Fired.transform.localScale = Fired.transform.localScale * GridMultS;//this scales the bullets based off of grid
            Fired.GetComponent<Rigidbody>().mass = FiredM * Mathf.Pow(GridMultS, 3);//this changes mass based off of the cube scale, as scale is the length of the bullet, and volume is the cube of scale.

            Vector3 FinalF = (GunTipT.forward * FiredF * GridMultF) + (Vector3.up * FiredLobBump);

            Fired.GetComponent<Rigidbody>().velocity = PlyrT.GetComponent<Rigidbody>().velocity;
            Fired.GetComponent<Rigidbody>().AddForce(FinalF, ForceMode.Impulse);
            yield return new WaitForSeconds(sec);//this causes the bullets to spawn with a time delay, giving the burst fire effect
        }
        if (Input.GetMouseButton(0)) { yield return new WaitForSeconds(.35f); }//this pauses so that if you're holding the fire button, you repeatedly fire with a small delay.
        else { yield return new WaitForSeconds(.05f * ((GridMultF + 1.35f) / (GridMultF + .4f))); }//you get a much smaller delay between when you can fire if you are clicking repetedly. scalled by speed

        BurstReady = true;
    }

    public void SelectBulletAndReadGridValues()//this method reads the grid when it closed, and updates the defaults of the bullets being fired
    {
        /*if(gridClose()){
            select which bullet type is being fired

            calculate Grid Modifyers and save thme to the apropriate value
            GridMultF = read from grid;
            GridMultS = read from grid;
            GridMultD = read from grid;
            GridBurst = read from grid;

            might want to place the switch below this into the if block,
            or maybe move the grid close detection to the update around this whole method
        }*/
        if (Input.GetKey(KeyCode.BackQuote))//for now just have the grid update button set to `. change this to recognizing that the grid is closed
        {
            switch (BeingFired)
            {
                //Regular Bullets
                //--the default option. can be the best workhorse you've ever met if dressed up properly.
                case 0:
                    SelectedPre = BulletPre;
                    FiredF = 4f;
                    FiredM = .1f;
                    FiredLobBump = 0;
                    FiredD = 1;
                    break;
                //Grenade
                //--the spicy bullet. this packs not much of a punch on its own, but what its got to say after that might change your mind about it being weak.
                case 1:
                    SelectedPre = GrenadePre;
                    FiredF = 6f;
                    FiredM = .6f;
                    FiredLobBump = 1;
                    FiredD = .4f;
                    break;
                //Rockets
                //--when grenades just aren't enough. these mini rockets fly in a straight line towards their target but provide a smaller blast than grenades, but with jsut as big a bang.
                case 2:
                    SelectedPre = RocketPre;
                    FiredF = 3.6f;
                    FiredM = .45f;
                    FiredLobBump = 0;
                    FiredD = .25f;
                    break;
                //Grapple Hook
                //--for when you need to move something. this does almost no real damage, but makes up for its lack in fighting prowes in its utility of being able to let you move all kinds of things, or reach new areas.
                case 3:
                /*
                SelectedPre = GrappleHookPre;
                FiredF = 3.75f;
                FiredM = .25f;
                FiredLobBump = 1;
                FiredD = .15f;
                break;
                */
                default:
                    break;
            }
        }
        //have some way to detect when the grid menu is closed to choose what kind of bullet is being used
        //select which prefab to load off of a switch statment
    }

    public void FIRE()//this is a method to fire the bullet, whatever kind of bullet that's being fired
    {
        switch (BeingFired)
        {
            case 0://basic bullet
                if (BurstReady) { StartCoroutine(BurstFire(BurstDelay)); }//this fires the bullets with a small delay between shots for burst
                break;
            case 1://grenade
                if (BurstReady) { StartCoroutine(BurstFire(BurstDelay)); }//this fires the bullets with a small delay between shots for burst
                break;
            case 2://rocket
                if (BurstReady) { StartCoroutine(BurstFire(BurstDelay * 2.25f)); }//this fires the bullets with a larger delay between rockets in a burst
                break;
            case 3:
                /*i tried to get a toggle working, but that didn't work so i'm just going with a hold
                i also handled the grapple in a different manner is instead of repeatedly firing i want it to just fire and stay out if held
                then 'retract' when let go
                GridBurst = 1;//grapple is locked to 1 bullet per shot. no burst, but you can waste the space with the mod if you want to
                */
                break;
            default:
                break;
        }
    }
    /*public void GrappleHandler()
    {
        //maybe make this as Coroutine taht listens for the inputs???
        if (Input.GetMouseButton(0))
        {
            GridBurst = 1;//grapple is locked to 1 bullet per shot. no burst, but you can waste the space with the mod if you want to
                          //instantiate the grapple hook here
            Fired = Instantiate(SelectedPre);
            Fired.transform.position = GunTipT.position;
            Fired.transform.rotation = GunTipT.rotation;
            Fired.transform.localScale = Fired.transform.localScale * GridMultS;//this scales the bullets based off of grid
            Fired.GetComponent<Rigidbody>().mass = FiredM * Mathf.Pow(GridMultS, 3);//this changes mass based off of the cube scale, as scale is the length of the bullet, and volume is the cube of scale.

            Vector3 FinalF = (GunTipT.forward * FiredF * GridMultF) + (Vector3.up * FiredLobBump);

            Fired.GetComponent<Rigidbody>().velocity = PlyrT.GetComponent<Rigidbody>().velocity;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(Fired);
        }
    }*/
}
