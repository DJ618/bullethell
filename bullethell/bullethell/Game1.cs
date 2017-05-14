using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; //used for Convert and Math
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

//for debugging

namespace bullethell
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private enum GameMode { MainMenu, Game, ExitMenu }
        public static Random Rand = new Random();
        private Song backgroundMusic;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Background backGround;
        Background backGround2;
        Settings settings;
        Stats gameStats;
        Player player;
        ConfigParser configParser;
        EnemyManager enemyManager;
        CollisionDetector collisionDetector;
        ProjectileManager projectileManager;
        Menu mainMenu;
        List<ExplodingProjectile> explodingProjectiles = new List<ExplodingProjectile>(50);
        public static ContentManager StaticContentManager;
        private GraphicsDrawer drawer;
        private bool canChangeSpeed = true;
        private bool showMenu = true;
        private GameMode gameMode = GameMode.MainMenu;
        private bool gameStarted = false;
        private bool gameOver = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //settings = new Settings(graphics); //will replace with stuff from config
            gameStats = new Stats();

            projectileManager = new ProjectileManager();
            projectileManager.OnProjectileExplode += AddExplodingProjectile;
            configParser = new ConfigParser("normal.xml", graphics);
            settings = configParser.ParseSettings();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //make borderless window that matches size of screen

            base.Initialize();
            //settings = configParser.ParseSettings();
            StaticContentManager = Content;
            //Window.IsBorderless = true;
            settings.MatchWindowSize();
            settings.Apply();
            mainMenu.LoadMenuOptions();
            TextureFactory.LoadTextures();
            enemyManager = configParser.ParseEnemies();
            backGround = new Background(TextureFactory.GetTexture("Space003"), new Rectangle(0, 0,
                                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            backGround2 = new Background(TextureFactory.GetTexture("Space003"), new Rectangle(0, -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
                                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));

            enemyManager.OnSpawnProjectiles += SpawnEnemyProjectiles;
            enemyManager.OnSpawnEnemy += AddEnemyToCollisionDetector;
            enemyManager.OnGameOver += HandleGameOver;
            enemyManager.OnSpawnHealthPack += HandleSpawnHealthPack;
        }

        private void HandleSpawnHealthPack(object sender, EventArgs args)
        {
            GameObject obj = (GameObject) sender;
            var healthPack = CreateHealthPack((int)obj.XPos, (int)obj.YPos);
            List<Projectile> healthPacks = new List<Projectile>(1);
            healthPacks.Add(healthPack);
            projectileManager.AddHealthPacks(healthPacks);
            collisionDetector.AddHealthPacks(healthPacks);
        }

        private Projectile CreateHealthPack(int x, int y)
        {
            return new Projectile(TextureFactory.GetTexture("healthpack"), x, y, new Vector2(0, 1), false)
            {
                Speed = 5,
                Scale = .0001f * Settings.GlobalDisplayWidth
            };
        }

        private void HandlePlayerSpeedChange(object sender, EventArgs args)
        {
            player.Speed = (sender as Settings).PlayerSpeedFactor * settings.DisplayWidth;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawer = new GraphicsDrawer(spriteBatch);

            //configParser = new ConfigParser("normal.xml", graphics);
            //settings = configParser.ParseSettings(); //currently useless other than skipping to game section

            //stats
            gameStats.scoreFont = Content.Load<SpriteFont>("score");

            try
            {
                backgroundMusic = Content.Load<Song>("interstellar");
                MediaPlayer.Play(backgroundMusic);
            }
            catch (Exception e)
            {
            }

            SetupPlayer();
            SetupCollisionDetector(); //must be called after player is setup
            collisionDetector.OnEnemyHit += EnemyHitEventHandler;

            SetupMainMenu();
        }

        private void EnemyHitEventHandler(object enemyHit, EventArgs args)
        {
            gameStats.adjustPlayerScore(100);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!gameOver)
            {
                switch (gameMode)
                {
                    case GameMode.MainMenu:
                        mainMenu.DetectMenuInteraction();
                        break;
                    case GameMode.Game:
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                            Keyboard.GetState().IsKeyDown(Keys.Escape))
                            Pause();

                        UpdateObjects();
                        break;
                    case GameMode.ExitMenu:
                        break;
                }
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin();

            if (!gameOver)
            {
                drawer.DrawScrollingBackground(backGround);
                drawer.DrawScrollingBackground(backGround2);
                DrawGame(); //This way we can see the game even when paused!

                switch (gameMode)
                {
                    case GameMode.MainMenu:
                        mainMenu.DrawMenu(spriteBatch);
                        break;
                    case GameMode.ExitMenu:
                        drawer.DrawEndGame(gameStats);
                        break;
                }
            }
            else
            {
                GameOverScreen(spriteBatch);
            }
            
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            if (gameStarted)
            {
                drawer.DrawPlayer(player);
                drawer.DrawEnemies(enemyManager.SpawnedEnemies);
                drawer.DrawProjectiles(projectileManager.EnemyProjectiles);
                drawer.DrawProjectiles(projectileManager.FriendlyProjectiles);
                drawer.DrawProjectiles(projectileManager.HealthPacks);
                drawer.DrawStats(gameStats, player.Health);
                drawer.DrawExplosions(explodingProjectiles);
            }
        }

        private void UpdateObjects()
        {
            RemoveDoneExplosions();
            backGround.Update();
            backGround2.Update();
            player.Move();
            enemyManager.MoveAllEnemies();
            if (!player.hitCD)
            {
                projectileManager.MoveAllProjectiles();
                collisionDetector.ScanForCollisions();
            }
            else
            {
                projectileManager.RemoveAllProjectiles();
            }
            HandleChangeSpeed();
        }

        private void CreateFriendlyProjectiles(object sender, FirePatternEventArgs args)
        {
            var projectileList = args.Projectiles;
            projectileManager.AddFriendlyProjectiles(projectileList);
            collisionDetector.AddFriendlyProjectiles(projectileList);
        }

        private void SpawnEnemyProjectiles(object sender, FirePatternEventArgs args)
        {
            var projectileList = args.Projectiles;
            projectileManager.AddEnemyProjectiles(projectileList);
            collisionDetector.AddEnemyProjectiles(projectileList);
        }

        private void AddEnemyToCollisionDetector(object sender, GameObject enemy)
        {
            collisionDetector.AddEnemy(enemy);
        }

        private void SetupPlayer()
        {
            var t = Content.Load<Texture2D>(@"Spaceship");
            var p = Content.Load<Texture2D>(@"playerBullet");
            float pScale = .05f * settings.DisplayWidth / t.Width;
            //make sure player is scaled to 5% of the screen width;
            player = new Player(t, p, Convert.ToInt32(settings.DisplayWidth / 2 - .5 * t.Width * pScale),
                settings.DisplayHeight - t.Height - 50)
            {
                Scale = pScale,
                Speed = settings.PlayerSpeedFactor * settings.DisplayWidth, //allow player to move 0.75% of the width per frame (that speed applied to any direction tho)
                BottomBound = settings.DisplayHeight,
                RightBound = settings.DisplayWidth
            };
            
            player.OnDestroy += PlayerKilledHandler;
            player.OnSpawnProjectiles += CreateFriendlyProjectiles;
            settings.OnPlayerSpeedChange += HandlePlayerSpeedChange;
        }

        private void AddExplodingProjectile(object sender, EventArgs args)
        {
            explodingProjectiles.Add(new ExplodingProjectile()
            {
                Projectile = (Projectile) sender
            });
        }

        private void SetupCollisionDetector()
        {
            collisionDetector = new CollisionDetector(player);
        }

        //the player was killed
        private void PlayerKilledHandler(object sender, EventArgs e)
        {
            //if the player was hit, and we are out of lives: end the game
            if(gameStats.adjustPlayerHealth(-1) < 0)
            {
                //check if we have a new high score
                if(gameStats.playerScore > gameStats.highScore)
                {
                    //update the new highscore
                    gameStats.highScore = gameStats.playerScore;
                }

                gameOver = true;
            }
        }

        private void RemoveDoneExplosions()
        {
            for (int i = 0; i < explodingProjectiles.Count; i++)
            {
                if (explodingProjectiles[i] != null)
                {
                    if (explodingProjectiles[i].Count <= 0)
                        explodingProjectiles.RemoveAt(i);
                }
            }
        }

        private void HandleChangeSpeed()
        {
            if (Keyboard.GetState().IsKeyUp(Keys.LeftControl) && Keyboard.GetState().IsKeyUp(Keys.RightControl) && Keyboard.GetState().IsKeyUp(Keys.P))
            {
                canChangeSpeed = true;
            } else if (canChangeSpeed && (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl)) && Keyboard.GetState().IsKeyDown(Keys.P))
            {
                settings.ChangePlayerSpeed();
                canChangeSpeed = false;
            }
        }

        private void SetupMainMenu()
        {
            mainMenu = new Menu();
            mainMenu.OnSelectPlayGame += PlayGame;
            mainMenu.OnSelectExit += Quit;
        }

        

        private void PlayGame(object sender, EventArgs args)
        {
            if (gameMode != GameMode.Game)
            {
                if (!gameStarted)
                {
                    gameStarted = true;
                    enemyManager.Start();
                }
                player.Enabled = true;
                gameMode = GameMode.Game;
            }
        }

        private void Pause()
        {
            player.Enabled = false;
            gameMode = GameMode.MainMenu;
        }

        private void Quit(object sender, EventArgs args)
        {
            Exit();
        }

        private void GameOverScreen(SpriteBatch spriteBatch)
        {
            GameOver gameOver = new GameOver();
            gameOver.DrawGameOver(spriteBatch, gameStats.playerScore);
        }

        private void HandleGameOver(object sender, EventArgs args)
        {
            gameOver = true;
        }
    }
}