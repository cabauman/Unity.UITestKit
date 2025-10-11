//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        private class WaitForSceneLoad : Condition
        {
            private string name;

            public WaitForSceneLoad(string name)
            {
                this.name = name;
                //EditorSceneManager.LoadSceneInPlayMode("Assets/Scenes/" + name + ".unity", new LoadSceneParameters());
                SceneManager.LoadScene("Assets/Scenes/" + name + ".unity", LoadSceneMode.Single);
            }

            public override bool IsFulfilled() => SceneManager.GetActiveScene().name.Equals(name);

            public override string GetResult()
            {
                return $"Waited for scene {name} to load but it never did.";
            }
        }
    }
}
