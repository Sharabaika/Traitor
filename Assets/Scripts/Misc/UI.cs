using System;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject startGameButton;
        public GameObject StartGameButton => startGameButton;

        private void Start()
        {
            StartGameButton.SetActive(false);
        }
    }
}