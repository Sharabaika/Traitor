using System;
using System.Collections.Generic;
using Characters;
using Logics;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MultiPlayer
{
    public class PlayerOverviewPannel : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerCharacter playerPrefab;

        private GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        private void Start()
        {
            var character = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }

        public void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer.NickName + " joined the room");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log(otherPlayer.NickName + " left the room");
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}