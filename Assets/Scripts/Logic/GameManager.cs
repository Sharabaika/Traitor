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

        [SerializeField] private UnityEvent OnGameStarted;

        private List<PlayerCharacter> _characters;
        private List<Player> _players;
        
        public List<PlayerCharacter> AliveCharacters => _characters.FindAll(character => character.IsAlive);
        public List<Player> AlivePlayers=> _players.FindAll(player => GetPlayersCharacter(player).IsAlive);
        
        public List<PlayerCharacter> Characters => _characters;
        public List<Player> Players => _players;
        public List<PlayerCharacter> Impostors => AliveCharacters.FindAll(character => character.IsImposter);

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
        
        public PlayerCharacter CurrentCharacter => GetPlayersCharacter(PhotonNetwork.LocalPlayer);

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
            _characters = new List<PlayerCharacter>();
            _players = new List<Player>();
            _voting = GetComponent<Voting>();

            Instance = this;
        }

        // TODO add remove player
        public void AddPlayer(PlayerCharacter character)
        {
            character.DeathEvent.AddListener(CheckGameState);
            
            _characters.Add(character);
            _players.Add(character.photonView.Owner);

            if (_characters.Count >= requiredPlayers)
                CanStartGame = true;

            character.DeathEvent.AddListener(CheckGameState);
        }
        
        public void StartGame()
        {
            if(!PhotonNetwork.IsMasterClient || !CanStartGame) return;

            var impostor = _characters[Random.Range(0, _characters.Count)];
            impostor.photonView.RPC("SignRoles", RpcTarget.All,true);
            
            MovePlayersToSpawnPositions();
            
            GameIsStarted = true;
        }

        public void MovePlayersToSpawnPositions()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;

            var delta = 360 / _characters.Count;
            var angle = 0;
            foreach (var character in _characters)
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
            
            var character = GetPlayersCharacter(player);
            character.Kill();
        }

        public void CheckGameState()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;

            var impostors = Impostors.Count;
            var civs = AliveCharacters.Count - impostors;
            if (impostors == 0)
            {
                Debug.Log("civs win");
            }
            else if (impostors >= civs)
            {
                Debug.Log("imps win");
            }
        }

        [PunRPC]private void EndGame()
        {
            Debug.Log("end of game");
        }

        public void FreezePlayers()
        {
            Debug.Log("Players are frozen");
            foreach (var character in _characters)
            {
                character.Freeze();
            }
        }

        public void UnfreezePlayers()
        {
            Debug.Log("Players are unfrozen");
            foreach (var character in _characters)
            {
                character.Unfreeze();
            }
        }
        
        
        public PlayerCharacter GetPlayersCharacter(Player player)
        {
            var i = _players.IndexOf(player);
            return _characters[i];
        }
    }
}