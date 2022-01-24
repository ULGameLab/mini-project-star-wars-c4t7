using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private bool jump;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private float x;
    private float z;

    // Update is called once per frame
    void Update() {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (!jump && Input.GetKeyDown(KeyCode.Space))
            jump = true;

        //transform.position += new  Vector3(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime);
    }

    private void FixedUpdate() {

        rb.MovePosition(transform.position + new Vector3(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime));

        if (jump) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
        Debug.Log(rb.velocity);
    }
}
