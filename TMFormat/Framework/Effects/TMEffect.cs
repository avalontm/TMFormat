using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Effects
{
    public class TMEffect : IEffect
    {
        public TMEffect(int item_id, VectorInt3 position) : base(item_id, position)
        {

        }

        public TMEffect(string item_name, VectorInt3 position) : base(item_name, position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
        }
    }
}
