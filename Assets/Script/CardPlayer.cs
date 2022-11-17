using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Edu.RPS.Battle;
using Edu.RPS.Layout;

namespace Edu.RPS.Player
{
    public sealed class CardPlayer : MonoBehaviour
    {
        [SerializeField]
        private Transform _attackPositionRef = default;

        private Card _choosenCard = default;

        [SerializeField]
        private HealthBar _healthBar = default;

        [SerializeField]
        private TMP_Text _txtHealth = default;

        [HideInInspector]
        public float MaxHealth = default;

        public float Health = default;

        private Tweener _tweener = default;

        public Attack? AttackValue
        {
            get => _choosenCard is null ? null : _choosenCard.AttackValue;
        }

        public void SetChoosenCard(Card card)
        {
            if (_choosenCard is not null)
            {
                _choosenCard.Reset();
                return;
            }

            _choosenCard = card;
            _choosenCard.transform.DOScale(_choosenCard.transform.localScale * 1.1f, 0.2f);
        }

        private void Awake()
        {
            _txtHealth.text = "Loading...";
            IsClickable(false);
        }

        public void InitHealth(float maxHealth)
        {
            Health = maxHealth;
            MaxHealth = maxHealth;
            _healthBar.UpdateBar(Health / MaxHealth);
            _txtHealth.text = $"{Health:f2} / {MaxHealth:f1}";
        }

        public void ChangeHealth(float ammount)
        {
            Health -= ammount;
            Health = Mathf.Clamp(Health, 0, MaxHealth);

            _healthBar.UpdateBar(Health / MaxHealth);
            _txtHealth.text = $"{Health:f2} / {MaxHealth:f2}";
        }

        public void Reset()
        {
            if (_choosenCard != null)
                _choosenCard.Reset();
            _choosenCard = null;
        }

        public void AnimateAttack()
        {
            _tweener = _choosenCard.transform.DOMove(_attackPositionRef.position, 0.5f);
        }

        public void AnimateDamage()
        {
            var image = _choosenCard.GetComponent<Image>();
            _tweener = image
                .DOColor(Color.red, 0.1f)
                .SetLoops(3, LoopType.Yoyo)
                .SetDelay(0.2f);
        }

        public void AnimateDraw()
        {
            _tweener = _choosenCard.transform
                .DOMove(_attackPositionRef.position, 0.5f)
                .SetDelay(0.2f);
        }

        public bool IsAnimating() =>
            _tweener.IsActive();

        public void IsClickable(bool value)
        {
            var cards = GetComponentsInChildren<Card>();
            foreach (var item in cards)
                item.SetClickable(value);
        }

    }
}
