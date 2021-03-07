using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float _doubleSpeedDuration;
    [SerializeField] private float _doubleJumpDuration;

    public void DoubleSpeedStart()
    {
        //MoutionBlur VFX
        gameObject.GetComponent<Player>().SetBonusSpeed(2);
        Invoke(nameof(DoubleSpeedFinal), _doubleSpeedDuration);
    }

    private void DoubleSpeedFinal()
    {
        gameObject.GetComponent<Player>().SetBonusSpeed(0.5f);
    }

    public void StartDoubleJump()
    {
        StartCoroutine(DoubleJumpActiveTime(_doubleJumpDuration));
    }
     IEnumerator DoubleJumpActiveTime(float duration)
    {
        //Передать в UI duration
        Debug.Log("ДАБЛДЖАМПКУРУТИНА!");
        gameObject.GetComponent<Player>()._isDoubleJumpActive = true;
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<Player>()._isDoubleJumpActive = false;
    }
}
