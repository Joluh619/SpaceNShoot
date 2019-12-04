using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
/*Jorge luis Carvajal Jasso 160108
 UPSLP
 Teoria computacional*/

namespace spaceNShoot
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /*-----------------generalidades------------------------*/
        Texture2D background;
        SpriteFont font;
        Song backgroundMusic;
        SoundEffect gameOverSound;
        int stage = 1;
        bool endGame = false;
        bool retry = false;
        Random random = new Random();


        /*----------------------nave y disparos--------------------------*/
        List<Sprite> _sprites;

        /*--------------------------lista dez UFOS enemigos---------------*/
        List<Enemy> _enemies = new List<Enemy>();
        int spawnedUfos = 0;
        int ufosToSpawn = 3;

        /*--------------------------Meteoritos--------------------------*/
        List<Meteorite> _meteorites = new List<Meteorite>();
        float spawn = 0;

        /*---------------------------Rocas------------------------*/
        List<Rock> _rocks = new List<Rock>();
        float spawnRocks = 0;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            /*---------------carga de texturas---------------------------*/
            var shipTexture = Content.Load<Texture2D>("ship11");



            /*------------------declaracion de la nave y su respectivo disparo------------------------*/
            _sprites = new List<Sprite>()
            {
                new Ship(shipTexture)
                {
                    _normalTexture = shipTexture,
                    Position = new Vector2(GraphicsDevice.Viewport.Width /2 , GraphicsDevice.Viewport.Height + 10), //400, 430     800/2,480+10
                    Bullet = new Bullet(Content.Load<Texture2D>("shotV")),
                    shotSound = Content.Load<SoundEffect>("laser1"),
                    ShipExplosionSound = Content.Load<SoundEffect>("SFX_Explosion_ship"),
                    _explosionTexture= Content.Load<Texture2D>("explosion1"),
                }
            };

           


            /*--------------------declaracion de texturas y objetos varios del juego---------*/
            background = Content.Load<Texture2D>("backgroundSpace");
            font = Content.Load<SpriteFont>("font");
            backgroundMusic = Content.Load<Song>("metal-slug-3-ost-into-the-space-extended");
            gameOverSound = Content.Load<SoundEffect>("metal_slug_gravestone");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);


            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if (_sprites[0].lifes != 0)
            {
                if (_sprites[0].score == 1000)
                    stage = 2;
                if (_sprites[0].score == 2000)
                    stage = 3;

                /*-----------------update de jugador------------------------*/
                foreach (var sprite in _sprites.ToArray())
                    sprite.Update(gameTime, _sprites);
                postUpdate();

                /*-------------------Update de ufos---------------------*/
                foreach (Enemy enemy in _enemies)
                {
                    enemy.Update(_sprites[0], _sprites);
                    foreach (var bullet in enemy.bullets)
                        bullet.Update((Ship)_sprites[0]);
                }
                LoadEnemies();

                if (stage >=2)
                {
                    /*-------------------update de meteoritos----------------*/
                    spawn += (float)gameTime.ElapsedGameTime.TotalSeconds; //usado como delay para aparecer un meteorito
                    foreach (Meteorite meteorite in _meteorites)
                    {
                        meteorite.Update(graphics.GraphicsDevice, (Ship)_sprites[0]);
                    }
                    LoadMeteorites();
                }
                if(stage >= 3)
                {
                    /*update de rocas*/
                    spawnRocks += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    foreach (var rock in _rocks)
                        rock.Update((Ship)_sprites[0]);
                    LoadRocks();
                }
   
            }
            else
            {
                if(_sprites[0].lifes ==0 && endGame == false)
                {
                    endGame = true;
                    MediaPlayer.Stop();
                    gameOverSound.Play();

                }
                var key = Keyboard.GetState();//usada por si es que el ussuario quiere reiniciar el juego
                if (key.IsKeyDown(Keys.R))
                {
                    retry = true;
                }
                if(endGame== true && retry == true)
                {   //reinicio de todo
                    retry = false;
                    endGame = false;
                    stage = 1;
                    _sprites[0].lifes = 10;
                    _sprites[0].score = 0;
                    for(int i=1;i<_sprites.Count ; i++)
                    {
                        _sprites.RemoveAt(i);
                    }
                    for (int i = 0; i < _meteorites.Count; i++)
                    {
                        _meteorites.RemoveAt(i);
                    }
                    for (int i = 0; i < _rocks.Count; i++)
                    {
                        _rocks.RemoveAt(i);
                    }
                    _sprites[0].Position = new Vector2(400,485);
                    MediaPlayer.Play(backgroundMusic);
                    MediaPlayer.IsRepeating = true;

                }
            }


            /*--------------propio de monogame-----------*/
            base.Update(gameTime);
        }

        /*llenado de rocas en su lista*/
        public void LoadRocks()
        {
            if (spawnRocks >= 10)
            {
                spawnRocks = 0;
                if (_rocks.Count < 2)
                    _rocks.Add(new Rock(Content.Load<Texture2D>("rock1")));
            }
            for (int i = 0; i < _rocks.Count; i++)
            {
                if (_rocks[i].isRemoved == true)
                    _rocks.RemoveAt(i);
            }
        }
        /*carga y elimina los enemigos*/
        public void LoadEnemies()
        {
            random = new Random();
            if (spawnedUfos < ufosToSpawn )       //se agrega un enemigo
            {

                _enemies.Add(new Enemy(Content.Load<Texture2D>("ufo1"), Content.Load<Texture2D>("ufo_shot1"))
                {
                    enemyShotSound = Content.Load<SoundEffect>("heat-vision"),
                    enemyExplosionSound = Content.Load<SoundEffect>("SFX_Explosion_ufo"),
                    _explosionTexture=Content.Load<Texture2D>("explosionUfo1"),
                    _normalTexture =Content.Load<Texture2D>("ufo1"),
                    });
                    spawnedUfos++;
            }
            

            for(int i=0;i<_enemies.Count; i++)  // se revisa cada enemigo en juego
            {
                for(int j=0;j<_enemies[i].bullets.Count; j++)//se revisa el estado de las balas de cada enemigo
                {
                    if(_enemies[i].bullets[j].IsRemoved == true)
                    {
                        _enemies[i].bullets.RemoveAt(j);
                    }
                }
                if(_enemies[i].isRemoved == true)
                {
                    _enemies.RemoveAt(i);
                    spawnedUfos--;
                }
            }
        }

        /*-------------carga de  los meteoritos, entre 1 y 4------------*/
        public void LoadMeteorites()
        {
            int randY = random.Next(100,400);
        
            if(spawn >= 1)  
            {
                spawn = 0;
                if (_meteorites.Count < 6)
                    _meteorites.Add(new Meteorite(Content.Load<Texture2D>("meteorite1"), new Vector2(1100,randY)));
            }
            for(int i=0; i< _meteorites.Count; i++)
            {
                if (!_meteorites[i].isVisible)
                {
                    _meteorites.RemoveAt(i);
                }
            }
        }

        /*se revisa si el objeto bala desaparece de la lista*/
        private void postUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            if (_sprites[0].lifes != 0)
            {
                spriteBatch.DrawString(font, "Lifes: " + _sprites[0].lifes, Vector2.Zero, Color.Yellow);
                spriteBatch.DrawString(font, "Protection: " + _sprites[0].protection, new Vector2(0, 11), Color.Yellow);
                spriteBatch.DrawString(font, "Score: " + _sprites[0].score, new Vector2(700, 0), Color.Yellow);
                spriteBatch.DrawString(font, "Instructions:", new Vector2(0, 400), Color.Yellow);
                spriteBatch.DrawString(font, "Move - WASD" , new Vector2(0, 420), Color.Yellow);
                spriteBatch.DrawString(font, "Shoot - Space", new Vector2(0, 440), Color.Yellow);
                spriteBatch.DrawString(font, "Exit - Esc" , new Vector2(0, 460), Color.Yellow);

                foreach (Rock rock in _rocks)
                    rock.Draw(spriteBatch);

                foreach (var sprite in _sprites)
                    sprite.Draw(spriteBatch);

                foreach (Meteorite meteorite in _meteorites)
                    meteorite.Draw(spriteBatch);

                foreach (Enemy enemy in _enemies)
                    enemy.Draw(spriteBatch);
            }
            else
            {
                if(_sprites[0].lifes == 0)
                {
                    spriteBatch.DrawString(font, "GAME OVER", new Vector2((GraphicsDevice.Viewport.Width/2)-50,(GraphicsDevice.Viewport.Height/2)-10), Color.Yellow);
                    spriteBatch.DrawString(font, "Score obtained: "+_sprites[0].score, new Vector2((GraphicsDevice.Viewport.Width / 2) - 70, (GraphicsDevice.Viewport.Height / 2)+10), Color.Yellow);
                    spriteBatch.DrawString(font, "Press ESC to exit" , new Vector2((GraphicsDevice.Viewport.Width / 2) - 70, (GraphicsDevice.Viewport.Height / 2) + 30), Color.Yellow);
                    spriteBatch.DrawString(font, "Press R to Restart the game", new Vector2((GraphicsDevice.Viewport.Width / 2) - 80, (GraphicsDevice.Viewport.Height / 2) + 50), Color.Yellow);
                }
            }

                
               
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
