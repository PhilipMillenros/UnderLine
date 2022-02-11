using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FG
{
    public class IntroInput : MonoBehaviour
    {
        public GameObject UIObject;
        private void OnConnect()
        {
            UIObject.SetActive(true);
        }

        private void OnRetract(InputValue input)
        {
            UIObject.SetActive(false);
        }

    }
}