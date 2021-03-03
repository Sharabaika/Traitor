using System;
using System.Xml;
using Logic;
using UnityEngine;

namespace MapObjects.Light
{
    public class Sun : MonoBehaviour
    {
        [SerializeField] private GameTime timer;

        private Vector3 _midnightRot =new Vector3(270, 270, 0);
        private Vector3 _nextmidnightRot =new Vector3(270+360, 270, 0);
        
        private void Update()
        {
            var t = timer.CurrentTime / 24f;
            var targetRot = Vector3.Lerp(_midnightRot, _nextmidnightRot, t);
            transform.rotation = Quaternion.Euler(targetRot);
        }
    }
}