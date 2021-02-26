using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text logText;
        [SerializeField] private InputField nickTextField;

        [SerializeField] private PlayerPreferences playerPreferences;
        
        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
            
            nickTextField.text = playerPreferences.nickName;
        }

        public override void OnConnectedToMaster()
        {
            Log("Connected to master");
        }

        public void CreateRoom()
        {
            PhotonNetwork.NickName = playerPreferences.nickName;
            PhotonNetwork.CreateRoom(null);
        }

        public void JoinRoom()
        {
            PhotonNetwork.NickName = playerPreferences.nickName;
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Log("Joined to room");
        
            PhotonNetwork.LoadLevel("GameScene");
        }

        public void ChangeNick(string nick)
        {
            playerPreferences.nickName = nick;
        }

        public void Log(string message)
        {
            Debug.Log(message);
            logText.text += message + "\n";
        }
    }
}
