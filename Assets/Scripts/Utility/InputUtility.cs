using UnityEngine;

namespace Utility
{
    public static class InputUtility
    {
        public static bool CheckTouchPhase(TouchPhase targetPhase)
        {
            if (Input.touchSupported && Input.touchCount > 0)
                return Input.GetTouch(0).phase == targetPhase;
            return false;
        }
    }
}