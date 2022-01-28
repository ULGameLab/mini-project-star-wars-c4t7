using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Icon Refs")]
    public GameObject Sprint_Icon;
    public GameObject Crouch_Icon;
    float RepulsorValue;
    public GameObject Repulsor_Ready_Icon;
    public GameObject Repulsor_Unready_Icon;

    [Header("Slider Refs")]
    public Slider HealthBar;
    public Slider RepulsorBar;

    [Header("Script Refs")]
    public PlayMove MoveRef;
    public PlayInput InputRef;
    public PlayerHealth HealthRef;

    //private static UIManager instance;
    //public static UIManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        RepulsorValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Sprint and Crouch Icons
        if (Input.GetKey(KeyCode.C))
        {
            Crouch_Icon.SetActive(true);
            Sprint_Icon.SetActive(false);
        }
        else
        {
            if (MoveRef.GetOnGround() && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C))
            {
                Crouch_Icon.SetActive(false);
                Sprint_Icon.SetActive(true);
            }
            else
            {
                Crouch_Icon.SetActive(false);
                Sprint_Icon.SetActive(false);
            } 
                
        }

        // Health Bar
        HealthBar.value = HealthRef.GetHealthPercentage();

        //Repulsor Icon
        if (InputRef.GetRepulsorReady() )
        {
            Repulsor_Ready_Icon.SetActive(true);
            Repulsor_Unready_Icon.SetActive(false);
        }
        else
        {
            Repulsor_Ready_Icon.SetActive(false);
            Repulsor_Unready_Icon.SetActive(true);
        }

        // Repulsor Bar
        if (InputRef.GetRepulsorReady() && Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Repulsor());
        }
        RepulsorBar.value = RepulsorValue;
        
    }
    
    IEnumerator Repulsor()
    {
        //RepulsorReady = false;
        //yield return new WaitForSeconds(.20f);
        RepulsorValue = 0;
        float temp = InputRef.GetWait();

        //yield return new WaitForSeconds(.20f);
        while (RepulsorValue != 1)
        {
            RepulsorValue += .01f;
            yield return new WaitForSeconds(temp);
        }
        
    }
    




}
