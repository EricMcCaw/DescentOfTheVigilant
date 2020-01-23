using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSp;
    protected Rigidbody Rb;
    public bool CanJump;
    float normalY;
    // Start is called before the first frame update
    void Start()
    {
        CanJump = true;
        Rb = transform.GetComponent<Rigidbody>();
        normalY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Jump();
    }

    public void Jump()
    {
        if (Input.GetAxis("Jump") != 0 && CanJump)
        {
            Rb.AddForce(new Vector3(0, 1500, 0));
            CanJump = false;

        }
    }
    public void GetInputs(float x = 0, float z = 0)
    {
        Vector3 dz;

        if (x == 0 && z == 0)
        {
            dz = Input.GetAxis("Horizontal") * transform.forward;
        }
        else
        {
            dz = x * transform.forward;
        }
        Vector3 inputs = dz;

        //moving using rb still checks collisions without feeling like a slip n slide 
        if (!CanJump)
        {
            Vector3 movement = inputs.normalized * movementSp / 2 * Time.deltaTime;
            Rb.AddForce(movement);
        }
        else
        {
            float modifier = 1f;
            Vector3 movement;
            if (Input.GetAxis("Sprint") > 0)
            {
                modifier = 2f;
            }
            if(Input.GetAxis("Crouch") > 0)
            {
                modifier = 0.5f;
                transform.localScale = new Vector3(transform.localScale.x, normalY/2, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, normalY, transform.localScale.z);
            }
            movement = inputs.normalized * (movementSp * modifier)* Time.deltaTime;
            Rb.AddForce(movement);
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            CanJump = true;
        }
      
    }
}
