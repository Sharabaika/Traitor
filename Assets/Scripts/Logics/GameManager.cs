using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;
using Characters;
using UserInterface;

namespace Logics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerCharacter playerPrefab;
        [SerializeField] private Transform homePoint;
        [SerializeField] private UI userInterface;
        
        [SerializeField] private int requiredPlayers = 2;

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

        public bool GameIsStarted { get; private set; } = false;

        private Voting _voting;
        
        
        private void Awake()
        {
            _characters = new List<PlayerCharacter>();
            _players = new List<Player>();
            _voting = GetComponent<Voting>();
        }

        // TODO add remove player
        public void AddPlayer(PlayerCharacter character)
        {
            character.DeathEvent.AddListener(CheckGameState);
            
            _characters.Add(character);
            _players.Add(character.photonView.Owner);

            if (_characters.Count >= requiredPlayers)
                CanStartGame = true;
            
            Debug.Log(character.photonView.Owner.NickName);
        }
        
        public void StartGame()
        {
            if(!PhotonNetwork.IsMasterClient || !CanStartGame) return;

            // pick impostor
            var impostor = _characters[Random.Range(0, _characters.Count)];
            impostor.photonView.RPC("SignRoles", RpcTarget.All,1, true);
                
            // TODO pick roles
            
            MovePlayersToSpawnPositions();

            // TODO spawn players

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
                if(!character.IsAlive)
                    continue;
                
                var pos = homePoint.position + Quaternion.Euler(0, 0, angle) * Vector3.right;
                character.photonView.RPC("RelocateTo", character.photonView.Owner, pos);
                
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
            Debug.Log("kek");
            
            if(!PhotonNetwork.IsMasterClient)
                return;

            if (Impostors.Count >= AliveCharacters.Count)
            {
                photonView.RPC("EndGame", RpcTarget.All);
            }
        }

        [PunRPC]private void EndGame()
        {
            Debug.Log("end");
        }

        public PlayerCharacter GetPlayersCharacter(Player player)
        {
            var i = _players.IndexOf(player);
            return _characters[i];
        }
    }
}