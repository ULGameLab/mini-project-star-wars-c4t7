using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorController : MonoBehaviour
{
    [SerializeField] private Transform UserCamera;

    public float ReflectMagnifier;
    public float Force;

    public ForceLevelStatus theStatus;
    private int forceReduction = 20;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            
            other.attachedRigidbody.velocity = Vector3.Reflect(other.attachedRigidbody.velocity, UserCamera.transform.forward) * ReflectMagnifier;
            
        }
        else
        {
            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.AddForce(UserCamera.transform.forward * Force, ForceMode.Impulse);
            }
        }
    }

}
