using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace Logics
{
    public class GameTime : MonoBehaviour
    {
        [SerializeField] private float nightDuration;
        [SerializeField] private float dayDuration;

        [SerializeField] private UnityEvent onMidnight;
        [SerializeField] private UnityEvent onMorning;
        [SerializeField] private UnityEvent onHighNoon;
        [SerializeField] private UnityEvent onEvening;

        private Coroutine _gameCycle;

        public void StartGameCycle()
        {
            _gameCycle = StartCoroutine(GameCycle());
        }
        
        private IEnumerator GameCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(nightDuration/2f);
                onMorning?.Invoke();
                yield return new WaitForSeconds(dayDuration/2f);
                onHighNoon?.Invoke();
                yield return new WaitForSeconds(dayDuration/2f);
                onEvening?.Invoke();
                yield return new WaitForSeconds(nightDuration/2f);
                onMidnight?.Invoke();
            }
        }
        
        
    }
}