using Characters;
using Logics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface.Voting
{
    public class VotingButton : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        
        [SerializeField] private Button button;
        [SerializeField] private GameObject confirmButtons;

        [SerializeField] public UnityEvent<PlayerCharacter> onVotedForCharacter;
        
        private PlayerCharacter _attachedCharacter;
        public Player AttachedPlayer => _attachedCharacter.photonView.Owner;

        private bool _isActive;
        public bool IsActive
        {
            // TODO
            get => _isActive;
            set
            {
                _isActive = value;
                button.interactable = value;
            }
        }

        public PlayerCharacter AttachedCharacter
        {
            get => _attachedCharacter;
            set
            {
                _attachedCharacter = value;
                text.text = AttachedPlayer.NickName;
            }
        }

        public void HideConfirmButtons()
        {
            confirmButtons.SetActive(false);
        }
        public void ShowConfirmButtons()
        {
            confirmButtons.SetActive(true);
        }
        
        public void Vote()
        {
            onVotedForCharacter?.Invoke(AttachedCharacter);
            image.color = Color.red;
        }
    }
}