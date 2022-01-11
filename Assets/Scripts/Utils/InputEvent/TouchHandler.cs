using UnityEngine;

namespace KKoding92.Util
{
    public class TouchHandler : IInputHandlerBase
    {
        bool IInputHandlerBase.isInputDown => Input.GetTouch(0).phase == TouchPhase.Began;
        bool IInputHandlerBase.isInputUp => Input.GetTouch(0).phase == TouchPhase.Ended;

        Vector2 IInputHandlerBase.inputPosition => Input.GetTouch(0).position;
    }
}
