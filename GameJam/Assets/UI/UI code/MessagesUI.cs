using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MessagesUI : MonoBehaviour
    {
        [SerializeField] public GameObject message;
        [SerializeField] public GamePause GamePause;

    
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                Time.timeScale = 0;
                message.SetActive(true);
                MenuCode.CursorShow();
                GamePause.messageUIActive = true;
            }
        }

        public void CloseMessage()
        {
            Time.timeScale = 1;
            message.SetActive(false);
            MenuCode.CursorLock();
            GamePause.messageUIActive = false;
            gameObject.SetActive(false);
        }
    }
}