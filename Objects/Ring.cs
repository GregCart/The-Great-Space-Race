﻿using Microsoft.Xna.Framework;
using The_Great_Space_Race;
using System;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Ring : DrawableGameComponent, IObservable<RingPassed>, IObserver<ModelCollision>
    {
        public static int debugId = 0;

        public bool hasPassed;
        public RingType type;
        public EntityModel em;
        public bool isNextRing;
        public List<Entity> colliders;

        private List<IObserver<RingPassed>> Observers;
        private Model debug;
        private Vector3 entityScale;


        public Ring(Game1 game) : base(game)
        {
            this.entityScale = new Vector3(1, 5, 1);
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

        protected override void LoadContent()
        {
            this.debug = Game.Content.Load<Model>("Models/DefaultCube_1x1x1");


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.colliders[0].WorldTransform = em.entity.WorldTransform;
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
            //Entity e = new Cylinder(this.em.Transform.Translation, entityScale.Y, entityScale.Z/2);
            Entity e = new Box(this.em.Transform.Translation, entityScale.X, entityScale.Y, entityScale.Z);
            colliders.Add(e);
            /*for (int i = 0; i < 6; i++)
            {
                Entity e = new Cylinder(this.em.entity.WorldTransform.Translation, 5, 1);
                colliders.Add(e);
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            Microsoft.Xna.Framework.Matrix[] transforms = new Microsoft.Xna.Framework.Matrix[debug.Bones.Count];
            debug.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in debug.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //                              POSITION                        SCALE                                               ???
                    effect.World = transforms[mesh.ParentBone.Index] * Microsoft.Xna.Framework.Matrix.CreateScale(entityScale) * colliders[debugId].WorldTransform.toXNA();
                    //effect.World = worldMatrix.toXNA();
                    // camera effects
                    effect.View = ((Ship)Game.Services.GetService(typeof(Ship))).Camera.ViewMatrix;
                    effect.Projection = ((Ship)Game.Services.GetService(typeof(Ship))).Camera.ProjectionMatrix;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
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
            if (value.type != EventType.Collision || value.obj.Shape.GetType().Name != "CylinderShape")
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
