using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace spaceNShoot
{
    class Enemy 
    {
        public Texture2D _texture;
        public Texture2D _bulletTexture;
        public Texture2D _explosionTexture;
        public Texture2D _normalTexture;
        public SoundEffect enemyShotSound;
        public SoundEffect enemyExplosionSound;

        public List<EnemyBullet> bullets = new List<EnemyBullet>();
        public Vector2 Position;
        public bool isRemoved = false;


        public bool land = false;
        public bool landToShoot = false;
        public bool readyToShot = true;
        private int counterToWait = 0;  //usado para que la explosion permanezca
        private int posX;
        Random random = new Random();

        public int waitToDie = 0;
        public bool exploted = false;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Enemy(Texture2D texture,Texture2D bulletTexture)
        {
            _texture = texture;
            _bulletTexture = bulletTexture;
            Position.X = random.Next(600);
            
        }
        
        public void Update(Sprite player, List<Sprite> bullets)
        {
           if(counterToWait == 0)
            {
                /*-------------------if para bajar a la nave--------------------*/
                if (this.Position.Y <= 80 && land == false)//si jala
                {
                    this.Position.Y++;
                    if (this.Position.Y > 80)
                    {
                        land = true;
                    }
                }
                /*-------------------if para mover de lado a lado a la nave---------------------*/
                if(exploted == false)
                {
                    if (land == true && landToShoot == false)//si jala   ya que bajo, se determina su posicion en X con respecto del jugador
                    {
                        if (readyToShot == true)
                        {
                            posX = random.Next((int)player.Position.X - 200, (int)player.Position.X + 200);
                            readyToShot = false;
                        }

                        if (posX < this.Position.X)
                        {
                            this.Position.X -= 5;
                        }
                        else
                        {
                            if (posX > this.Position.X)
                            {           // 50  60
                                this.Position.X += 5;
                            }
                            else
                            {
                                this.Position.X = posX;
                            }
                        }

                        if (this.Position.X == posX || (this.Position.X >= posX - 5 && this.Position.X <= posX + 5))
                        {
                            landToShoot = true;
                            readyToShot = true;
                            AddBullet();
                            enemyShotSound.Play();
                        }

                    }
                }
                
                /*cuando se detenga, disaprará y reiniciará el ciclo de movimiento aleatorio*/
                if (readyToShot == true && landToShoot == true)
                {
                    
                    landToShoot = false;
                    readyToShot = true;
                    counterToWait = 20; //variable que permite tener un pequeño retraso para moverse den las naves enemigas despues de disparar
                }
            }
            else
            {
                counterToWait--;
            }
            foreach (var bullet in bullets)
            {
                if (bullet.Position.Y >= 490)
                    bullet.IsRemoved = true;
            }

            for(int i=1;i< bullets.Count; i++)
            {
                if (land == true && exploted ==false)
                {
                    if ((this.IsTouchingLeft(bullets[i]) || this.IsTouchingRight(bullets[i]) || this.IsTouchingTop(bullets[i]) || this.IsTouchingBottom(bullets[i])))
                    {
                        
                        this.enemyExplosionSound.Play();
                        bullets[i].IsRemoved = true;
                        player.score += 100;
                        this._texture = _explosionTexture;
                        waitToDie = 50;
                        exploted = true;

                    }
                }
                
            }

            //explosion ufo
            if (exploted == true && waitToDie == 0)
            {
                this.isRemoved = true;
                this._texture = _normalTexture;
                this.Position = new Vector2(400, 485);
                this.land = false;
                this.exploted = false;

            }
            else
            {
                waitToDie--;
            }
        }

        
        private void AddBullet()
        {
            EnemyBullet bullet = new EnemyBullet(_bulletTexture);
            bullet.Position = this.Position;
            bullet.Position.X = this.Position.X + 22;
            bullet.Position.Y += 30;
            bullet.IsRemoved = false;
            bullets.Add(bullet);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
            foreach (var bullet in bullets)
                spriteBatch.Draw(_bulletTexture, bullet.Position, Color.White);
        }
        #region Collision

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Left &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left < sprite.Rectangle.Right &&
                this.Rectangle.Right > sprite.Rectangle.Right &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top < sprite.Rectangle.Bottom &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        #endregion
    }

}
