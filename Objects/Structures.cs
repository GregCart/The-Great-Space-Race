﻿using Microsoft.Xna.Framework.Input;
using The_Great_Space_Race;


namespace Objects
{
    public interface IModelCollision
    {
        
    }

    public struct RingPassed : IModelCollision
    {
        public Ring ring;
    }

    public struct InputEvent
    {
        public EventType type;
        public Keys key;
        public MouseState mouseState;
    }

    public enum EventType
    {
        Mouse_Down,
        Key_Down,
        Model_Collision,
        num_types_event_type
    }

    //from https://gamedev.stackexchange.com/questions/109516/xna-get-model-height-in-location
    public class CollisionTriangle
    {
        public Microsoft.Xna.Framework.Vector3[] v;
        public CollisionTriangle()
        {
            v = new Microsoft.Xna.Framework.Vector3[3];
        }
    }
}
