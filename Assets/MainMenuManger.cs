using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Edu.RPS.Core
{
    public sealed class MainMenuManger : MonoBehaviour
    {
        [SerializeField]
        private Button _btnStart = default;

        [SerializeField]
        private Button _btnExit = default;

        private void Awake()
        {
            _btnExit.onClick.AddListener(() => Application.Quit());
            _btnStart.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.Gameplay));
        }
    }
}

