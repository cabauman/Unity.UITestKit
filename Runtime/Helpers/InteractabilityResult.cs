using UnityEngine;

namespace GameCtor.UITestKit
{
    public class InteractabilityResult
    {
        public InteractabilityResult(string name, Interactability interactability, Transform blocker = null)
        {
            Name = name;
            Interactability = interactability;
            if (blocker != null)
            {
                BlockerName = blocker.GetPath();
            }
        }

        public string Name { get; }

        public bool IsInteractable => Interactability == Interactability.Interactable;

        public Interactability Interactability { get; }

        public string BlockerName { get; }

        public override string ToString()
        {
            switch (Interactability)
            {
                case Interactability.Interactable:
                    return $"{Name} is interactable.";
                case Interactability.GraphicComponentMissing:
                    return $"{Name} doesn't contain a graphic component.";
                case Interactability.GraphicGameObjectInactive:
                    return $"{Name} contains at least one graphic component but none of their gameobjects are active.";
                case Interactability.GraphicComponentDisabled:
                    return $"{Name} contains at least one graphic component but none of them are enabled.";
                case Interactability.RaycastTargetFalse:
                    return $"{Name} contains at least one graphic component but none of them have raycastTarget set to true.";
                case Interactability.OutsideScreenBounds:
                    return $"{Name} is not within screen bounds.";
                case Interactability.Blocked:
                    return $"{Name} is blocked by {BlockerName}.";
                case Interactability.Culled:
                    return $"{Name} is culled by something, such as a Mask or RectMask2D component.";
                default:
                    return $"{Name} isn't interactable but the reason is unknown.";
            };
        }
    }
}
