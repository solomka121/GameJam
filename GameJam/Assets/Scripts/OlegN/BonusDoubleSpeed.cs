using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDoubleSpeed : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Bonus>().DoubleSpeedStart();
            Destroy(gameObject);
        }
    }
}
