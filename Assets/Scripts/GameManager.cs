using System;
using System.Collections;

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{

        public static bool loadOnce = false;
        public Text uitTimer;
        public Text uitTimerLeft;
        public Text theLeaderboard;
        public Text theTimes;
        public GameObject countdown;
        public GameObject roundOver;
        public GameObject timeZone;
        public GameObject theGoal;
        public Collider theGoalCol;
        public static float timer = 0;
        public int allPlayers = 1;
        public float tempTime;
        public float timeLeft = 30;
        public static string[] names = new string[4];
        public static string[] times = new string[4];
        public static int place;

        /// Called when the local player left the room. We need to load the launcher scene.
        public void Start(){
            countdown.SetActive(false);
            roundOver.SetActive(false);
            timeZone.SetActive(true);
            Time.timeScale = 1f;
            theGoal = GameObject.FindGameObjectWithTag("Goal");
            theGoalCol = theGoal.GetComponent<Collider>();
            timer = 0;
            tempTime = timer + 30f;
            if(place == 0){
                place = 0;
            }
            if(names != null){
                Array.Clear(names, 0, names.Length);
            }
            PhotonNetwork.Instantiate("Snowman", new Vector3(0, 1, -8), Quaternion.identity, 0);
            allPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
            
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        public void Update(){
            timer += Time.deltaTime;
            uitTimer.text = timer.ToString("F2");
            
            GameObject[] dos = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject pTemp in dos){
                Collider tempCol = pTemp.GetComponent<Collider>();
                if(tempCol.bounds.Intersects(theGoalCol.bounds)){
                    Debug.Log("Someone Hit the Goal");
                    PhotonView tempView = pTemp.GetPhotonView();
                    theLeaderboard.text += tempView.Owner.NickName + "\n";
                    theTimes.text += timer.ToString("F2") + "\n";
                    Destroy(pTemp);
                }
                
            }

            if(allPlayers < GameObject.FindGameObjectsWithTag("Player").Length){
                allPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
                Debug.Log("allPlayers less than objects found");
            }

            if(allPlayers > GameObject.FindGameObjectsWithTag("Player").Length && GameObject.FindGameObjectsWithTag("Player").Length != 0){
                //Debug.Log("Goal.goalMet = " + Goal.goalMet);
                countdown.SetActive(true);
                timeLeft = tempTime - timer;
                uitTimerLeft.text = timeLeft.ToString("F2");
                Debug.Log("allPlayers greater than objects found");
            }else{
                tempTime = timer + 30f;
            }
            if(timeLeft <= 0f){
                loadOnce = true;
                for(int i = 0; i < names.Length; i++){
                    if(names[i] != null){
                        theLeaderboard.text += (i+1) + ". " + names[i] + "\n";
                        theTimes.text += times[i] + "\n";
                    }
                }
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject pTemp in gos){
                    PhotonView tempView = pTemp.GetPhotonView();
                    theLeaderboard.text += tempView.Owner.NickName + "\n";
                    theTimes.text += "DNF\n";
                    Destroy(pTemp);
                }
            }
            
            //Debug.Log("Player Objects: " + GameObject.FindGameObjectsWithTag("Player").Length);
            if(GameObject.FindGameObjectsWithTag("Player").Length == 0 && !loadOnce){
                roundOver.SetActive(true);
                loadOnce = true;
                countdown.SetActive(false);
                timeZone.SetActive(false); 
                for(int i = 0; i < names.Length; i++){
                    if(names[i] != null){
                        theLeaderboard.text += (i+1) + ". " + names[i] + "\n";
                        theTimes.text += times[i] + "\n";
                    }
                    
                }
            }
        }
        public void GoToLobby(){
            if(PhotonNetwork.IsMasterClient){
                Goal.goalMet = false;
                theLeaderboard.text = "";
                theTimes.text = "";
                PhotonNetwork.CurrentRoom.IsOpen = true;
                PhotonNetwork.LoadLevel("Launcher");
            }
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            
        }      


        public void LeaveRoom()
        {
            PhotonNetwork.Disconnect();
        }

}
