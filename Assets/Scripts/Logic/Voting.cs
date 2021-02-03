using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UserInterface;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Logics
{
    public class Voting: MonoBehaviourPun
    {
        [SerializeField] private double votingTimer = 180f;

        public UnityEvent<PlayerCharacter[], float> OnStartVoting;
        public UnityEvent OnEndVoting;

        private bool _isVotingNow = false;
        private GameManager _manager;

        private List<Player> _voters;

        private List<Player> _votes;

        private double _votingStartingTime;
        public double RemainingVotingTime => votingTimer - (PhotonNetwork.Time - _votingStartingTime);
        
        private bool CanStartVoting(Player initiator)
        {
            // TODO check for number of initiations
            // TODO fix
            // return _manager.GameIsStarted && !_isVotingNow && _manager.GetPlayersCharacter(initiator).IsAlive;
            throw new NotImplementedException();
        }

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
                if (CanStartVoting(initiator))
                {
                    photonView.RPC("StartVoting", RpcTarget.Others, initiator);

                    _votingStartingTime = PhotonNetwork.Time;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable()
                        {{"votingStartingTime", PhotonNetwork.Time}});
                    
                    _isVotingNow = true;

                    StartCoroutine(VotingTimer());

                }
                else
                {
                    return;
                }
                
            }
            _votes.Clear();
            _voters.Clear();

            _isVotingNow = true;

            if (!PhotonNetwork.IsMasterClient &&
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("votingStartingTime", out var value))
            {
                _votingStartingTime = (double) value;
            }
         
            // TODO Fix
            // OnStartVoting?.Invoke(_manager.Characters.ToArray(), (float)RemainingVotingTime);
        }

        private IEnumerator VotingTimer()
        {
            while (RemainingVotingTime > 0 && _isVotingNow)
            {
                yield return null;
            }

            if(_isVotingNow)
                EndVoting();
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


            // TODO fix
            // if (_voters.Count == _manager.AlivePlayers.Count)
            // {
            //     EndVoting();
            // }
        }
        
        [PunRPC] public void EndVoting()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var res = FindResult(_votes.ToArray());
                _manager.KickPlayer(res);
                photonView.RPC("EndVoting", RpcTarget.Others);
            }


            _isVotingNow = false;
            // TODO show results
            OnEndVoting?.Invoke();
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