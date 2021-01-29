// using System.Collections.Generic;
// using Characters;
// using Logics;
// using UnityEngine;
//
// namespace UserInterface.InteractableObjectInterfaces.QuestBoard
// {
//     public class QuestBoardInterface : InteractableObjectInterface
//     {
//         [SerializeField] private QuestJournalPreviewPanel journalPreviewPrefab;
//         [SerializeField] private List<QuestJournalPreviewPanel> previews;
//
//         private List<Quest> _quests;
//
//         public override void Display(PlayerCharacter character = null)
//         {
//             foreach (var preview in previews)
//             {
//                 preview.AttachedPlayer = character;
//             }
//             base.Display(character);
//         }
//
//         public void DisplayJournals(List<Quest> quests)
//         {
//             this._quests = quests;
//
//             foreach (var quest in quests)
//             {
//                 // var panel = InstantiatePanel(quest);
//                 // previews.Add(panel);
//             }
//         }
//
//         private QuestJournalPreviewPanel InstantiatePanel(Quest quest)
//         {
//             
//             
//             var panel = Instantiate(journalPreviewPrefab, transform);
//             var rectTransform = panel.GetComponent<RectTransform>();
//             // rectTransform.pivot = new Vector2(10, -55f - 130*previews.Count );
//             var rect = rectTransform.rect;
//             rect.yMax = 50f + 210f * previews.Count;
//             rect.yMin = 250f + 210f * previews.Count;
//             
//             // panel. AttachedQuestJournal= journal;
//             previews.Add(panel);
//             return panel;
//         }
//     }
// }