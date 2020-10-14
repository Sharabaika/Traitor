using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;
using Characters;
using Misc;

namespace Logics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerCharacter playerPrefab;
        [SerializeField] private Transform homePoint;
        [SerializeField] private UI userInterface;
        
        private List<PlayerCharacter> _characters;
        private int _requiredPlayers = 2;

        private bool _canStartGame = false;
        public bool CanStartGame
        {
            get => _canStartGame;
            private set
            {
                _canStartGame = value;
                
                // TODO add events or smth
                userInterface.StartGameButton.SetActive(value && PhotonNetwork.IsMasterClient);
            }
        }

        private void Awake()
        {
            _characters = new List<PlayerCharacter>();
        }

        public void AddPlayer(Player player)
        {
            // TODO сейчас только у мастера есть список игроков - надо исправить
            photonView.RPC("CreatePlayerCharacter", RpcTarget.MasterClient, player);
        }

        [PunRPC]public void CreatePlayerCharacter(Player player)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            
            var character = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity)
                .GetComponent<PlayerCharacter>();
            character.photonView.TransferOwnership(player);
            _characters.Add(character);
            
            if (_characters.Count >= _requiredPlayers)
                CanStartGame = true;
        }
        
        public void StartGame()
        {
            if(!PhotonNetwork.IsMasterClient || !CanStartGame) return;

            // pick imposter
            var imposter = _characters[Random.Range(0, _characters.Count)];
            imposter.photonView.RPC("SignRoles", RpcTarget.All,1, true);
                
            // TODO pick roles
            

            // TODO spawn players
            var delta = 360 / _characters.Count;
            var angle = 0;
            foreach (var character in _characters)
            {
                var pos = homePoint.position + Quaternion.Euler(0, 0, angle) * Vector3.right;
                character.photonView.RPC("RelocateTo", character.photonView.Owner, pos);
                
                angle += delta;
            }
        }

    }
}