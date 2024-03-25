using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;

namespace Objects
{
    public class Ring : GameComponent, IObservable<RingPassed>, IObserver<ModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public EntityModel em;
        public bool isNextRing;
        public List<Entity> colliders;

        private List<IObserver<RingPassed>> Observers;


        public Ring(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            hasPassed = false;
            isNextRing = false;

            Observers = new List<IObserver<RingPassed>>();
            colliders = new List<Entity>();

            em = new EntityModel("RingLampV3_FullRing_100_Halo", Matrix.CreateFromYawPitchRoll(0f, MathHelper.ToRadians(90f), 0f).toBEPU(), .08f, new BEPUutilities.Vector3(15, 1, 15), this.Game);
            em.Subscribe(this);

            Game.Components.Add(em);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (isNextRing)
            {
                this.em.DrawDuplicateModel(1.5f);
            }

            base.Update(gameTime);
        }

        public void SetUp(RingType type, Vector3 location, Vector3 rotation) 
        {
            this.type = type;
            this.em.Initialize();
            this.em.Transform *= Matrix.CreateTranslation(location).toBEPU() * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z).toBEPU();
            this.em.UpdateContent();
            if (type == RingType.Start)
            {
                this.isNextRing = true;
            }
            for (int i = 0; i < 6; i++)
            {
                Entity e = new Cylinder(this.em.entity.WorldTransform.Translation, 5, 1);
                colliders.Add(e);
            }
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

            if (!this.hasPassed || this.type == RingType.Finnish)
            {
                this.hasPassed = true;
                this.isNextRing = false;
                RingPassed pass = new RingPassed(value);
                pass.ring = this;
                pass.ringType = this.type;

                foreach (IObserver<RingPassed> observer in Observers)
                {
                    observer.OnNext(pass);
                }
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
