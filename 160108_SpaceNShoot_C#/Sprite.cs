using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace spaceNShoot
{
    public class Sprite : ICloneable
    {
        

        protected Texture2D _texture;
        protected float _rotation;
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Vector2 Position;
        public Vector2 Origin;

        public Vector2 Direction;
        //public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;

        public Sprite Parent;

        public float LifeSpan = 0f;

        
        public int lifes = 10;
        public int respawnProtection=200;
        public bool isProtected = true;
        public String protection = "ON";
        public int score = 0;

        public bool IsRemoved = false;
        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X-25, (int)Position.Y-20, _texture.Width-10 , _texture.Height);
            }
        }

        public virtual void Update(GameTime time, List<Sprite> sprites){
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);

        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
