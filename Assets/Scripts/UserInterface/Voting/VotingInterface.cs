using System;
using System.Collections.Generic;
using Characters;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Voting
{
    public class VotingInterface : MonoBehaviour
    {
        [SerializeField] private VotingButton votingButtonPrefab;
        [SerializeField] private Text timer;

        private Logics.Voting _voting;
        private List<VotingButton> _buttons;
        private PlayerCharacter[] _characters;

        private float _remainingTime;
        private VotingButton _focusedOnButton;

        private void Awake()
        {
            _buttons = new List<VotingButton>(GetComponentsInChildren<VotingButton>());
        }

        private void Start()
        {
            _voting = FindObjectOfType<Logics.Voting>();
            gameObject.SetActive(false);
        }

        public void StartVoting(PlayerCharacter[] characters, float remainingTime)
        {
            _remainingTime = remainingTime;
            // TODO seems ton so good
            foreach (var button in _buttons)
            {
                button.onVotedForCharacter.AddListener(VoteFor);
                button.gameObject.SetActive(false);
            }
            
            _characters = characters;
            var i = 0;
            foreach (var character in characters)
            {
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].AttachedCharacter = character;
                
                i++;
            }
            gameObject.SetActive(true);
        }

        public void FocusOnButton(VotingButton button)
        {
            if (_focusedOnButton != null)
            {
                _focusedOnButton.HideConfirmButtons();
            }

            _focusedOnButton = button;
            button.ShowConfirmButtons();
        }

        public void VoteFor(PlayerCharacter character)
        {
            _voting.Vote(character.photonView.Owner, PhotonNetwork.LocalPlayer);
        }

        public void EndVoting()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            timer.text = Mathf.RoundToInt(_remainingTime).ToString();
            _remainingTime -= Time.deltaTime;
        }
    }
}