using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionTests;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using System;
using BEPUphysics.CollisionRuleManagement;

using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

using static Objects.Helpers;


namespace Objects
{
    // from https://github.com/bepu/bepuphysics1/blob/master/Documentation/Isolated%20Demos/GettingStartedDemo/EntityModel.cs
    public class EntityModel : DrawableGameComponent, IObservable<ModelCollision>
    {
        /// <summary>
        /// Entity that this model follows.
        /// </summary>
        public Entity entity;
        public Model model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;
        public float Scale = 1.0f;
        public string modelPath;

        private List<CollisionTriangle> triangles;
        private Microsoft.Xna.Framework.Vector3 min;
        private Microsoft.Xna.Framework.Vector3 max;
        public IObserver<ModelCollision> Observer;


        /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public EntityModel(string modelPath, Matrix transform, float scale, Game game)
            : base(game)
        {
            this.modelPath = modelPath;
            this.Transform = transform;
            this.Scale = scale;
        }

        public EntityModel(Entity entity, Model model, Matrix transform, Game game)
            : base(game)
        {
            this.entity = entity;
            this.model = model;
            this.Transform = transform;
        }

        public override void Initialize()
        {
            this.triangles = new List<CollisionTriangle>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (this.model == null)
            {
                this.model = Game.Content.Load<Model>("Models/" + modelPath);
            }
            if (this.entity == null)
            {
                List<CollisionTriangle> tmp;
                Microsoft.Xna.Framework.Vector3 tmpMin;
                Microsoft.Xna.Framework.Vector3 tmpMax;
                foreach (ModelMesh mesh in this.model.Meshes)
                {
                    Matrix transform = CreateTransform(mesh.ParentBone).toBEPU();
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        (tmp, tmpMax, tmpMin) = ExtractMeshPart(meshPart, transform.toXNA());
                        triangles.AddRange(tmp);
                        tmpMax.UpdateMinMax(ref max, ref min);
                        tmpMin.UpdateMinMax(ref max, ref min);
                    }
                }

                this.entity = new Cylinder(this.Transform.Translation, this.max.Y - this.min.Y, (this.max.X - this.min.X) / 2f);
                //From Addison
                //Disable solver to make box generate collision events but no affect physics(like a trigger in unity)
                //More about collision rules: https://github.com/bepu/bepuphysics1/blob/master/Documentation/CollisionRules.md
                this.entity.CollisionInformation.CollisionRules.Personal =
                CollisionRule.NoSolver;
                this.entity.Material.Bounciness = 1;
                //Add collision start listener
                //More about collision events: https://github.com/bepu/bepuphysics1/blob/master/Documentation/CollisionEvents.md
                this.entity.CollisionInformation.Events.ContactCreated += CollisionHappened;
            }
            this.entity.WorldTransform = this.Transform;
            this.entity.BecomeDynamic(1);


            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Notice that the entity's worldTransform property is being accessed here.
            //This property is returns a rigid transformation representing the orientation
            //and translation of the entity combined.
            //There are a variety of properties available in the entity, try looking around
            //in the list to familiarize yourself with it.
            var worldMatrix = entity.WorldTransform;

            Microsoft.Xna.Framework.Matrix[] transforms = new Microsoft.Xna.Framework.Matrix[this.model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            boneTransforms = transforms.toBEPU();
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //                              POSITION                        TRANSLATION             SCALE
                    effect.World = boneTransforms[mesh.ParentBone.Index].toXNA() * worldMatrix.toXNA() * Microsoft.Xna.Framework.Matrix.CreateScale(Scale);
                    //effect.World = worldMatrix.toXNA();
                    // camera effects
                    effect.View = ((Ship) Game.Components.ElementAt(0)).Camera.ViewMatrix;
                    effect.Projection = ((Ship)Game.Components.ElementAt(0)).Camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }

        //Handle collision events from addison
        void CollisionHappened(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            Console.WriteLine("Collision detected.");
            if (Observer != null)
                Observer.OnNext(new ModelCollision { type = EventType.Collision, entity = sender, obj = other, data = contact});
        }

        public IDisposable Subscribe(IObserver<ModelCollision> observer)
        {
            Observer = observer;

            return this;
        }
    }
}
