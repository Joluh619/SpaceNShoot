using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace spaceNShoot
{
    public class Ship : Sprite
    {
        public Bullet Bullet;
        public SoundEffect shotSound;
        public Texture2D _explosionTexture;
        public Texture2D _normalTexture;
        public SoundEffect ShipExplosionSound;

        public bool land = false;
        public bool exploted = false;
        public int waitToSpawn = 0;

        public Ship(Texture2D texture)
            : base(texture)
        {
        }

        public override void Update(GameTime time, List<Sprite> sprites)
        {
            if(land == true)
            {
                _previousKey = _currentKey;
                _currentKey = Keyboard.GetState();

                if (_currentKey.IsKeyDown(Keys.A))
                {
                    /*_rotation -= MathHelper.ToRadians(RotationVelocity);*/

                    Position.X -= 2;
                    if (Position.X <= 33)
                        Position.X += 2;
                }

                if (_currentKey.IsKeyDown(Keys.D))
                {
                    /*_rotation += MathHelper.ToRadians(RotationVelocity);*/
                    Position.X += 2;
                    if (Position.X >= 800 - 33)
                        Position.X -= 2;
                }

                if (_currentKey.IsKeyDown(Keys.W))
                {
                    Position.Y -= 2;
                    if (Position.Y <= 80)
                        Position.Y += 2;
                }
                if (_currentKey.IsKeyDown(Keys.S))
                {
                    /*Position -= Direction * LinearVelocity;*/
                    Position.Y += 2;
                    if (Position.Y >= 480 - 50)
                        Position.Y -= 2;
                }

                Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));//usado para que las balas puedan ir hacia arriba

                if(exploted == false)
                {
                    if (_currentKey.IsKeyDown(Keys.Space) &&
                   _previousKey.IsKeyUp(Keys.Space))
                    {
                        AddBullet(sprites);
                        shotSound.Play();
                    }
                }

                //para explosion
                if (respawnProtection == 0)
                {
                    respawnProtection = 200;
                    isProtected = false;
                    protection = "OFF";
                }
                else
                {
                    if (respawnProtection > 0 && isProtected == true)
                    {
                        respawnProtection--;
                        protection = "ON";
                    }
                        
                }

                if (exploted == true && waitToSpawn == 0)
                {
                    this._texture = _normalTexture;
                    this.Position = new Vector2(400, 485);
                    this.land = false;
                    this.exploted = false;
                    
                }
                else
                {
                    waitToSpawn--;
                    if (exploted == true)
                       this. _texture = _explosionTexture;
                }
                //fin para explosion
            }
            else
            {
                if (Position.Y <= 430)
                {
                    land = true;
                }
                else
                {
                    Position.Y--;
                }
            }

            
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            //bullet.Direction = this.Direction;
            bullet.Direction.X = -this.Direction.Y;
            bullet.Direction.Y = -this.Direction.X;
            bullet.Position = this.Position;
            bullet.Position.Y -= 40; 
            bullet.LinearVelocity = this.LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

    }
}
