using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        private class WaitForInteractableButton : Condition
        {
            private string name;
            private GameObject go;
            private Selectable button;
            private InteractabilityResult interactabilityResult;

            public WaitForInteractableButton(string name)
            {
                this.name = name;
            }

            public GameObject ButtonObject => go;

            public override bool IsFulfilled()
            {
                if (go is null)
                {
                    go = GameObject.Find(name);
                }

                if (go is null)
                {
                    return false;
                }
                else
                {
                    button = go.GetComponent<Selectable>();
                    var clickable = button != null && button.enabled && button.interactable;
                    interactabilityResult = UIUtil.CheckInteractability(go.transform);
                    return clickable && interactabilityResult.IsInteractable;
                }
            }

            public override string GetResult()
            {
                if (go is null)
                {
                    return $"Waited for {name} to become clickable but it's either inactive or not present in the scene.";
                }

                if (button is null)
                {
                    return $"Waited for {name} to become clickable but it doesn't have a {nameof(Selectable)} component.";
                }

                if (!button.enabled)
                {
                    return $"Waited for {name} to become clickable but the {nameof(Selectable)} component is disabled.";
                }

                if (!button.interactable)
                {
                    return $"Waited for {name} to become clickable but the {nameof(Selectable.interactable)} property remained false.";
                }

                return interactabilityResult.ToString();
            }
        }
    }
}
