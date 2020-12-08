using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 


namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        int temp = 1;
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject InstructPanel;

        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            if(!PhotonNetwork.IsConnected){
                progressLabel.SetActive(false);
                levelPicker.SetActive(false);
                InstructPanel.SetActive(false);
                controlPanel.SetActive(true);
                Debug.Log("Not Connected");
            }else{
                controlPanel.SetActive(false);
                progressLabel.SetActive(false);
                InstructPanel.SetActive(false);
                levelPicker.SetActive(true);
                temp = PhotonNetwork.PlayerList.Length;
                backToPicker();
                Debug.Log("already connected");
            }
        }
        void Update(){
            if(temp != PhotonNetwork.PlayerList.Length){
                temp = PhotonNetwork.PlayerList.Length;
                backToPicker();
            }
        }

        #endregion

        #region Public Methods
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject progressLabel;
        [SerializeField] private GameObject levelPicker;
        [SerializeField] private Text playersJoined;

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void MakeOwnRoom(){
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        public void ViewInstruct(){
            progressLabel.SetActive(false);
            levelPicker.SetActive(false);
            controlPanel.SetActive(false);
            InstructPanel.SetActive(true);
        }
        public void LeaveInstruct(){
            progressLabel.SetActive(false);
            levelPicker.SetActive(false);
            InstructPanel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
           if (isConnecting)
            {
             // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {   
            levelPicker.SetActive(false);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            PlayerPrefs.SetInt("Races", 1);
            controlPanel.SetActive(false);
            progressLabel.SetActive(false);
            levelPicker.SetActive(true);
            temp = PhotonNetwork.PlayerList.Length;
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
                playersJoined.text += "\n " + PhotonNetwork.PlayerList[i].NickName;
                PlayerPrefs.SetFloat(PhotonNetwork.PlayerList[i].NickName, 0f);
            }
        }
        public void backToPicker(){
            PlayerPrefs.SetInt("Races", 1);
            playersJoined.text = "Players Joined: \n";
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
                playersJoined.text += "\n " + PhotonNetwork.PlayerList[i].NickName;
                PlayerPrefs.SetFloat(PhotonNetwork.PlayerList[i].NickName, 0f);
            }
        }
        public void StartLevelRun(string level){
            if(PhotonNetwork.IsMasterClient){
                GameManager.loadOnce = false;
                PhotonNetwork.LoadLevel("Room for " + level);
            }
        }
        public void LeaveRoom()
        {
            playersJoined.text = "Players Joined: \n";
            PhotonNetwork.Disconnect();
        }


        #endregion


        #endregion


    }
}