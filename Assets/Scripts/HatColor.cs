using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HatColor : MonoBehaviour
{
    private Renderer myRend = null;
    private PhotonView myView = null;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
        myRend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        myRend.material.color = Color.green;

    }
}
