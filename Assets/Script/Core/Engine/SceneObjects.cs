using System;
using UnityEngine;
using Lncodes.Module.Unity.Editor;

namespace Edu.CrossyBox.Core
{
    [Serializable]
    public struct SceneObjects
    {
        [SerializeField]
        private SceneObject _mainMenu;

        [SerializeField]
        private SceneObject _gameplay;

        public SceneObject MainMenu { get => _mainMenu; }

        public SceneObject Gameplay { get => _gameplay; }

    }
}
