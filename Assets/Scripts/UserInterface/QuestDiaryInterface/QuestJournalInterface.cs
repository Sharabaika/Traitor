using UnityEngine;

namespace UserInterface.QuestDiaryInterface
{
    public class QuestJournalInterface : MonoBehaviour
    {
        // TODO
        
        public void Display()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}