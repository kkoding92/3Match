using UnityEngine;

namespace KKoding92.Util
{
    public class MouseHandler : IInputHandlerBase
    {
        bool IInputHandlerBase.isInputDown => Input.GetButtonDown("Fire1");
        bool IInputHandlerBase.isInputUp => Input.GetButtonDown("Fire1");

        Vector2 IInputHandlerBase.inputPosition => Input.mousePosition;
    }
}
