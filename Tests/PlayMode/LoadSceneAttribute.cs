using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace GameCtor.UITestKit
{
    public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
    {
        private readonly string _sceneName;

        public LoadSceneAttribute(string sceneName)
        {
            _sceneName = sceneName;
        }

        public IEnumerator BeforeTest(ITest test)
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(_sceneName, new LoadSceneParameters(LoadSceneMode.Single));
        }

        public IEnumerator AfterTest(ITest test)
        {
            yield return null;
        }
    }
}
