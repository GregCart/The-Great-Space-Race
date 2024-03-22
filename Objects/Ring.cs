using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;

namespace Objects
{
    public class Ring : GameComponent, IObservable<RingPassed>, IObserver<ModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public EntityModel em;

        private List<IObserver<RingPassed>> Observers;
        private Matrix WorldTransform;


        public Ring(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            hasPassed = false;

            Observers = new List<IObserver<RingPassed>>();

            em = new EntityModel("RingLampV3_FullRing_100_Halo", Matrix.CreateFromYawPitchRoll(0f, MathHelper.ToRadians(90f), 0f).toBEPU(), .08f, new BEPUutilities.Vector3(3, 3, 3), this.Game);
            em.Subscribe(this);
            

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
            this.em.Initialize();
            this.em.Transform *= Matrix.CreateTranslation(location).toBEPU() * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z).toBEPU();
            this.em.UpdateContent();
        }

        public IDisposable Subscribe(IObserver<RingPassed> observer)
        {
            Observers.Add(observer);

            return this;
        }

        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnNext(ModelCollision value)
        {
            if (value.type != EventType.Collision)
            {
                return;
            }

            this.hasPassed = true;
            RingPassed pass = new RingPassed(value);
            pass.ring = this.type;
            foreach (IObserver<RingPassed> observer in Observers)
            {

                observer.OnNext(pass);
            }
        }
    }

    public enum RingType
    {
        Start,
        Normal,
        Checkpoint,
        HalfWay,
        Finnish,
        num_types_ring_type
    }
}
