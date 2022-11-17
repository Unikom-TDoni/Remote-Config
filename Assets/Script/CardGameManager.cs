using TMPro;
using UnityEngine;
using Edu.RPS.Player;
using Edu.RPS.Battle;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Edu.RPS.Core;

namespace Edu.RPS.Card
{
    public sealed class CardGameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameOverPanel = default;

        [SerializeField]
        private TMP_Text _winnerText = default;

        [SerializeField]
        private CardPlayer _playerOne = default;

        [SerializeField]
        private CardPlayer _playerTwo = default;

        [SerializeField]
        private float _restoreValue = default;

        [SerializeField]
        private float _damageValue = default;

        [SerializeField]
        private Button _btnRestart = default;

        [SerializeField]
        private Button _btnExit = default;

        [SerializeField]
        private Button _btnRestartGameOver = default;

        [SerializeField]
        private Button _btnExitGameOver = default;

        [SerializeField]
        private Edu.RPS.Player.Card[] _botCard = default;

        private CardPlayer _damagedPlayer = default;

        private CardGameState _cardGameState = CardGameState.ChooseAttack;

        public enum CardGameState
        {
            ChooseAttack,
            Attack,
            Damage,
            Draw,
            GameOver
        }

        private void Awake()
        {
            _btnExit.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.MainMenu));
            _btnRestart.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.Gameplay));
            _btnExitGameOver.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.MainMenu));
            _btnRestartGameOver.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.Gameplay));
        }

        private void Update()
        {
            switch (_cardGameState)
            {
                case CardGameState.ChooseAttack:
                    if (_playerOne.AttackValue != null)
                    {
                        var randomCard = _botCard[Random.Range(default, _botCard.Length)];
                        randomCard.ChooseCard();
                        _playerOne.AnimateAttack();
                        _playerTwo.AnimateAttack();
                        _playerOne.IsClickable(false);
                        _playerTwo.IsClickable(false);
                        _cardGameState = CardGameState.Attack;
                    }
                    break;
                case CardGameState.Attack:
                    if (!_playerOne.IsAnimating() && !_playerTwo.IsAnimating())
                    {
                        _damagedPlayer = GetDamagePlayer();
                        if (_damagedPlayer != null)
                        {
                            _damagedPlayer.AnimateDamage();
                            _cardGameState = CardGameState.Damage;
                        }
                        else
                        {
                            _playerOne.AnimateDraw();
                            _playerTwo.AnimateDraw();
                            _cardGameState = CardGameState.Draw;
                        }
                    }
                    break;
                case CardGameState.Damage:
                    if (!_playerOne.IsAnimating() && !_playerTwo.IsAnimating())
                    {
                        if (_damagedPlayer == _playerOne)
                        {
                            _playerOne.ChangeHealth(_damageValue);
                            _playerTwo.ChangeHealth(-_restoreValue);
                        }
                        else
                        {
                            _playerOne.ChangeHealth(-_restoreValue);
                            _playerTwo.ChangeHealth(_damageValue);
                        }

                        var winner = GetWinner();
                        if (winner == null)
                        {
                            ResetPlayers();
                            _playerOne.IsClickable(true);
                            _playerTwo.IsClickable(true);
                            _cardGameState = CardGameState.ChooseAttack;
                        }
                        else
                        {
                            gameOverPanel.SetActive(true);
                            _winnerText.text = winner == _playerOne ? "Player 1 wins" : "Bot wins";
                            ResetPlayers();
                            _cardGameState = CardGameState.GameOver;
                        }
                    }
                    break;
                case CardGameState.Draw:
                    if (!_playerOne.IsAnimating() && !_playerTwo.IsAnimating())
                    {
                        ResetPlayers();
                        _playerOne.IsClickable(true);
                        _playerTwo.IsClickable(true);
                        _cardGameState = CardGameState.ChooseAttack;
                    }
                    break;
            }
        }

        private void ResetPlayers()
        {
            _damagedPlayer = null;
            _playerOne.Reset();
            _playerTwo.Reset();
        }

        public void SetDamageRestore(float damage, float restore)
        {
            _damageValue = damage;
            _restoreValue = restore;
        }

        public CardPlayer GetDamagePlayer()
        {
            var _playerAttack1 = _playerOne.AttackValue;
            var _playerAttack2 = _playerTwo.AttackValue;

            if (_playerAttack1 == Attack.Rock && _playerAttack2 == Attack.Paper)
            {
                return _playerOne;
            }
            else if (_playerAttack1 == Attack.Rock && _playerAttack2 == Attack.Scissor)
            {
                return _playerTwo;
            }
            else if (_playerAttack1 == Attack.Paper && _playerAttack2 == Attack.Rock)
            {
                return _playerTwo;
            }
            else if (_playerAttack1 == Attack.Paper && _playerAttack2 == Attack.Scissor)
            {
                return _playerOne;
            }
            else if (_playerAttack1 == Attack.Scissor && _playerAttack2 == Attack.Rock)
            {
                return _playerOne;
            }
            else if(_playerAttack1 == Attack.Scissor && _playerAttack2 == Attack.Paper)
            {
                return _playerTwo;
            }

            return null;
        }

        private CardPlayer GetWinner()
        {
            if (_playerOne.Health == 0)
                return _playerTwo;
            else if (_playerTwo.Health == 0)
                return _playerOne;
            else return null;
        }
    } 
}
