using UnityEngine;

namespace UserInterface
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject startGameButton;
        [SerializeField]private VotingInterface votingInterface;
        
        public GameObject StartGameButton => startGameButton;
        public VotingInterface VotingInterface => votingInterface;

        private void Start()
        {
            StartGameButton.SetActive(false);
        }
    }
}