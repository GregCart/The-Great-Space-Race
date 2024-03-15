using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;

namespace Objects
{
    public class Ring : GameComponent, IObservable<IModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public EntityModel em;

        private List<IObserver<IModelCollision>> Observers;
        private Matrix WorldTransform;


        public Ring(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            hasPassed = false;

            Observers = new List<IObserver<IModelCollision>>();

            em = new EntityModel("RingLampV3_FullRing_100_Halo", new Matrix().toBEPU(), .2f, this.Game);

            Game.Components.Add(em);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void SetUp(RingType type, Vector3 location, Vector3 rotation) 
        {
            this.type = type;
        }

        public IDisposable Subscribe(IObserver<IModelCollision> observer)
        {
            Observers.Add(observer);

            return this;
        }
    }

    public enum RingType
    {
        Normal,
        Checkpoint,
        HalfWay,
        Finnish,
        num_types_ring_type
    }
}
