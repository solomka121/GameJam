using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MessagesUI : MonoBehaviour
    {
        [SerializeField] public GameObject messages;
        [SerializeField] public GamePause GamePause;
        //private int counter;

        /*private void Awake()
        {
            counter = 0;
        }*/

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                Time.timeScale = 0;
                messages.SetActive(true);
                MenuCode.CursorShow();
                GamePause.messageUIActive = true;
            }
        }

        public void CloseMessage()
        {
            Time.timeScale = 1;
            messages.SetActive(false);
            MenuCode.CursorLock();
            GamePause.messageUIActive = false;
            gameObject.SetActive(false);
        }
    }
}