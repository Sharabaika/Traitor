using System;
using Photon.Pun;
using UnityEngine;

namespace Misc
{
    public class NickPanel : MonoBehaviourPun
    {
        [SerializeField] private TextMesh text;

        private void Start()
        {
            text.text = photonView.Owner.NickName;
        }
    }
}