using Edu.RPS.Card;
using Edu.RPS.Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using Unity.VisualScripting;
using UnityEngine;
using ConfigResponse = Unity.Services.RemoteConfig.ConfigResponse;

namespace Edu.RPS.Service
{
    public struct userAttributes
    {
        public float Health;
        public float Damage;
        public float Restore;
    }

    public struct appAttributes
    {

    }

    public sealed class RemoteConfigManager : MonoBehaviour
    {
        [SerializeField]
        private string _environmentName = default;

        [SerializeField]
        private CardPlayer _playerOne = default;

        [SerializeField]
        private CardPlayer _playerTwo = default;

        [SerializeField]
        private CardGameManager _manager = default;

        async void Awake()
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName(_environmentName);
            await UnityServices.InitializeAsync(options);
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            RemoteConfigService.Instance.FetchCompleted += OnFetchComplate;
            fetchConfig();
        }

        private void OnDestroy()
        {
            RemoteConfigService.Instance.FetchCompleted -= OnFetchComplate;
        }

        private void fetchConfig()
        {
            RemoteConfigService.Instance.FetchConfigs(
                    new userAttributes() { },
                    new appAttributes() { }
                );
        }

        private void OnFetchComplate(ConfigResponse response)
        {
            switch (response.requestOrigin)
            {
                case Unity.Services.RemoteConfig.ConfigOrigin.Default:
                    SetDifficultyVariable(0);
                    Debug.Log("From Default");
                    break;
                case Unity.Services.RemoteConfig.ConfigOrigin.Cached:
                    SetDifficultyVariable(0);
                    Debug.Log("From Chache");
                    break;
                case Unity.Services.RemoteConfig.ConfigOrigin.Remote:
                    SetDifficultyVariable(RemoteConfigService.Instance.appConfig.GetInt("Difficulty"));
                    break;
            }
        }

        private void SetDifficultyVariable(int difficulty)
        {
            _playerOne.IsClickable(true);
            switch (difficulty)
            {
                case 0:
                    _playerOne.InitHealth(100);
                    _playerTwo.InitHealth(100);
                    _manager.SetDamageRestore(10, 5);
                    break;
                case 1:
                    _playerOne.InitHealth(90);
                    _playerTwo.InitHealth(110);
                    _manager.SetDamageRestore(20, 7);
                    break;
                default:
                    _playerOne.InitHealth(80);
                    _playerTwo.InitHealth(120);
                    _manager.SetDamageRestore(30, 10);
                    break;
            }
        }

    }
}