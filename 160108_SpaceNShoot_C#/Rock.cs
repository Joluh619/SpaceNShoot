using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace spaceNShoot
{
    public class Rock
    {
        public Texture2D _texture;
        public Vector2 Position;
        public bool isRemoved = false;
        public float speed = 1f;

        Random random = new Random();

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width , _texture.Height);
            }
        }

        public Rock(Texture2D texture)
        {
            _texture = texture;
            Position.X = random.Next(400);
            Position.Y = -40;
        }

        public void Update(Ship ship)
        {
            Position.Y+= speed;
            if ((this.IsTouchingLeft(ship) || this.IsTouchingRight(ship) || this.IsTouchingTop(ship) || this.IsTouchingBottom(ship)) && ship.isProtected == false)
            {
                
                ship.lifes -= 1;
                ship.ShipExplosionSound.Play();

                ship.isProtected = true;
                ship.waitToSpawn = 50;
                ship.exploted = true;
            }
            if (Position.Y >= 480)
                isRemoved = true;
                

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
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