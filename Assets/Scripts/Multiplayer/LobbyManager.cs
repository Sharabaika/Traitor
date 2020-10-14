using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]private Text logText;
    
        void Start()
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1,1000);

            Log("New player nick is set to" + PhotonNetwork.NickName);
        
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        
        }

        public override void OnConnectedToMaster()
        {
            Log("Connected to master");
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(null);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Log("Joined to room");
        
            PhotonNetwork.LoadLevel("GameScene");
        }

        public void Log(string message)
        {
            Debug.Log(message);
            logText.text += message + "\n";
        }
    }
}
