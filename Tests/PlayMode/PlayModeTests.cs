using System.Collections;
using UnityEngine.TestTools;

namespace GameCtor.UITestKit
{
    public class PlayModeTests : UITest
    {
        [UnityTest]
        public IEnumerator PlayModeTestsWithEnumeratorPasses()
        {
            yield return LoadScene("UITestScene");
            yield return WaitUntilVisible<NewBehaviourScript>();
            yield return Tap("MyButton");
            yield return WaitUntilHidden<NewBehaviourScript>();
        }
    }
}
