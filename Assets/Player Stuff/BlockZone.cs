using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockZone : MonoBehaviour
{
    [SerializeField] private Transform UserCamera;

    public float ReflectMagnifier;
    public float Force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, UserCamera.forward.normalized * 1000, Color.red);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            other.attachedRigidbody.velocity = Vector3.Reflect(other.attachedRigidbody.velocity, UserCamera.transform.forward) * ReflectMagnifier;
        }
        
    }

}
