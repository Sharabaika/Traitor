using UnityEngine;

namespace MapObjects
{
    [CreateAssetMenu(fileName = "New interactable obj style", menuName = "Interactable object style", order = 0)]
    public class InteractableObjectStyle : ScriptableObject
    {
        [SerializeField] public Material outlineShader;
        [SerializeField] public Material defaultShader;
    }
}