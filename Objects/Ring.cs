﻿using BEPUphysics.Entities.Prefabs;
using Vector3 = BEPUutilities.Vector3;
using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;

namespace Objects
{
    public class Ring : DrawableGameComponent, IObservable<ModelCollision>
    {
        public bool hasPassed;
        public RingType type;
        public Cylinder Shape;
        public Box Insides;

        private List<IObserver<ModelCollision>> Observers;


        public Ring(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            hasPassed = false;

            base.Initialize();
        }

        public void SetUp(Vector2 loc, RingType type) 
        {
            this.type = type;
        }

        protected override void LoadContent()
        {
            Shape = new Cylinder(Vector3.Zero, 30, 1);

            base.LoadContent();
        }

        IDisposable Subscribe(IObserver<ModelCollision> observer)
        {
            throw new NotImplementedException();
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
