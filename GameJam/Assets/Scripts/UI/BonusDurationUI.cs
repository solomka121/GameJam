using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BonusDurationUI : MonoBehaviour
    {
        public Slider durationSlider;

        public void SetMaxTime(float time)
        {
            durationSlider.maxValue = time;
            durationSlider.value = time;
        }

        /*public void DebugTry(float t)
        {
            Debug.Log($"{t} *********");
        }*/

        public void SetDurationTime(float time)
        {
            durationSlider.value = time;
        }
    }
}