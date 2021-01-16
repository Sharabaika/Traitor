using Characters;
using Logics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface.InteractableObjectInterfaces.QuestBoard
{
    public class QuestJournalPreviewPanel : MonoBehaviour
    {
        [SerializeField] private QuestJournal attachedJournal;
        [SerializeField] private Text title;
        [SerializeField] private Text description;
        public QuestJournal AttachedQuestJournal
        {
            get => attachedJournal;
            set
            {
                title.text = value.name;
                attachedJournal = value;
            }
        }
        
        public PlayerCharacter AttachedPlayer { get; set; }

        public void TryAcceptQuest()
        {
            // AttachedPlayer.questJournal.AcceptQuest(attachedQuest);
        }
    }
}