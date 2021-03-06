using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float _doubleSpeedDuration;
    [SerializeField] private float _doubleJumpDuration;

    private void DoubleSpeedStart()
    {
        //MoutionBlur VFX
        gameObject.GetComponent<Player>().SetBonusSpeed(2);
        Invoke(nameof(DoubleSpeedFinal), _doubleSpeedDuration);
    }

    private void DoubleSpeedFinal()
    {
        gameObject.GetComponent<Player>().SetBonusSpeed(0.5f);
    }

    IEnumerator DoubleJumpActiveTime(float duration)
    {
        //Передать в UI duration
        Debug.Log("ДАБЛДЖАМПКУРУТИНА!");
        gameObject.GetComponent<Player>()._isDoubleJumpActive = true;
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<Player>()._isDoubleJumpActive = false;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            DoubleSpeedStart();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
          StartCoroutine(DoubleJumpActiveTime(_doubleJumpDuration));
        }
    }
}
