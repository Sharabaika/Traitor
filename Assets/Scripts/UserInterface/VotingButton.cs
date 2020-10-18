using System;
using Characters;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class VotingButton : MonoBehaviour
    {
        public Text text;
        public Button button;
        public Image image;

        public PlayerCharacter attachedCharacter;
        public Player attachedPlayer;
        
        // TODO remove
        public VotingInterface votingInterface;
        
        public void Vote()
        {
            image.color = Color.red;
            votingInterface.VoteFor(attachedPlayer);
        }
    }
}