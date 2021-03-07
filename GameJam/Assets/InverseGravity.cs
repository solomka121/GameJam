using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseGravity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().enabled)
            {
                Debug.Log("INVERSED!");
                other.gameObject.GetComponent<Player>().enabled = false;
                other.gameObject.GetComponent<ChangeGravityTest>().enabled = true;
            }
			else
			{
                Debug.Log("BACK TO NORMAL!");
                other.gameObject.GetComponent<Player>().enabled = true;
                other.gameObject.GetComponent<ChangeGravityTest>().enabled = false;
            }
        }
    }
}
