using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Framework;

namespace TMFormat.Framework.Creatures
{
    public class Creature : ICreature
    {
        public Creature(int id, string name, int level, int experience, double speed, int health_current, int health_max, int mana_current, int mana_max, VectorInt3 position, int dir, int look, int head, int body, int legs, int feet, bool isRespawn, bool isMe = false) : base(id, name, level, experience, speed, health_current, health_max, mana_current, mana_max, position, dir, look, head, body, legs, feet, isRespawn, isMe)
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
        public override void DrawExtras(SpriteBatch _spriteBatch)
        {
            base.DrawExtras(_spriteBatch);
        }

        public override void DrawName(SpriteBatch _spriteBatch)
        {
            base.DrawName(_spriteBatch);
        }
    }
}
