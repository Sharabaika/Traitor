using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MapObjects
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private Light2D[] lights;
        
        [SerializeField] private float lerpingTime;

        [SerializeField, Range(0f,1f)] private float intensity;

        public float Intensity
        {
            get => intensity;
            set
            {
                if(0f<value && value>1f)
                    return;
                
                intensity = value;
                foreach (var light2D in lights)
                {
                    light2D.intensity = value;
                }
            }
        }
        
        public void LerpIntensityOverTime(float to)
        {
            StartCoroutine(LerpingIntensityOverTime(to, lerpingTime));
        }

        public IEnumerator LerpingIntensityOverTime(float to, float time)
        {
            var from = Intensity;
            var t = 0f;
            while (t<=time)
            {
                Intensity = Mathf.Lerp(from, to, t / time);
                t += Time.deltaTime;
                yield return null;
            }

            Intensity = to;
        }

        private void OnValidate()
        {
            // Intensity = intensity;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var light2D in lights)
            {
                Gizmos.DrawLine(transform.position, light2D.transform.position);
            }
        }
    }
}