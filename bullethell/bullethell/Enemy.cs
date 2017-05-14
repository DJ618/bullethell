using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace bullethell
{
    class Enemy : GameObject
    {
        public EventHandler OnSpawnHealthPack = delegate { };
        public bool IsBoss { get; set; }
        public int FireType { get; set; }
        private List<List<FirePattern>> firePatterns = new List<List<FirePattern>>();
        private int firePatternIndex = 0;
        private int firePatternSetIndex = 0;
        private Timer nextFirePatternSetTimer = null;
        private List<int> firePatternDurations = new List<int>();
        public List<int> FirePatternDurations { get { return firePatternDurations; } set
        {
            firePatternDurations = value;
        } }
        private int durationIndex = 0;

        public List<List<FirePattern>> FirePatterns
        {
            get { return firePatterns; }
            set
            {
                firePatterns = value;
            }
        }

        public MovementPattern MovementPattern { get; set; }
        private Timer spawnTimer;
        public EventHandler OnSpawn = delegate { };

        public Enemy(Texture2D texture, int x, int y, int spawnTime)
            : base(texture, x, y)
        {
            FireType = 3;
            Speed = 1.0f;
            XDir = 0;
            YDir = 0;
            spawnTimer = new Timer(spawnTime);
            spawnTimer.Elapsed += Spawn;
        }

        public override void Fire()
        {
            if (FirePatterns.Count > 0)
            {
                if (FirePatterns[firePatternSetIndex][firePatternIndex % FirePatterns[firePatternSetIndex].Count] != null)
                    FirePatterns[firePatternSetIndex][firePatternIndex % FirePatterns[firePatternSetIndex].Count].Fire((int)XPos, (int)YPos);
                firePatternIndex++;
            }
        }

        private void SpawnProjectilesHandler(object sender, FirePatternEventArgs args)
        {
            SpawnProjectiles(args.Projectiles);
        }

        public void StartTimer()
        {
            spawnTimer.Start();
            if (firePatternDurations.Count > 0)
            {
                nextFirePatternSetTimer = new Timer(firePatternDurations[0]);
                nextFirePatternSetTimer.Elapsed += OnDurationChange;
                nextFirePatternSetTimer.Start();
            }
                
        }

        public void StopTimer()
        {
            spawnTimer.Stop();
            nextFirePatternSetTimer?.Stop();
        }

        public void ContinueTimer()
        {
            spawnTimer.Start();
            nextFirePatternSetTimer?.Start();
        }

        private void Spawn(object sender, EventArgs args)
        {
            spawnTimer.Stop();
            OnSpawn.Invoke(this, null);
        }

        public override void Move()
        {
            MovementPattern.UpdateDirection(this);
            base.Move();
        }

        private void SendXAndY(object sender, EventArgs args)
        {
            for (int i = 0; i < FirePatterns[firePatternSetIndex].Count; i++)
            {
                if (firePatterns[firePatternSetIndex][i] != null)
                {
                    firePatterns[firePatternSetIndex][i].X = (int)XPos;
                    firePatterns[firePatternSetIndex][i].Y = (int)YPos;
                }
            }
            
        }

        public override void Destroy()
        {
            int shouldDrop = Game1.Rand.Next(20);
            Debug.WriteLine("shouldDrop: " + shouldDrop);
            if (nextFirePatternSetTimer != null)
                nextFirePatternSetTimer.Stop();
            for (int i = 0; i < firePatterns.Count; i++)
            {
                if (firePatterns[i] != null)
                {
                    for (int a = 0; a < firePatterns[i].Count; a++)
                    {
                        if (firePatterns[i][a] != null)
                            firePatterns[i][a].Stop = true;
                    }
                }
            }

            if (shouldDrop == 1)
            {
                OnSpawnHealthPack.Invoke(this, null);
            }

            base.Destroy();
        }

        public override bool Intersects(Rectangle rect)
        {
            if (IsInBounds(this))
                return base.Intersects(rect);
            return false;
        }

        public bool IsInBounds(GameObject gameObject)
        {
            return gameObject.XPos + gameObject.Texture.Width > 0 && gameObject.YPos + gameObject.Texture.Height > 0 && gameObject.XPos < Settings.GlobalDisplayWidth && gameObject.YPos < Settings.GlobalDisplayHeight;
        }

        public void AddFirePatterns(List<List<FirePattern>> firePatterns)
        {
            foreach (var firePatternSet in firePatterns)
            {
                this.firePatterns.Add(new List<FirePattern>());
                foreach (var firePattern in firePatternSet)
                {
                    AddFirePattern(firePattern);
                }
            }
        }

        public void AddFirePattern(FirePattern firePattern)
        {
            firePatterns[firePatterns.Count - 1].Add(firePattern);
            firePattern.OnSpawnProjectiles += SpawnProjectilesHandler;
            firePattern.RequestNewXAndY += SendXAndY;
        }

        private void OnDurationChange(object sender, ElapsedEventArgs args)
        {
            nextFirePatternSetTimer.Stop();
            firePatternSetIndex++;
            firePatternIndex = 0;
            if (++durationIndex < firePatternDurations.Count)
            {
                nextFirePatternSetTimer = new Timer(firePatternDurations[durationIndex]);
                nextFirePatternSetTimer.Elapsed += OnDurationChange;
                nextFirePatternSetTimer.Start();
            }
                
        }
    }
}