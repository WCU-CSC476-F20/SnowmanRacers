using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" && !Goal.goalMet){
            //Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player" && !Goal.goalMet){
            Goal.goalMet = true;
        }    
    }
}
