﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using The_Great_Space_Race;
using System;
using System.Linq;
using System.Collections.Generic;

// from https://stackoverflow.com/questions/75449753/c-sharp-monogame-handling-mouse
namespace Objects
{
    public class InputManager : GameComponent, IObservable<InputEvent>
    {
        public static bool LeftClicked = false;
        public static bool LeftWasClicked = false;
        public static bool ButtonDown = false;
        public static bool count = true;
        public static Vector2 MDPos = Vector2.Zero;
        public static float Time = 0.0f;
        public static float DownTime = 0.0f;
        public static InputManager Instance
        {
            get { return instance; }
        }

        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;
        private static MouseState ms = new MouseState(), oms;
        private static List<IObserver<InputEvent>> Observers;
        private static InputManager instance;

        public InputManager(Game1 game) : base(game)
        {
            instance = this;
            Observers = new List<IObserver<InputEvent>>();
        }

        public static void Reset()
        {
            MDPos = Vector2.Zero;
            Time = 0.0f;
            DownTime = 0.0f;
            LeftClicked = false;
            LeftWasClicked = false;
            count = true;
        }

        public override void Update(GameTime gameTime)
        {
            oms = ms;
            previousKeyState = currentKeyState;
            ms = Mouse.GetState();
            currentKeyState = Keyboard.GetState();
            LeftWasClicked = LeftClicked;
            LeftClicked = ms.LeftButton == ButtonState.Pressed;
            // true On left release like Windows buttons
            //Debug.WriteLine("test " + KeyPressed(Keys.Space));
            if (currentKeyState.GetPressedKeyCount() > 0)
            {
                ButtonDown = true;
                foreach (Keys k in currentKeyState.GetPressedKeys())
                {
                    instance.Notify(new InputEvent { type = EventType.Key_Down, key = k });
                }
            }
            if (LeftClicked)
            {
                instance.Notify(new InputEvent { type = EventType.Mouse_Down, mouseState = ms });
            }
            else if (LeftWasClicked)
            {

            }
            else if (!ButtonDown)
            {
                Reset();
            } else if (ButtonDown)
            {
                
            }
        }

        public static bool Hover(Rectangle r)
        {
            return r.Contains(new Vector2(ms.X, ms.Y));
        }

        public static bool IsDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool WasDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return !currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key);
        }

        public IDisposable Subscribe(IObserver<InputEvent> observer)
        {
            Observers.Add(observer); 
            
            return this;
        }

        public void Notify(InputEvent iEvent)
        {
            foreach (IObserver<InputEvent> observer in Observers)
            {
                observer.OnNext(iEvent);
            }
        }
    }
}
