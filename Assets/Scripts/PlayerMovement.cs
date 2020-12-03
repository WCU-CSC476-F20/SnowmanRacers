using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 1f;
    public float jumpPower;
    public float jumpHeight = 3f;
    public bool isJumping = false; 
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
        if(velocity.x >= 40f){
                velocity.x = 40f;
         }else if(velocity.x <= -40f){
                velocity.x = -40f;
        }
        rb.velocity = velocity;
        }
           
}
