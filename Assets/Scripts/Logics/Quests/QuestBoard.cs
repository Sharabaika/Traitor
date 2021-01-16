using System;
using System.Collections.Generic;
using Characters;
using MapObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UserInterface;
using UserInterface.InteractableObjectInterfaces.QuestBoard;

namespace Logics.Quests
{
    public class QuestBoard : InteractableObject
    {

        [SerializeField] private List<QuestJournal> journals;
        protected override void OnStart()
        {
            // TODO serialize only QuestBoardInterface
            (ui as QuestBoardInterface).DisplayJournals(journals);
        }

        [PunRPC]public void SingJournalIfCan(int number, Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SingJournalIfCan", RpcTarget.MasterClient, number, player);
                return;
            }

            var journal = journals[number];
            if(journal.owner != null)
                return;
            
            var character = GameManager.Instance.GetPlayersCharacter(player);
            character.questJournal.owner = null;
            character.questJournal = journal;
            
            photonView.RPC("UpdateAvailableJournals", RpcTarget.Others, GetOwners());
        }

        [PunRPC] public void UpdateAvailableJournals(Player[] owners)
        {
            for (var i = 0; i < journals.Count; i++)
            {
                journals[i].owner = owners[i];
            }
        }

        public Player[] GetOwners()
        {
            var res = new Player[journals.Count];
            for (var i = 0; i < journals.Count; i++)
            {
                res[i] = journals[i].owner;
            }

            return res;
        }
        
    }
}