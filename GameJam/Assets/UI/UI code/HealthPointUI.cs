using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    //delegate int HpAmount();
    public class HealthPointUI : MonoBehaviour
    {
        [SerializeField] private Image[] _heartImages;
        [SerializeField] private Sprite _fullHeart;
        [SerializeField] private Sprite _emptyHeart;
        
        [SerializeField] private int _hpHUD;
        [SerializeField] private int _numOfHearts;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>(); // подписываемся на класс до старта, чтобы можно было подписаться на ивент
        }

        private void Start()
        {
            _numOfHearts = _player._hp;
            _hpHUD = _player._hp;
        }

        private void OnEnable() // подписываемся на изменение хп
        {
            _player.HpChanged += UpdateHpUI;
        }

        private void UpdateHpUI() //обновляем хп
        {
            _hpHUD = _player._hp;
        }

        private void OnDisable() // отписываемся
        {
            _player.HpChanged -= UpdateHpUI;
        }

        private void Update()
        {
            for (int i = 0; i < _heartImages.Length; i++)
            {
                if (i < _hpHUD)
                {
                    _heartImages[i].sprite = _fullHeart;
                }
                else
                {
                    _heartImages[i].sprite = _emptyHeart;
                }
                if (i < _numOfHearts)
                {
                    _heartImages[i].enabled = true;
                }
                else
                {
                    _heartImages[i].enabled = false;
                }
            }
            /*if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(_numOfHearts);
            }*/
        }
    }
}