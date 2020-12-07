using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FollowPlayer : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject myCamera;

    [Header("Set Dynamically")]
    public GameObject thePlayer;
    public Vector3 playerPos;


    void Awake(){
        Invoke("GetPlayer", 1);
    }
    void GetPlayer(){
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject pTemp in gos){
                    PhotonView tempView = pTemp.GetPhotonView();
                    if(tempView.IsMine){
                        thePlayer = pTemp;
                        break;
                    }
                }
        playerPos = thePlayer.transform.position;
    }
    void FixedUpdate(){
        if(playerPos != null){
            myCamera.transform.position = new Vector3(playerPos.x, 8.0f, -15.0f);
            GetPlayer();
        }else{
            Invoke("GetPlayer",1);
        }
    }
}
