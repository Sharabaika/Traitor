using System.Collections.Generic;
using Characters;
using Logics;
using UnityEngine;

namespace UserInterface.InteractableObjectInterfaces.QuestBoard
{
    public class QuestBoardInterface : InteractableObjectInterface
    {
        [SerializeField] private QuestJournalPreviewPanel journalPreviewPrefab;
        [SerializeField] private List<QuestJournalPreviewPanel> previews;

        private List<QuestJournal> _journals;

        public override void Display(PlayerCharacter character = null)
        {
            foreach (var preview in previews)
            {
                preview.AttachedPlayer = character;
            }
            base.Display(character);
        }

        public void DisplayJournals(List<QuestJournal> journals)
        {
            this._journals = journals;

            foreach (var journal in journals)
            {
                var panel = InstantiatePanel(journal);
                previews.Add(panel);
            }
        }

        private QuestJournalPreviewPanel InstantiatePanel(QuestJournal journal)
        {
            // TODO fix
            
            var panel = Instantiate(journalPreviewPrefab, transform);
            var rectTransform = panel.GetComponent<RectTransform>();
            // rectTransform.pivot = new Vector2(10, -55f - 130*previews.Count );
            var rect = rectTransform.rect;
            rect.yMax = 50f + 210f * previews.Count;
            rect.yMin = 250f + 210f * previews.Count;
            
            panel. AttachedQuestJournal= journal;
            previews.Add(panel);
            return panel;
        }
    }
}