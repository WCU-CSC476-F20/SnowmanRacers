using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 2f;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
