using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace bullethell
{
    class EnemyBuilder
    {
        private XmlTextReader reader;

        public EnemyBuilder(XmlTextReader reader)
        {
            this.reader = reader;
        }

        public Enemy ConstructEnemy()
        {
            int spawnTime = 0;
            int health = 1;
            List<List<string>> fireStrings = new List<List<string>> {new List<string>()};
            string movementStr = "";
            List<List<string>> projectileStrings = new List<List<string>> {new List<string>()};
            string xStr = "";
            string yStr = "";
            string textureStr = "ghost";
            List<int> durations = new List<int>();
            while (reader.Name != "enemy")
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "startTime":
                            spawnTime = int.Parse(GetValue());
                            break;
                        case "health":
                            health = int.Parse(GetValue());
                            break;
                        case "fire":
                            fireStrings[fireStrings.Count - 1].Add(GetValue());
                            break;
                        case "movement":
                            movementStr = GetValue();
                            break;
                        case "projectile":
                            projectileStrings[projectileStrings.Count - 1].Add(GetValue());
                            break;
                        case "x":
                            xStr = GetValue();
                            break;
                        case "y":
                            yStr = GetValue();
                            break;
                        case "texture":
                            textureStr = GetValue();
                            break;
                        case "duration":
                            durations.Add(int.Parse(GetValue()));
                            fireStrings.Add(new List<string>());
                            projectileStrings.Add(new List<string>());
                            break;
                    }
                }
                if (reader.Name == "enemy")
                    break;
                reader.Read();
            }
            return MakeEnemy(spawnTime, health, fireStrings, movementStr, projectileStrings, xStr, yStr, textureStr, durations);
        }

        private float ParseX(string xStr, Texture2D texture, float scale)
        {
            double perc;

            if (!double.TryParse(xStr, out perc))
            {
                if (xStr == "left")
                    return 0 - texture.Width * scale;
                if (xStr == "right")
                    return Settings.GlobalDisplayWidth;

                return 0.0f;
            }
            return (float) (perc * Settings.GlobalDisplayWidth);
        }

        private float ParseY(string yStr, Texture2D texture, float scale)
        {
            double perc;

            if (!double.TryParse(yStr, out perc))
            {
                if (yStr == "above")
                    return 0 - texture.Height * scale;
                if (yStr == "below")
                    return Settings.GlobalDisplayHeight;

                return 0.0f;
            }
            return (float) (perc * Settings.GlobalDisplayHeight);
        }

        private Enemy MakeEnemy(int spawnTime, int health, List<List<string>> fireStrings, string movementStr, List<List<string>> projectileStrings, string xStr,
            string yStr, string textureStr, List<int> durations)
        {
            MovementPattern movementPattern = ConstructMovementPattern(movementStr);
            List<List<ProjectileType>> pTypes = new List<List<ProjectileType>>();
            foreach (var pSet in projectileStrings)
            {
                pTypes.Add(new List<ProjectileType>());
                foreach (var str in pSet)
                {
                    pTypes[pTypes.Count - 1].Add(GetProjectileType(str));
                }
            }
            List<List<FirePattern>> firePatterns = new List<List<FirePattern>>();
            for (int i = 0; i < fireStrings.Count; i++)
            {
                firePatterns.Add(new List<FirePattern>());
                for (int a = 0; a < fireStrings[i].Count; a++)
                {
                    firePatterns[i].Add(ConstructFirePattern(fireStrings[i][a], pTypes[i][a]));
                }
                
            }
            var texture = GetEnemyTexture(textureStr);
            float x = ParseX(xStr, texture, .025f * Settings.GlobalDisplayWidth / texture.Width);
            float y = ParseY(yStr, texture, .025f * Settings.GlobalDisplayWidth / texture.Width);
            Enemy enemy = new Enemy(texture, (int) x, (int) y, spawnTime)
            {
                MovementPattern = movementPattern,
                Scale = .025f * Settings.GlobalDisplayWidth / texture.Width,
                Health = health,
                FirePatternDurations = durations
            };
            enemy.AddFirePatterns(firePatterns);
            return enemy;
        }

        private ProjectileType GetProjectileType(string selection)
        {
            switch (selection)
            {
                case "standard":
                    return ProjectileType.Standard;
                case "orange":
                    return ProjectileType.Orange;
                case "rope":
                    return ProjectileType.Rope;
                case "lazer":
                    return ProjectileType.Lazer;
                case "pulsating":
                    return ProjectileType.Pulsating;
                case "spinning":
                    return ProjectileType.Spinning;
                case "bluefuzzystandard":
                    return ProjectileType.BlueFuzzyStandard;
                default:
                    return ProjectileType.Standard;
            }
        }

        private Texture2D GetEnemyTexture(string selection)
        {
            switch (selection)
            {
                case "ghost":
                    return TextureFactory.GetTexture(selection);
                case "monster":
                    return TextureFactory.GetTexture(selection);
            }
            return TextureFactory.GetTexture("ghost");
        }

        private MovementPattern ConstructMovementPattern(string selection)
        {
            switch (selection)
            {
                case "dtl":
                    return new CurveDownThenLeft();
                case "dtr":
                    return new CurveDownThenRight();
                case "staggared":
                    return new StaggeredMovement();
                case "random":
                    return new RandomMovement();
				case "paused":
		            return new PausedMovement();
                case "straightdown":
                    return new StraightDown();
                case "minibossleft":
                    return new MiniBossMovementLeft();
                case "minibossright":
                    return new MiniBossMovementRight();
                default:
                    return new StaggeredMovement();
            }
        }

        private FirePattern ConstructFirePattern(string selection, ProjectileType projectileType)
        {
            var texture = TextureFactory.GetTexture("enemyBullet");
            switch (selection)
            {
                case "pulsatingblast":
                    return new PulsatingBlast(projectileType);
                case "spinningblast":
                    return new SpinningCircleBlast(projectileType);
                case "666":
                    return new Fire666(projectileType);
                case "123":
                    return new Fire123(projectileType);
                case "circle":
                    return new FireCircle(projectileType);
                case "rope":
                    return new FireRope(projectileType);
                case "doublerope":
                    return new FireDoubleRope(projectileType);
                case "triplerope":
                    return new FireTripleRope(projectileType);
				case "mcflurry":
		            return new FireMcFlurry(projectileType);
                case "none":
                    return new FireNone(projectileType);
                case "lazers":
                    return new FireLazers(projectileType);
                case "normal":
                    return new FireNormal(projectileType);
                default:
                    return new FireNone(projectileType);
            }
        }

        private string GetValue()
        {
            while (reader.Value == "" || string.IsNullOrWhiteSpace(reader.Value))
            {
                reader.Read();
            }
            return reader.Value;
        }
    }
}