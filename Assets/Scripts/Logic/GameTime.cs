using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class GameTime : MonoBehaviourPun
    {
        [SerializeField] private float nightDurationSecs;
        [SerializeField] private float dayDurationSecs;

        [SerializeField] private float startingTime = 12f;

        public float CurrentTime { get; private set; }
        public int DaysPassed { get; private set; } = 0;
        public float CycleDuration => nightDurationSecs + dayDurationSecs;

        private Coroutine _timeCoroutine;

        private void Awake()
        {
            CurrentTime = startingTime;
        }

        public void StartTimeCycle()
        {
            photonView.RPC("StartCyclesSync", RpcTarget.All,PhotonNetwork.Time);
        }

        [PunRPC] public void StartCyclesSync(double startedAt)
        {
            _timeCoroutine = StartCoroutine(GameCycle(startedAt));
        }

        private IEnumerator GameCycle(double startedAt)
        {
            var timePassed = PhotonNetwork.Time - startedAt;
            CurrentTime += (float)timePassed;
            while (true)
            {
                yield return null;
                CurrentTime += Time.deltaTime / CycleDuration * 24f;
                
                if (CurrentTime > 24f)
                {
                    CurrentTime -= 24f;
                    DaysPassed++;
                }
            }
        }
    }
}