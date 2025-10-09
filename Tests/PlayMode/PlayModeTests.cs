using System.Collections;
using UnityEngine.TestTools;

namespace GameCtor.UITestKit
{
    public class PlayModeTests : UITest
    {
        [UnityTest]
        //[LoadScene("Assets/Samples/UITestKit/UITestKitDemo.unity")]
        public IEnumerator PlayModeTestsWithEnumeratorPasses()
        {
            // Loading screen takes 1 second.
            yield return WaitUntilActive<NewBehaviourScript>();
            yield return Tap("MyButton");
            yield return WaitUntilInactive<NewBehaviourScript>();
        }
    }
}
