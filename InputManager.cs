using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MathTest;

public enum MouseButton
{
    Left, Middle, Right
}


public static class InputManager
{
    public static Keys UpKey = Keys.W;
    public static Keys DownKey = Keys.S;
    public static Keys RightKey = Keys.D;
    public static Keys LeftKey = Keys.A;


    public static KeyboardState KeyboardState { get; private set; }
    public static KeyboardState KeyboardStateOld { get; private set; }

    public static MouseState MouseState { get; private set; }
    public static MouseState MouseStateOld { get; private set; }

    public static Vector2 MovementVelocityOld = Vector2.Zero;
    public static Vector2 MovementVelocity = Vector2.Zero;
    public static Vector2 OneHitMovementVelocity = Vector2.Zero;

    public static int UpHeldDuration { get; private set; } = 0;
    public static int DownHeldDuration { get; private set; } = 0;
    public static int LeftHeldDuration { get; private set; } = 0;
    public static int RightHeldDuration { get; private set; } = 0;


    private static int lastScrollWheelValue = 0;


    public static void Update()
    {

        KeyboardStateOld = KeyboardState;
        KeyboardState = Keyboard.GetState();

        MouseStateOld = MouseState;
        MouseState = Mouse.GetState();


        MovementVelocity = Vector2.Zero;
        OneHitMovementVelocity = Vector2.Zero;

        MovementVelocityOld = MovementVelocity;

        if (KeyboardState.IsKeyDown(RightKey))
        {
            MovementVelocity.X++;

            if (KeyboardStateOld.IsKeyUp(RightKey))
                OneHitMovementVelocity.X++;
        }
        if (KeyboardState.IsKeyDown(LeftKey))
        {
            MovementVelocity.X--;

            if (KeyboardStateOld.IsKeyUp(LeftKey))
                OneHitMovementVelocity.X--;
        }
        if (KeyboardState.IsKeyDown(DownKey))
        {
            MovementVelocity.Y++;

            if (KeyboardStateOld.IsKeyUp(DownKey))
                OneHitMovementVelocity.Y++;
        }
        if (KeyboardState.IsKeyDown(UpKey))
        {
            MovementVelocity.Y--;

            if (KeyboardStateOld.IsKeyUp(UpKey))
                OneHitMovementVelocity.Y--;
        }

        if (MovementVelocity != Vector2.Zero)
            MovementVelocity = Vector2.Normalize(MovementVelocity);

        GetOverworldMovement();


        if (MouseState.ScrollWheelValue > lastScrollWheelValue)
        {
            ScrollUpEvent?.Invoke(null, EventArgs.Empty);
            lastScrollWheelValue = MouseState.ScrollWheelValue;
        }
        else if (MouseState.ScrollWheelValue < lastScrollWheelValue)
        {
            ScrollDownEvent?.Invoke(null, EventArgs.Empty);
            lastScrollWheelValue = MouseState.ScrollWheelValue;
        }
    }


    private static void GetOverworldMovement()
    {
        if (KeyboardState.IsKeyUp(Keys.Up))
            UpHeldDuration = 0;
        else
            UpHeldDuration++;

        if (KeyboardState.IsKeyUp(Keys.Down))
            DownHeldDuration = 0;
        else
            DownHeldDuration++;

        if (KeyboardState.IsKeyUp(Keys.Left))
            LeftHeldDuration = 0;
        else
            LeftHeldDuration++;

        if (KeyboardState.IsKeyUp(Keys.Right))
            RightHeldDuration = 0;
        else
            RightHeldDuration++;
    }

    public static bool Fullscreen => GetKeyDown(Keys.F11);

    public static bool GetKeyDown(Keys key) => KeyboardState.IsKeyDown(key) && KeyboardStateOld.IsKeyUp(key);
    public static bool GetKeyUp(Keys key) => KeyboardState.IsKeyUp(key) && KeyboardStateOld.IsKeyDown(key);

    public static bool GetMouseButtonDown(MouseButton button) => button switch
    {
        MouseButton.Right => MouseState.RightButton == ButtonState.Pressed && MouseStateOld.RightButton == ButtonState.Released,
        MouseButton.Middle => MouseState.MiddleButton == ButtonState.Pressed && MouseStateOld.MiddleButton == ButtonState.Released,
        _ => MouseState.LeftButton == ButtonState.Pressed && MouseStateOld.LeftButton == ButtonState.Released
    };

    public static bool GetMouseButtonUp(MouseButton button) => button switch
    {
        MouseButton.Right => MouseState.RightButton == ButtonState.Released && MouseStateOld.RightButton == ButtonState.Pressed,
        MouseButton.Middle => MouseState.MiddleButton == ButtonState.Released && MouseStateOld.MiddleButton == ButtonState.Pressed,
        _ => MouseState.LeftButton == ButtonState.Released && MouseStateOld.LeftButton == ButtonState.Pressed
    };

    public static event EventHandler ScrollUpEvent;
    public static event EventHandler ScrollDownEvent;
}
