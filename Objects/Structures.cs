using BEPUphysics.BroadPhaseEntries;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;
using The_Great_Space_Race;


namespace Objects
{
    public class ModelCollision
    {
        public EventType type;
        public BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable entity;
        public Collidable obj;
        public BEPUphysics.CollisionTests.ContactData data;
    }

    public class RingPassed : ModelCollision
    {
        public new EventType type { get; private set; } = EventType.Ring_Passed;
        public RingType ring;

        public RingPassed(ModelCollision other)
        {
            this.type = other.type;
            this.entity = other.entity;
            this.obj = other.obj;
            this.data = other.data;
        }
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
        Collision,
        Ring_Passed,
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
