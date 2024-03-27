using BEPUphysics;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using The_Great_Space_Race;
using The_Great_Space_Race.Objects;


namespace Objects
{
    public class Course : DrawableGameComponent, IObserver<ModelCollision>, IObservable<bool>
    {
        public string Name { get; set; }
        public Space space;
        public SkyBox skyBox;
        public Ring[] Rings;
        public SpriteFont font;
        public bool IsOrdered;
        public bool HasFinnished;
        public double timer;
        public int RingsHit;

        private static List<IObserver<bool>> Observers;

        private Ship ship;
        private int score;
        private bool started;


        public Course(Game1 game) : base(game)
        {
            this.IsOrdered = false;
            this.HasFinnished = false;
            this.started = false;
            this.score = 0;
        }

        public override void Initialize()
        {
            if (space == null)
            {
                space = new Space();
            }

            this.Rings = new List<Ring>().ToArray();

            base.Initialize();

            ship = Game.Components[0] as Ship;
            this.space.Add(ship.em.entity);
        }

        public void UpdateContent(bool isOrdered, List<Ring> rings, SkyBox box) 
        //public void UpdateContent(bool isOrdered, List<Ring> rings)
        {
            this.IsOrdered = isOrdered;
            this.Rings = rings.ToArray();
            //this.skyBox = box;

            foreach (Ring r in Rings)
            {
                Game.Components.Add(r);
                foreach (Entity e in r.colliders)
                {
                    this.space.Add(e);
                }
            }
            box.Initialize();
            Game.Components.Add(box);

            this.space.Add(ship.collider);

            this.LoadContent();
        }

        public void LoadContent()
        {
            foreach (Ring r in Rings)
            {
                space.Add(r.em.entity);
                r.Subscribe(this);
            }

            if (this.font == null)
            {
                this.font = Game.Content.Load<SpriteFont>("Textures/UI-font");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!started || !HasFinnished)
            {
                space.Update();
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }

            base.Update(gameTime);
        }

        public void OnCompleted()
        {
            //do nothing
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine(error);
        }

        public void OnNext(ModelCollision value)
        {
            if (value.type == EventType.Ring_Passed)
            {
                this.RingsHit++;
                RingPassed rp = (RingPassed)value;
                if (!this.started && rp.ringType == RingType.Start)
                {
                    this.started = true;
                } 
                else if (rp.ringType == RingType.Finnish)
                {
                    this.HasFinnished = true;
                    CalculateScore();
                }

                if (!this.HasFinnished)
                {
                    int ring = Array.FindIndex(this.Rings, obj => obj.Equals(rp.ring));
                    this.Rings[ring + 1].isNextRing = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).DrawString(font, "Time: " + timer / 1000, new Vector2(10, 10), Color.White);
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).DrawString(font, "Rings: " + RingsHit, new Vector2(10, 50), Color.White);

            if (this.HasFinnished)
            {
                ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).DrawString(font, "Your Score:\n" + score, new Vector2(100, 100), Color.White);
            }

            base.Draw(gameTime);
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            Observers.Add(observer);

            return this;
        }

        private void CalculateScore()
        {
            this.score = (int)((1.5 * RingsHit) * (timer * 100));
        }
    }
}
