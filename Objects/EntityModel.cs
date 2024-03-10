using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Objects;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Matrix = BEPUutilities.Matrix;

namespace The_Great_Space_Race.Objects
{
    public class EntityModel : DrawableGameComponent
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

        public string modelPath;


        /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public EntityModel(string modelPath, Matrix transform, Game game)
            : base(game)
        {
            this.modelPath = modelPath;
            this.Transform = transform;
        }

        public EntityModel(Entity entity, Model model, Matrix transform, Game game)
            : base(game)
        {
            this.entity = entity;
            this.model = model;
            this.Transform = transform;
        }

        protected override void LoadContent()
        {
            if (this.model ==  null)
            {
                this.model = Game.Content.Load<Model>("Models/" + modelPath);
            }

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

        public void UpdateContent()
        {
            this.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //Notice that the entity's worldTransform property is being accessed here.
            //This property is returns a rigid transformation representing the orientation
            //and translation of the entity combined.
            //There are a variety of properties available in the entity, try looking around
            //in the list to familiarize yourself with it.
            var worldMatrix = Transform * entity.WorldTransform;


            model.CopyAbsoluteBoneTransformsTo(boneTransforms.toXNA());
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index].toXNA() * worldMatrix.toXNA();
                    effect.View = ((Ship) Game.Components.ElementAt(0)).Camera.ViewMatrix.toXNA();
                    effect.Projection = ((Ship)Game.Components.ElementAt(0)).Camera.ProjectionMatrix.toXNA();
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
