using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;
using Characters;
using Networking;
using UnityEngine.Events;
using UserInterface;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Logics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public enum Roles
        {
            Serb, Croat, Murderer,Survivor
        }
        
        [SerializeField] private Transform homePoint;
        [SerializeField] private int requiredPlayers = 2;
        
        [SerializeField] private UnityEvent<PlayerCharacter> onLocalPlayerCreated;
        
        [SerializeField] private UnityEvent<bool> onCanStartGame;
        [SerializeField] private UnityEvent<bool> onCanStartGameMaster;
        
        [SerializeField] private UnityEvent onGameStarted;
        [SerializeField] private UnityEvent onGameStartedMaster;

        [SerializeField] private Class[] classes;
        [SerializeField] private ClassList classIDList;
        
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
                if (GameIsStarted)
                {
                    _canStartGame = false;
                }else
                {
                    _canStartGame = value;
                }
                onCanStartGame.Invoke(CanStartGame);
                
                if (PhotonNetwork.IsMasterClient)
                    onCanStartGameMaster.Invoke(CanStartGame);
            }
        }

        private bool _gameIsStarted = false;

        public bool GameIsStarted
        {
            get => _gameIsStarted;
            private set
            {
                _gameIsStarted = value;
                if (value)
                    CanStartGame = false;
            }
        }

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Characters = new Dictionary<Player, PlayerCharacter>();
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
                onLocalPlayerCreated?.Invoke(character);
            }
        }
        
        public void StartGame()
        {
            if(!PhotonNetwork.IsMasterClient || !CanStartGame) return;
            GameIsStarted = true;
            
            onGameStartedMaster.Invoke();
            photonView.RPC("OnGameStarted_PRC", RpcTarget.All);
        }

        [PunRPC] public void OnGameStarted_PRC()
        {
            onGameStarted.Invoke();
        }

        public void SingClasses()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            foreach (var character in Characters.Values)
            {
                var index = Random.Range(0, classes.Length);
                var characterClass = classes[index];
                
                // TODO it should do Class , nor GameManager
                character.Inventory.SetItems(characterClass.StartingInventory);
                character.Health.MaxHealth = characterClass.MaxHealth;
                character.Health.RemainingHealth = characterClass.StartingHealth;
            }
        }
        
        private void SingRoles()
        {
            var players = Characters.Keys.ToList();

            var serbian = players[Random.Range(0, players.Count)];
            var sHash = new Hashtable{{"Role", Roles.Serb.ToString()}};
            serbian.SetCustomProperties(sHash);
            players.Remove(serbian);
            
            var croat = players[Random.Range(0, players.Count)];
            var cHash = new Hashtable{{"Role", Roles.Croat.ToString()}};
            croat.SetCustomProperties(cHash);
            players.Remove(croat);

            var hash = new Hashtable{{"Role",Roles.Survivor.ToString()}};
            foreach (var player in players)
            {
                player.SetCustomProperties(hash);
            }
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
            // TODO
        }

        private void EndGame()
        {
            // TODO
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