using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceLevelStatus : MonoBehaviour
{
    public Slider forceSlider;
    public Slider overChargedSlider;
    public int maxForce = 100;
    public int overCharge = 100;
    public int currentForce;

    public GameObject forceRecharge;

    // Start is called before the fir[st frame update
    void Start()
    {
        currentForce = 150;
        InvokeRepeating("reduceOverCharged", 0.0f, 1.0f);
    }
    public void Update()
    {
        if (currentForce <= maxForce)
        {
            forceSlider.value = currentForce;
        }
        if (currentForce > maxForce)
        {
            forceSlider.value = 100;
        }
        overChargedSlider.value = currentForce - maxForce;
        
        
       
    }
    public int getForce()
    {
        return currentForce;
    }
    
    public void AddForce(int x)
    {
        if (currentForce < 200)
            currentForce += x;
    }
    public void reduceOverCharged()
    {
        if (currentForce - maxForce > 0)
        {
            
            currentForce += -1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectables")
        {
            other.gameObject.SetActive(false);
            currentForce = 200;
            StartCoroutine(respawn(5,other));
        }
        if (other.gameObject.tag == "badCollectables")
        {
            other.gameObject.SetActive(false);
            StartCoroutine(respawn(5,other));
            if (currentForce > 50)
                currentForce += -50;
            if (currentForce < 50)
                currentForce = 0;
        }
        
    }
    IEnumerator respawn(int secs,Collider other)
    {
        yield return new WaitForSeconds(secs);
        other.gameObject.SetActive(true);
    }

}
