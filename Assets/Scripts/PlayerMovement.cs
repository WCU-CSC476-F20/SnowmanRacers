using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 

public class PlayerMovement : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 1f;
    public float jumpPower = 3f;
    public float runningJump = 9f;
    public float maxSpeed = 5f;
    public bool isJumping = false; 
    public bool isColliding = false;
    public Rigidbody rb;
    [SerializeField] PhotonView myView;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myView = GetComponent<PhotonView>();
    }

    void OnCollisionStay(Collision other){
        if(other.gameObject.tag == "ground"){
            isJumping = false;
            velocity.y = 0f;
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Goal"){
            if(myView.IsMine){
                if(isColliding)return;
                isColliding = true;
                Debug.Log("Entered the goal");
                
                Debug.Log(GameManager.place);
                GameManager.names[GameManager.place] = myView.Owner.NickName;
                Debug.Log("Sent Name");
                GameManager.times[GameManager.place] = GameManager.timer.ToString("F2");
                Debug.Log("Sent Time");
                GameManager.place++;
                Debug.Log("GameManager.place = " + GameManager.place.ToString());
                PhotonNetwork.Destroy(myView);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(myView.IsMine){
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
                if(Input.GetKeyDown(KeyCode.W) && !isJumping){
                    isJumping = true;
                    velocity.y += jumpPower;   
                    rb.velocity = velocity;
                }else if(isJumping){
                    velocity.y += -.04f;
                    rb.velocity = velocity;
                }
            }
        
            if(Input.GetKey(KeyCode.W) && !isJumping){
                isJumping = true;
                velocity += transform.up*jumpPower;
                rb.velocity = velocity;
            }
            if(velocity.x >= maxSpeed){
                velocity.x = maxSpeed;
            }else if(velocity.x <= -maxSpeed){
                velocity.x = -maxSpeed;
            }
            if(velocity.y >= jumpPower){
                velocity.y = jumpPower;
            } 
        }

    }
    void FixedUpdate(){
        if(myView.IsMine){
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
                if(Input.GetKey(KeyCode.D)){
                    velocity.x += speed;
                    //if(isJumping){
                    //    velocity.y += -.04f;
                    //}
                    rb.velocity = velocity;
                }    
                if(Input.GetKey(KeyCode.A)){
                    velocity.x += -speed;
                    //if(isJumping){
                    //    velocity.y += -.04f;
                    //}
                    rb.velocity = velocity;
                }
            }else{
                if(velocity.x > 0){
                    velocity.x -= speed;
                    rb.velocity = velocity;
                }else if(velocity.x < 0){
                    velocity.x += speed;
                    rb.velocity = velocity;
                }    
            }    
        }        
    }   
}
