using Microsoft.Xna.Framework.Input;
using The_Great_Space_Race;


namespace Objects
{
    public struct ModelCollision
    {
        
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
}
