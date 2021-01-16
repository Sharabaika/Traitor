using Characters;
using UnityEngine;

namespace UserInterface.InteractableObjectInterfaces
{
    public class InteractableObjectInterface : MonoBehaviour
    {
        protected PlayerCharacter character;
        
        // TODO display through UI class
        public virtual void Display(PlayerCharacter character = null)
        {
            this.character = character;
            gameObject.SetActive(true);
        }
        

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}