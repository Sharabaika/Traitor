using System;
using System.Collections.Generic;
using Characters;
using Logics;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class VotingInterface : MonoBehaviour
    {
        [SerializeField] private VotingButton votingButtonPrefab;
        [SerializeField] private Transform[] placeholders;


        private Voting _voting;
        private List<VotingButton> _buttons;
        private PlayerCharacter[] _characters;

        private bool _isInitialized = false;

        private void Awake()
        {
            _buttons = new List<VotingButton>();
        }

        private void Start()
        {
            // TODO remove
            _voting = FindObjectOfType<Voting>();
            
            gameObject.SetActive(false);
        }

        // TODO lock buttons after vote
        
        private void Initialize(PlayerCharacter[] characters, Player[] players)
        {
            foreach (var button in _buttons)
            {
                Destroy(button.gameObject);
            }
            _buttons.Clear();
            
            _characters = characters;
            var i = 0;
            foreach (var character in characters)
            {
                var button = Instantiate(votingButtonPrefab, placeholders[i]);

                button.text.text = character.photonView.Owner.NickName;
                button.attachedCharacter = character;
                button.attachedPlayer = players[i];
                button.votingInterface = this;

                _buttons.Add(button);
                i++;
            }

            _isInitialized = true;
        }

        public void StartVoting(PlayerCharacter[] characters, Player[] players)
        {
            Initialize(characters, players);
            gameObject.SetActive(true);
        }

        public void VoteFor(Player player)
        {
            _voting.Vote(player, PhotonNetwork.LocalPlayer);
        }

        public void EndVoting()
        {
            gameObject.SetActive(false);
        }
    }
}