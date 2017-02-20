using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

namespace SB.Seed
{
    [RequireComponent(typeof(Receiver))]
    [RequireComponent(typeof(Collider))]
    public class UIInputTrigger : MonoBehaviour, IPointerTriggerPressSetHandler
    {
        public KeyboardEditor inputManager;
        public string Title;

        void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            inputManager.textEntry = GetComponent<UnityEngine.UI.InputField>();
            inputManager.ShowKeyboard(Title);
        }

        public void OnPointerTriggerPress(ViveControllerModule.EventData eventData)
        {
            //throw new NotImplementedException();
        }

        public void OnPointerTriggerPressUp(ViveControllerModule.EventData eventData)
        {
            //throw new NotImplementedException();
        }
    }
}