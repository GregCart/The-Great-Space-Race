using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;

namespace Objects
{
    public class Ring : GameComponent, IObservable<ModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public EntityModel em;


        private List<IObserver<ModelCollision>> Observers;


        public Ring(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            hasPassed = false;

            Observers = new List<IObserver<ModelCollision>>();

            em = new EntityModel("RingLampV3_FullRing_100_Halo", new Matrix().toBEPU(), this.Game);

            base.Initialize();

            Game.Components.Add(em);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void SetUp(RingType type) 
        {
            this.type = type;
        }

        public IDisposable Subscribe(IObserver<ModelCollision> observer)
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
