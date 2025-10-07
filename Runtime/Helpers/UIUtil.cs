using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCtor.UITestKit
{
    public static class UIUtil
    {
        public static bool IsInteractable(RectTransform rectTransform)
        {
            return CheckInteractability(rectTransform).IsInteractable;
        }

        public static InteractabilityResult CheckInteractability(Transform transform)
        {
            switch (CheckRaycastability(transform))
            {
                case Raycastability.GraphicComponentMissing:
                    return new InteractabilityResult(transform.name, Interactability.GraphicComponentMissing);
                case Raycastability.GraphicGameObjectInactive:
                    return new InteractabilityResult(transform.name, Interactability.GraphicGameObjectInactive);
                case Raycastability.GraphicComponentDisabled:
                    return new InteractabilityResult(transform.name, Interactability.GraphicComponentDisabled);
                case Raycastability.RaycastTargetFalse:
                    return new InteractabilityResult(transform.name, Interactability.RaycastTargetFalse);
            }

            if (!IsAtLeastPartiallyWithinScreenBounds(transform))
            {
                return new InteractabilityResult(transform.name, Interactability.OutsideScreenBounds);
            }

            var raycastResults = Raycast(transform);
            if (raycastResults.Count == 0)
            {
                // The target object is *not* included in the results, even though it's raycastable
                // and at least partially within screen bounds. By process of elimination, this
                // means it must be culled by something, such as a Mask or RectMask2D component.
                return new InteractabilityResult(transform.name, Interactability.Culled);
            }

            var current = raycastResults[0].gameObject.transform;
            while (current != null && !current.name.Equals(transform.name))
            {
                if (current.GetComponent(typeof(IPointerClickHandler)) != null || current.GetComponent(typeof(IPointerDownHandler)) != null)
                {
                    current = null;
                    break;
                }

                current = current.parent;
            }

            if (current == null)
            {
                foreach (var result in raycastResults)
                {
                    if (string.Equals(result.gameObject.name, transform.name))
                    {
                        // The target object is included in the results; just not the first result.
                        // This means it's blocked by another object.
                        return new InteractabilityResult(transform.name, Interactability.Blocked, raycastResults[0].gameObject.transform);
                    }
                }

                // The target object is *not* included in the results, even though it's raycastable
                // and at least partially within screen bounds. By process of elimination, this
                // means it must be culled by something, such as a Mask or RectMask2D component.
                return new InteractabilityResult(transform.name, Interactability.Culled);
            }

            return new InteractabilityResult(transform.name, Interactability.Interactable);
        }

        public static Raycastability CheckRaycastability(Transform transform)
        {
            var graphics = transform.GetComponentsInChildren<Graphic>(true);
            if (graphics == null || graphics.Length == 0)
            {
                return Raycastability.GraphicComponentMissing;
            }

            var active = false;
            var enabled = false;
            var raycastTarget = false;
            foreach (var graphic in graphics)
            {
                if (!graphic.gameObject.activeInHierarchy)
                {
                    continue;
                }

                active |= true;

                if (!graphic.enabled)
                {
                    continue;
                }

                enabled |= true;

                if (!graphic.raycastTarget)
                {
                    continue;
                }

                return Raycastability.Raycastable;
            }

            if (!active)
            {
                return Raycastability.GraphicGameObjectInactive;
            }

            if (!enabled)
            {
                return Raycastability.GraphicComponentDisabled;
            }

            if (!raycastTarget)
            {
                return Raycastability.RaycastTargetFalse;
            }

            return Raycastability.Raycastable;
        }

        public static IReadOnlyList<RaycastResult> Raycast(Transform transform)
        {
            var normalizedPositions = new[]
            {
                new Vector2(0.5f, 0.5f), // Center
                new Vector2(0.1f, 0.1f), // Bottom-left
                new Vector2(0.1f, 0.9f), // Top-left
                new Vector2(0.9f, 0.9f), // Top-right
                new Vector2(0.9f, 0.1f), // Bottom-right
            };

            var rt = transform.GetComponent<RectTransform>();
            var pivotDelta = new Vector2(0.5f, 0.5f) - rt.pivot;
            var offset = Vector2.Scale(pivotDelta, rt.rect.size);
            var position = new Vector2(rt.position.x, rt.position.y) + offset;

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = position,
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results;
        }

        public static bool IsAtLeastPartiallyWithinScreenBounds(Transform transform)
        {
            return transform.GetComponent<RectTransform>().CountCornersVisibleFrom(camera: null) > 0;
        }

        /// <summary>
        /// Counts the corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The number of corners that are visible from the camera.</returns>
        /// <param name="rectTransform">The rect transform.</param>
        /// <param name="camera">An optional camera.</param>
        public static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera = null)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] rectTransformCorners = new Vector3[4];
            rectTransform.GetWorldCorners(rectTransformCorners);

            int visibleCorners = 0;
            Vector2 screenSpaceCorner;
            for (var i = 0; i < rectTransformCorners.Length; i++)
            {
                screenSpaceCorner = RectTransformUtility.WorldToScreenPoint(camera, rectTransformCorners[i]);
                if (screenBounds.Contains(screenSpaceCorner))
                {
                    ++visibleCorners;
                }
            }

            return visibleCorners;
        }
    }
}
