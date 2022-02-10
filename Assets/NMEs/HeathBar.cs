using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
use case for this script
[enemies]
EmptyObject   (this is the enemy holder that is used in csripts that move the enemy, has a rigidbody on it.layer and tag set to enemy. script goes here)
  model       (this is the model of the enemy, has a collider on it. layer and tag set to enemy)
  EmptyObject (this holds the canvas so the healthbar stays above the enemy at all times)
    canvas    (this is the what will hold the healthbar itself.layer and tag left unset)
      slider  (this a UI object that goes onto the canvas and will be used as the healthbar.layer and tag left unset)
      so the slider has a hierarchy like this
  slider                  [width/height 160/20, scale is .02 on x/y/z]
    background            [left and right are 0, this is the missing health color]
    fill area             [left and right are 5]
      fill                [left and right are -5, this is the color of having health]
    handle slides area    [left and right are 10]
      handle              [remove this, don't need it]

[objects with health]
model         (this is the model of the object, has a collider and rigidbody on it)
  EmptyObject (this holds the canvas so the healthbar stays above the object at all times)
    canvas    (this is the what will hold the healthbar itself)
      slider  (this a UI object that goes onto the canvas and will be used as the healthbar)

to have the object you gavea  healthbar take damage, just get a reference to this script form the object, and call the TakeDamage(DMG) method, with DMG=the damage you want it to take.
-DMG counts as healing, and casehandling for overhealing is handled.
*/
public class HeathBar : MonoBehaviour
{
    //maybe come back to this at some point and change it so taht when you take damage there is a bar that goes down to the damage you took, but your actual health only slowly scrolls to that value.
    //so if you get fataly hit and heal before it hits zero, you're still alive. that could be interesting to get back to
    [Header("Health")]
    [Tooltip("this is the max health it can have")]
    public float MaxH;
    [Tooltip("this is its current health")]
    public float CurH;

    [Header("Options")]
    [Tooltip("this is checked if the object is to remain in the scene once its health is depleted")]
    public bool persisBody;
    [Tooltip("this is checked if the healthbar is allways visible, even at full health")]
    public bool isVisbl;

    public bool LoseOnDeath;
    public bool WinOnDeath;

    [Header("References")]
    [Tooltip("this is a reference to the object that you want to have a healthbar")]
    public GameObject SelfObj;
    [Tooltip("this is a reference to the slider UI object that is the healthbar")]
    public Slider HBSl;
    [Tooltip("this is a reference to the canvas object that holds the healthbar")]
    public GameObject CanObj;
    [Tooltip("this is a reference to what the healthbar should face towards")]
    public Transform FaceT;
    [Tooltip("this is a reference to the empty object that holds the canvas")]
    public Transform HldrT;
    
    bool HBoff = true;//the health bar is off untill the object takes damage

    void Start()
    {
        CurH = Mathf.Max(Mathf.Min(CurH, MaxH), 0);//this ensures that CurH is betwen 0 and MaxH
        CanObj.SetActive(false);
        InvokeRepeating("HealthRegen", 0.0f, 0.25f);
    }

    void Update()
    {
        if (!isVisbl)
        {
            HldrT.rotation = Quaternion.Euler(0, 0, 0);//this keeps the healthbar above the object at all times
            if (CurH < MaxH && HBoff)
            {
                CanObj.SetActive(true);
                HBoff = false;
            }
            if (!HBoff)
            {
                CanObj.transform.rotation = Quaternion.LookRotation(FaceT.position - CanObj.transform.position);
            }
        } else { CanObj.SetActive(true); }
    }

    public void TakeDamage(float dmg)
    {
        CurH -= dmg;

        if (CurH > MaxH) { CurH = MaxH; }//healing will inevitibly come up, and healing is just -damage. don't want to allow over healing (for now, may be something i try out later)
        if (CurH < 0) { CurH = 0; }      //this ensures that if you damage an enemy past the amount of health it has left, the healthbar dosen't fill backwards
        HBSl.value = CurH / MaxH;            //this moves the slider based off a percentage of the remaining health out of the max health

        if (CurH <= 0)
        {
            if (persisBody)
            {
                if (LoseOnDeath)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Lose");
                }
                if (WinOnDeath)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Win");
                }
                Destroy(SelfObj.GetComponent<HeathBar>());
                Destroy(CanObj);
            }
            else
            {
                /*for (float i = 0; i < 1; i = i + .001f)
                {
                    Color temp = SelfObj.GetComponent<Renderer>().material.color;
                    temp.a = 1f - i;
                }//wanted to do a fade out, but that just dosen't work for some reason. maybe come back to this way later.*/
                if (LoseOnDeath)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Lose");
                }
                if (WinOnDeath)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Win");
                }
                Destroy(SelfObj);
                
            }
        }
    }

    public void HealthRegen()
    {
        TakeDamage(-1);
    }
}