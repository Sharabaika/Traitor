using System.Collections;
using UnityEngine;

namespace MapObjects.Light
{
    public class LightGroup : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Light[] lights;

        private float _intensity;

        public float Intensity
        {
            get => _intensity;
            set
            {
                _intensity = Mathf.Clamp01(value);
                foreach (var l in lights)
                {
                    l.intensity = value;
                }
            }
        }

        private Quaternion _rotation;

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                foreach (var l in lights)
                {
                    l.transform.rotation = value;
                }
            }
        }
    }
}