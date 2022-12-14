using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Dawnfall.Helper
{
    public static class HelperUI
    {
        public static void AddMethodToButton(string buttonName, UnityAction action)
        {
            Button button = HelperUnity.FindComponentOnGameObject<Button>(buttonName);

            if (button != null)
                button.onClick.AddListener(action);
        }

        public static void AddMethodToButton(string buttonName, GameObject parent, UnityAction action)
        {
            Button button = HelperUnity.FindComponentOnChild<Button>(buttonName, parent);

            if (button != null)
                button.onClick.AddListener(action);
        }

        public static void SetCanvasGroup(CanvasGroup canvasGroup, float alpha, bool isInteractable, bool doBlockRaycasts, bool doIgnoreParentGroups)
        {
            canvasGroup.alpha = alpha;
            canvasGroup.interactable = isInteractable;
            canvasGroup.blocksRaycasts = doBlockRaycasts;
            canvasGroup.ignoreParentGroups = doIgnoreParentGroups;
        }
    }
}