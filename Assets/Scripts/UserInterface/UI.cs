using System.Collections.Generic;
using UnityEngine;
using UserInterface.InteractableObjectInterfaces;
using UserInterface.Voting;

namespace UserInterface
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject startGameButton;
        [SerializeField] private VotingInterface votingInterface;
        
        public GameObject StartGameButton => startGameButton;
        public VotingInterface VotingInterface => votingInterface;
    }
}