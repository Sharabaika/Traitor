using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UserInterface;

namespace Logics
{
    public class Voting: MonoBehaviourPun
    {
        [SerializeField] private UI userInterface;

        private bool _isVotingNow = false;
        private GameManager _manager;

        private bool _canStartVoting(Player initiator)
        {
            // TODO check for number of initiations
            return _manager.GameIsStarted && !_isVotingNow && _manager.GetPlayersCharacter(initiator).IsAlive;
        }

        private List<Player> _voters;
        private List<Player> _votes;

        private void Awake()
        {
            _manager = GetComponent<GameManager>();
            _voters = new List<Player>();
            _votes = new List<Player>();
        }

        public void InitiateVoting()
        {
            photonView.RPC("StartVoting", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
        }

        [PunRPC]public void StartVoting(Player initiator)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (_canStartVoting(initiator))
                {
                    photonView.RPC("StartVoting", RpcTarget.Others, initiator);

                }
                else
                {
                    return;
                }
                
            }
            _votes.Clear();
            _voters.Clear();

            _isVotingNow = true;
            userInterface.VotingInterface.StartVoting(_manager.AliveCharacters.ToArray(), _manager.AlivePlayers.ToArray());
        }

        // TODO add voting timer
        
        [PunRPC] public void Vote(Player vote, Player voter)
        {
            // TODO add check for vote sender, sync with others
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("Vote", RpcTarget.MasterClient, vote, voter);
                return;
            }

            if(_voters.Contains(voter))
                return;
            
            _votes.Add(vote);
            _voters.Add(voter);


            if (_voters.Count == _manager.AlivePlayers.Count)
            {
                // TODO убрать говнокод
                var res = FindResult(_votes.ToArray());
                photonView.RPC("EndVoting", RpcTarget.All, _voters.ToArray(), _votes.ToArray(), res);
            }
        }

        [PunRPC] public void EndVoting(Player[] voters, Player[] votes, Player result)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _manager.KickPlayer(result);
            }

            _isVotingNow = false;
            userInterface.VotingInterface.EndVoting();
            Debug.Log(result);
        }

        private static Player FindResult(Player[] votes)
        {
            /*
             string maxRepeated = prod.GroupBy(s => s)
                         .OrderByDescending(s => s.Count())
                         .First().Key;
             */
            
            
            var l = votes.Length;
            var players = new List<Player>();

            foreach (var vote in votes)
            {
                if(!players.Contains(vote))
                    players.Add(vote);
            }

            var counts = new List<int>();
            for (int i = 0; i < players.Count; i++)
            {
                counts.Add(players.FindAll((player => player == players[i])).Count);
            }
            

            var max = Mathf.Max(counts.ToArray());
            Debug.Log(max);

            // TODO убрать говнокод
            if (counts.FindAll(n => n == max).Count == 1)
            {
                return players[counts.IndexOf(max)];
            }

            return null;
        }
    }
}