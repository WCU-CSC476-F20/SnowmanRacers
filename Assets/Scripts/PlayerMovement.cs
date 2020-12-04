using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 1f;
    public float jumpPower = 8f;
    public float jumpHeight = 3f;
    public float jumpCurrent = 0f;
    public bool isGrounded = false;
    public Vector3 jump = new Vector3 (0f, 2f, 0f);
    public Rigidbody rb;
    PhotonView myView;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.D)){
            velocity += transform.right * speed;
        }
        if(Input.GetKey(KeyCode.A)){
            velocity += -transform.right * speed;
        }
        if(Input.GetKey(KeyCode.W)){
            //transform.position = transform.position + new Vector3(0, 1f * jumpPower * Time.deltaTime, 0);
            //rb.AddForce(jump*jumpPower*Time.deltaTime, ForceMode.Impulse);
            //rb.AddForce(Vector3.up*jumpPower);
            //velocity += jumpPower*transform.up;
            //print(transform.up);
            isGrounded = false;
        }
        if(velocity.x >= 40f){
                velocity.x = 40f;
        }else if(velocity.x <= -40f){
                velocity.x = -40f;
        }
        rb.velocity = velocity;
        }
/*
        bool IsGrounded(){
            return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.x, col.bounds.min.y, col.bounds.min.y, col.bounds.center.z), col.radius *.8f, groundTags);

        }
        */
        void OnCollisionStay()
        {
            isGrounded = true;
        }
        void OnCollisionExit(){
             isGrounded = false;
       }
}
