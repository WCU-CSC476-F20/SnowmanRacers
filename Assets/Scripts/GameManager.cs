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
        public Text playersLeft;
        public GameObject countdown;
        public GameObject roundOver;
        public GameObject timeZone;
        public GameObject leavers;
        public static float timer = 0;
        public int allPlayers = 1;
        public float tempTime;
        public float timeLeft = 30;
        public static string[] names = new string[4];
        public static float[] times = new float[4];
        public static int place;
        public bool someoneFinished = false;
        public bool finalFinish = false;
        public bool finalOnce = false;

        /// Called when the local player left the room. We need to load the launcher scene.
        public void Start(){
            countdown.SetActive(false);
            roundOver.SetActive(false);
            leavers.SetActive(false);
            timeZone.SetActive(true);
            Time.timeScale = 1f;
            timer = 0;
            tempTime = timer + 30f;
            place = 0;
            loadOnce = false;
            if(names != null){
                Array.Clear(names, 0, names.Length);
            }
            PhotonNetwork.Instantiate("Snowman", new Vector3(0, 1, -8), Quaternion.identity, 0);
            allPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
            PlayerPrefs.SetInt("Races", PlayerPrefs.GetInt("Races") + 1);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        public void Update(){
            timer += Time.deltaTime;
            uitTimer.text = timer.ToString("F2");


            if(allPlayers < GameObject.FindGameObjectsWithTag("Player").Length){
                allPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
                //Debug.Log("allPlayers less than objects found");
            }

            if(allPlayers > GameObject.FindGameObjectsWithTag("Player").Length && GameObject.FindGameObjectsWithTag("Player").Length != 0){
                someoneFinished = true;
                //Debug.Log("Goal.goalMet = " + Goal.goalMet);
                countdown.SetActive(true);
                timeLeft = tempTime - timer;
                uitTimerLeft.text = timeLeft.ToString("F2");
                //Debug.Log("allPlayers greater than objects found");
            }else{
                tempTime = timer + 30f;
            }
            if(timeLeft <= 0f && !loadOnce){
                countdown.SetActive(false);
                timeZone.SetActive(false);
                roundOver.SetActive(true);
                loadOnce = true;
                for(int i = 0; i < names.Length; i++){
                    if(names[i] != null){
                        if(PlayerPrefs.GetInt("Races") != 4){
                            theLeaderboard.text += (i+1) + ". " + names[i] + "\n";
                            theTimes.text += times[i].ToString("F2") + "\n";
                        }
                        PlayerPrefs.SetFloat(names[i], PlayerPrefs.GetFloat(names[i]) + times[i]);
                    }
                }
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject pTemp in gos){
                    PhotonView tempView = pTemp.GetPhotonView();
                    if(PlayerPrefs.GetInt("Races") != 4){
                        theLeaderboard.text += tempView.Owner.NickName + "\n";
                        theTimes.text += "DNF\n";
                    }
                    PlayerPrefs.SetFloat(tempView.Owner.NickName, PlayerPrefs.GetFloat(tempView.Owner.NickName) + times[0] + 60f);
                    Destroy(pTemp);
                }
            }

            
            //Debug.Log("Player Objects: " + GameObject.FindGameObjectsWithTag("Player").Length);
            if(GameObject.FindGameObjectsWithTag("Player").Length == 0 && !loadOnce){
                countdown.SetActive(false);
                timeZone.SetActive(false); 
                roundOver.SetActive(true);
                loadOnce = true;
                for(int i = 0; i < names.Length; i++){
                    if(names[i] != null){
                        if(PlayerPrefs.GetInt("Races") != 4){
                            theLeaderboard.text += (i+1) + ". " + names[i] + "\n";
                            theTimes.text += times[i].ToString("F2") + "\n";
                        }
                        PlayerPrefs.SetFloat(names[i], PlayerPrefs.GetFloat(names[i]) + times[i]);  
                    } 
                }
                Debug.Log("Player Prefs Value: " + PlayerPrefs.GetInt("Races"));
            }
            if(PlayerPrefs.GetInt("Races") == 4){
                if(GameObject.FindGameObjectsWithTag("Player").Length == 0 && !finalFinish){
                    finalFinish = true;
                    for(int y = 0; y < names.Length; y++){
                        times[y] = PlayerPrefs.GetFloat(names[y]);
                    }
                    for(int i = 0; i < names.Length; i++){
                        for(int z = 0; z < names.Length; z++){
                            if(times[z] > times[i]){
                                float temp = times[i];
                                string temp2 = names[i];
                                times[i] = times[z];
                                times[z] = temp;
                                names[i] = names[z];
                                names[z] = temp2;
                            }
                        }
                    }
                }
                if(finalFinish && !finalOnce){
                    finalOnce = true;
                    int g = 1;
                    for(int b = 0; b < names.Length; b++){
                        if(times[b] > 1){
                            theLeaderboard.text += (g) + ". " + names[b] + "\n";
                            theTimes.text += times[b].ToString("F2") + "\n";
                            g++;
                        }
                    }
                }
            }
        
        }
        public void GoToLobby(){
            PlayerPrefs.SetInt("Races", 0);
            if(PhotonNetwork.IsMasterClient){
                Goal.goalMet = false;
                theLeaderboard.text = "";
                theTimes.text = "";
                PhotonNetwork.CurrentRoom.IsOpen = true;
                PhotonNetwork.LoadLevel("Launcher");
            }
        }
        public void HideLeavers(){
            leavers.SetActive(false);
            playersLeft.text = "";
        }
        public void GoNextLevel(){
            string curScene = SceneManager.GetActiveScene().name;
            
            if(PhotonNetwork.IsMasterClient){
                switch(curScene){
                case "Room for 1":
                    PhotonNetwork.LoadLevel("Room for 2");
                    break;
                case "Room for 2":
                    PhotonNetwork.LoadLevel("Room for 3");
                    break;
                case "Room for 3":
                    PhotonNetwork.LoadLevel("Room for 4");
                    break;    
                case "Room for 4":
                    PhotonNetwork.LoadLevel("Launcher");
                    break;
                } 
            }
                       
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        leavers.SetActive(true);
        if(!someoneFinished){
            allPlayers--;
        }
        playersLeft.text += other.NickName + " has left the room.";
        Invoke("HideLeavers", 5f);
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
        public static void makePlayer(){
            PhotonNetwork.Instantiate("Snowman", new Vector3(0, 1, -8), Quaternion.identity, 0);
        }

}
