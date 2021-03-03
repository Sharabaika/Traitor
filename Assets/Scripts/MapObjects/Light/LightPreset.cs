using UnityEngine;

namespace MapObjects.Light
{
    [CreateAssetMenu(fileName = "New LightPreset", menuName = "Light Preset", order = 0)]
    public class LightPreset : ScriptableObject
    {
        [SerializeField] private Quaternion rotation;
        [SerializeField, Range(0f,1f)] private float intensity;

        public float Intensity => intensity;
        public Quaternion Rotation => rotation;
    }
}