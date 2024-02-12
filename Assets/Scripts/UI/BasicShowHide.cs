using UnityEngine;

namespace UI
{
    public static class BasicShowHide
    {
        public static void Show(GameObject toShow)
        {
            toShow.SetActive(true);
        }

        public static void Hide(GameObject toHide)
        {
            toHide.SetActive(false);
        }
    }
}