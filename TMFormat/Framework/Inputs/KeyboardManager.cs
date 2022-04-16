using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Framework.Enums;

namespace TMFormat.Framework.Inputs
{
    public static class KeyboardManager
    {
        public static KeyboardState KeyState { private set; get; }

        public static void Update(GameTime gameTime)
        {
            KeyState = Keyboard.GetState();
        }

        public static void Move(PlayerDir dir)
        {
            if (TMInstance.Map.player.is_walking)
            {
                return;
            }

            VectorInt3 position = new VectorInt3(TMInstance.Map.player.pos_x, TMInstance.Map.player.pos_y, TMInstance.Map.player.pos_z);

            switch (dir)
            {
                case PlayerDir.North:
                    TMInstance.Map.player.toPathWalk(position, new List<VectorInt3>() { new VectorInt3(position.X, position.Y - 1, position.Z) });
                    break;
                case PlayerDir.West:
                    TMInstance.Map.player.toPathWalk(position, new List<VectorInt3>() { new VectorInt3(position.X - 1, position.Y, position.Z) });
                    break;
                case PlayerDir.South:
                    TMInstance.Map.player.toPathWalk(position, new List<VectorInt3>() { new VectorInt3(position.X, position.Y + 1, position.Z) });
                    break;
                case PlayerDir.East:
                    TMInstance.Map.player.toPathWalk(position, new List<VectorInt3>() { new VectorInt3(position.X + 1, position.Y, position.Z) });
                    break;
            }

        }
    }
}
