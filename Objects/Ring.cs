using BEPUphysics.Entities.Prefabs;
using Vector3 = BEPUutilities.Vector3;
using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities;
using BEPUutilities;
using Matrix = BEPUutilities.Matrix;

namespace Objects
{
    public class Ring : DrawableGameComponent, IObservable<ModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public Cylinder shape;
        public Cylinder threshold;
        public Model model;
        public Entity entity;
        public Matrix Transform;

        private List<IObserver<ModelCollision>> Observers;


        public Ring(Game1 game) : base(game)
        {

        }

        public override void Initialize()
        {
            hasPassed = false;

            Observers = new List<IObserver<ModelCollision>>();

            base.Initialize();
        }

        public void SetUp(RingType type) 
        {
            this.type = type;
        }

        protected override void LoadContent()
        {
            shape = new Cylinder(Vector3.Zero, 30, 1);

            base.LoadContent();
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
