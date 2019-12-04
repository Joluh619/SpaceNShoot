using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace spaceNShoot
{
    public class Meteorite 
    {
        public Texture2D _texture;
        public Vector2 Position;
        public Vector2 velocity;

        public bool isVisible = true;

        Random random = new Random();
        int randX, randY;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Meteorite(Texture2D texture, Vector2 newPosition)
        {
            _texture = texture;
            Position = newPosition;

            randY = random.Next(-4,4);
            randX = random.Next(-4, -1);

            velocity = new Vector2(randX, randY);
        }

        public void Update(GraphicsDevice graphics, Ship ship)
        {
            Position += velocity;

            if (Position.Y <= 0 || Position.Y >= graphics.Viewport.Height - _texture.Height)
                velocity.Y = -velocity.Y;
            if (Position.X < 0 - _texture.Width)
                isVisible = false;

            
            if ((this.IsTouchingLeft(ship) || this.IsTouchingRight(ship) || this.IsTouchingTop(ship) || this.IsTouchingBottom(ship)) && ship.isProtected == false)
            { 
                this.velocity.X = 0;
                this.velocity.Y = 0;
                this.isVisible = false;
                ship.lifes -= 1;
                ship.ShipExplosionSound.Play();

                ship.isProtected = true;
                ship.waitToSpawn = 50;
                ship.exploted = true;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position,Color.White);
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
