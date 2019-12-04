using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceNShoot
{
    class EnemyBullet
    {
        public Texture2D _texture;
        public Vector2 Position;
        public bool  IsRemoved = false;
        public float speed = 5f;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public EnemyBullet(Texture2D texture)
        {
            _texture = texture;
        }

        public void Update(Ship ship)
        {
            //colisiones contra jugador
            if ((this.IsTouchingLeft(ship) || this.IsTouchingRight(ship) || this.IsTouchingTop(ship) || this.IsTouchingBottom(ship)) && ship.isProtected ==false)
            {
                this.speed = 0;
                this.IsRemoved = true;
                ship.lifes -= 1;
                ship.ShipExplosionSound.Play();

                ship.isProtected = true;
                ship.waitToSpawn = 50;
                ship.exploted = true;
                
            }
            else
            {
                this.Position.Y += speed;
            }
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
