using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;
using Characters;
using UnityEngine.Events;
using UserInterface;

namespace Logics
{
    public class GameManager : MonoBehaviourPunCallbacks
    { 
        [SerializeField] private PlayerCharacter playerPrefab;
        [SerializeField] private Transform homePoint;
        [SerializeField] private UI userInterface;
        
        [SerializeField] private int requiredPlayers = 2;

        [SerializeField] private UnityEvent<PlayerCharacter> OnLocalPlayerCreated;
        [SerializeField] private UnityEvent OnGameStarted;

        public Dictionary<Player, PlayerCharacter> Characters
        {
            get;
            private set;
        }

        private bool _canStartGame = false;
        private bool CanStartGame
        {
            get => _canStartGame;
            set
            {
                _canStartGame = value;
                
                // TODO add events or smth
                userInterface.StartGameButton.SetActive(value && PhotonNetwork.IsMasterClient);
            }
        }

        public static GameManager Instance { get; private set; }
        
        private bool _isGameStarted = false;

        public bool GameIsStarted
        {
            get => _isGameStarted;
            private set
            {
                if (value && _isGameStarted == false)
                {
                    _isGameStarted = true;
                    OnGameStarted?.Invoke();
                }
            }
        }

        private Voting _voting;
        
        
        private void Awake()
        {
            Characters = new Dictionary<Player, PlayerCharacter>();
            _voting = GetComponent<Voting>();

            Instance = this;
        }

        // TODO add remove player
        public void AddPlayer(PlayerCharacter character)
        {
            character.DeathEvent.AddListener(CheckGameState);
            Characters.Add(character.photonView.Owner,character);

            if (Characters.Count >= requiredPlayers)
                CanStartGame = true;

            if (character.photonView.IsMine)
            {
                OnLocalPlayerCreated?.Invoke(character);
            }
        }
        
        public void StartGame()
        {
            if(!PhotonNetwork.IsMasterClient || !CanStartGame) return;

            // pick impostor
            // var impostor = _characters[Random.Range(0, _characters.Count)];
            // impostor.photonView.RPC("SignRoles", RpcTarget.All,true);
            
            MovePlayersToSpawnPositions();
            
            GameIsStarted = true;
        }

        public void MovePlayersToSpawnPositions()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;

            var delta = 360 / Characters.Count;
            var angle = 0;
            foreach (var character in Characters.Values)
            {
                if (character.IsAlive)
                { 
                    var pos = homePoint.position + Quaternion.Euler(0, 0, angle) * Vector3.right;
                    character.photonView.RPC("RelocateTo", character.photonView.Owner, pos);
                }
                
                angle += delta;
            }
        }
        
        public void KickPlayer(Player player)
        {
            if(player is null)
                return;
            
            var character = Characters[player];
            character.Kill();
        }

        public void CheckGameState()
        {
            throw new NotImplementedException();
        }

        private void EndGame()
        {
            throw new NotImplementedException();

        }

        public void FreezePlayers()
        {
            Debug.Log("Players are frozen");
            foreach (var character in Characters.Values)
            {
                character.Freeze();
            }
        }

        public void UnfreezePlayers()
        {
            Debug.Log("Players are unfrozen");
            foreach (var character in Characters.Values)
            {
                character.Unfreeze();
            }
        }
    }
}