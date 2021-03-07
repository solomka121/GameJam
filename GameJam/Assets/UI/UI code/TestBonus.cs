using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestBonus : MonoBehaviour
    {
        [SerializeField] private bool _startTimer = false;
        [SerializeField] private float _effectTime;

        public BonusDurationUI bonusDurationUI;
        //private float _effectTimer;
        private float _effectTimerZero = 0.0f;

        /*private void Awake()
        {
            _timeSlider = GetComponent<BonusDurationUI>();
        }*/

        private void Start()
        {
            //_effectTimer = _effectTime;
            //bonusDurationUI.DebugTry(666.666f);
            bonusDurationUI.SetMaxTime(_effectTime);
            bonusDurationUI.SetDurationTime(_effectTime);
        }

        private void Update()
        {
            if (_startTimer)
            {
                bonusDurationUI.durationSlider.value -= Time.deltaTime;
                
                if (bonusDurationUI.durationSlider.value == _effectTimerZero)
                {
                    _startTimer = false;
                }
            }
        }
    }
}