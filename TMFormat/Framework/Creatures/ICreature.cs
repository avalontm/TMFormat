using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TMFormat.Formats;
using TMFormat.Framework;
using TMFormat.Helpers;
using TMFormat.Framework.Maps;
using TMFormat.Framework.Models;
using TMFormat.Framework.PathFinding;
using TMFormat.Enums;
using TMFormat.Framework.Maps.Actions;
using TMFormat.Framework.Extentions;
using TMFormat.Framework.Outfits;
using TMFormat.Framework.Enums;
using TMFormat.Framework.Loaders;

namespace TMFormat.Framework.Creatures
{
    public partial class ICreature : IDisposable
    {
        //MODEL
        TMCreature anim;

        //Sprites
        public List<TMDir> dirs;

        //Propiedades Publicas
        public long id { set; get; }
        public string name { set; get; }
        public int vocation { set; get; }
        public int direction { set; get; }
        public int heal_current { set; get; }
        public int heal_max { set; get; }
        public int mana_current { set; get; }
        public int mana_max { set; get; }
        public int pos_x { set; get; }
        public int pos_y { set; get; }
        public int pos_z { set; get; }
        public int level { set; get; }
        public int experience { set; get; }
        public double speed { set; get; }
        public int money { set; get; }
        public bool is_walking { set; get; }
        public bool is_dead { set; get; }
        public Vector2 offset { set; get; }

        public int look { private set; get; }
        public int head { private set; get; }
        public int body { private set; get; }
        public int legs { private set; get; }
        public int feet { private set; get; }

        public bool isTarget
        {
            get { return is_target; }
            set { is_target = value; }
        }

        public bool isFollow;
        public bool StartPath;

        public bool isCreature
        {
            get
            {
                return !isMe;
            }
        }

        public bool ToBackLayer
        {
            get
            {
                return _toBackLayer;
            }
            private set { _toBackLayer = value; }
        }

        //Propiedades Internas
        bool is_target;
        float anim_count;
        float _velocity;
        Texture2D _rect;
        bool isMe;
        Vector2 localOffset;
        Vector2 position;
        float anim_speed;
        Vector2 OffsetFloor;
        int AnimationIndex;
        VectorInt3 old_position;
        bool is_offset;

        public List<PathFindingModel> toWalk { private set; get; }
        PathFinder _path;
        List<Creature> _targets;
        Rectangle BoundingBox;
        Vector2 pos1, pos2, pos3, pos4;

        bool _toBackLayer;
        public int next_level;
        Vector2 _name;
        public Vector2 offsetFloor = Vector2.Zero;

        public ICreature(long id, string name, int level, int experience, double speed, int health_current, int health_max, int mana_current, int mana_max, VectorInt3 position, int dir, int look, int head, int body, int legs, int feet, bool isRespawn, bool isMe = false)
        {
            this.isMe = isMe;

            this.id = id;
            this.name = name;
            this.direction = dir;
            this.speed = speed;
            this.level = level;
            this.heal_current = health_current;
            this.heal_max = health_max;
            this.mana_current = mana_current;
            this.mana_max = mana_max;
            this.experience = experience;
            this.pos_x = (int)position.X;
            this.pos_y = (int)position.Y;
            this.pos_z = (int)position.Z;

            this.look = look;
            this.head = head;
            this.body = body;
            this.legs = legs;
            this.feet = feet;

            next_level = this.onNextExperience();

            _name = Fonts.FontDefault.MeasureString(name);

            ChangeOutfit(look, head, body, legs, feet);

            old_position = new VectorInt3(pos_x, pos_y, pos_z);

            offset = new Vector2(pos_x * TMBaseMap.TileSize, pos_y * TMBaseMap.TileSize); //Posicion en Pantalla
            localOffset = Vector2.Zero;

            if (this.is_offset)
            {
                localOffset = new Vector2((TMBaseMap.TileSize / 2) - (TMBaseMap.TileSize / 4), (TMBaseMap.TileSize / 2) - (TMBaseMap.TileSize / 4)); //Centrar en el Tile
            }

            OffsetFloor = Vector2.Zero;
            toWalk = new List<PathFindingModel>();
            _path = new PathFinder(TMInstance.Map);
            _targets = new List<Creature>();

            if (isRespawn)
            {
                onSpawnEffect(); //Efecto de Spawn
            }
        }

        public void FloorOffset(Vector2 _offset)
        {
            OffsetFloor = new Vector2(_offset.X, _offset.Y);
        }

        public virtual void Update(GameTime gameTime)
        {
            pos1 = new Vector2((position.X - OffsetFloor.X + offsetFloor.X) - localOffset.X, (position.Y - OffsetFloor.Y + offsetFloor.Y) - localOffset.Y);
            pos2 = new Vector2((position.X - OffsetFloor.X + offsetFloor.X) - (localOffset.X + TMBaseMap.TileSize), (position.Y - OffsetFloor.Y + offsetFloor.Y) - localOffset.Y);
            pos3 = new Vector2((position.X - OffsetFloor.X + offsetFloor.X) - localOffset.X, (position.Y - OffsetFloor.Y + offsetFloor.X) - (localOffset.Y + TMBaseMap.TileSize));
            pos4 = new Vector2((position.X - OffsetFloor.X + offsetFloor.X) - (localOffset.X + TMBaseMap.TileSize), (position.Y - OffsetFloor.Y + offsetFloor.Y) - (localOffset.Y + TMBaseMap.TileSize));

            if (isMe)
            {
                position = Camera.Center;
            }
            else
            {
                position = new Vector2((offset.X - Camera.Screen.X), (offset.Y - Camera.Screen.Y));
            }

            if (!is_dead)
            {
                _velocity = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * this.speed);
                anim_speed = (_velocity * TMInstance.targetFPS);

                TileTop();
                onTileStay();
                onMoving();
                isMoving();
                onPathWalk();

                if (isMe)
                {
                    Camera.Screen.X = ((int)offset.X - (int)Camera.Center.X);
                    Camera.Screen.Y = ((int)offset.Y - (int)Camera.Center.Y);
                }

                var pos1 = new Vector2((position.X - OffsetFloor.X + offsetFloor.X), (position.Y - OffsetFloor.Y + offsetFloor.Y));
                BoundingBox = new Rectangle((int)pos1.X, (int)pos1.Y, TMBaseMap.TileSize, TMBaseMap.TileSize);
            }
            if (isMe)
            {
                /*
                if (GameDefaultData.Instance != null)
                {
                    //VIDA
                    GameDefaultData.Instance.Heal_Current = heal_current;
                    GameDefaultData.Instance.Max_Heal = heal_max;
                    //MANA
                    GameDefaultData.Instance.Max_Mana = mana_max;
                    GameDefaultData.Instance.Mana_Current = mana_current;
                    //EXPERIENCIA
                    GameDefaultData.Instance.Experience_Current = experience;
                    GameDefaultData.Instance.Max_Experience = next_level;
                }
                */
            }
            Animator(gameTime);
        }

        public void Remove()
        {
            /*
            if (TouchManager.Target != null && TouchManager.Target.id == id)
            {
                TouchManager.onTargetLost();
            }
            */
        }

        public bool Colliding(Vector2 point)
        {
            bool col = false;

            if (!isMe)
            {
                var pos1 = new Vector2((point.X - OffsetFloor.X), (point.Y - OffsetFloor.Y));

                if (BoundingBox.Intersects(new Rectangle((int)pos1.X, (int)pos1.Y, 5, 5)))
                {
                    col = true;
                }
            }

            return col;
        }

        public void TileTop()
        {
            ToBackLayer = TMInstance.Map.MapBase.Floors[pos_z][pos_x, pos_y].isTop;
        }

        void onSpawnEffect()
        {
            //Effects.Effect effect = new Effects.Effect("Teleport", new VectorInt3(pos_x, pos_y, pos_z));
           // GameManager.effects.Add(effect);
        }

        public void onDead()
        {
            if (!is_dead)
            {
                is_dead = true;
                anim_count = 0;
                AnimationIndex = 0;
                localOffset = Vector2.Zero;
                direction = (int)PlayerStatus.Dead;
                TMInstance.Map.MapBase.Floors[pos_z][pos_x, pos_y].isCreature = false;
                TMInstance.Map.players.Remove(this);

                if (isMe)
                {
                    /*
                    if (DeathWindowData.Instance != null)
                    {
                        DeathWindowData.Instance.Show();
                    }
                    */
                }

                Dispose();
            }
        }

        public void onTemple()
        {
            localOffset = Vector2.Zero;

            if (this.is_offset)
            {
                localOffset = new Vector2((TMBaseMap.TileSize / 2) - (TMBaseMap.TileSize / 4), (TMBaseMap.TileSize / 2) - (TMBaseMap.TileSize / 4)); //Centrar en el Tile
            }
            is_dead = false;
            direction = (int)PlayerStatus.None;
        }

        void onTileStay()
        {
            var items = TMInstance.Map.MapBase.Floors[pos_z][pos_x, pos_y].items;

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.Type == (int)TypeItem.Field)
                    {
                        this.onFieldStay(pos_x, pos_y, pos_z, item);
                    }
                }
            }
        }

        void onPathWalk()
        {
            if (toWalk.Count > 0)
            {
                if (!is_walking)
                {
                    old_position = new VectorInt3(pos_x, pos_y, pos_z);

                    var path = toWalk[0];

                    if (pos_z != path.position.Z)
                    {
                        toWalk.Clear();
                        pos_x = old_position.X;
                        pos_y = old_position.Y;
                        pos_z = old_position.Z;
                        return;
                    }

                    if (!canMove(path.position.X, path.position.Y, path.position.Z))
                    {
                        toWalk.Clear();
                        pos_x = old_position.X;
                        pos_y = old_position.Y;
                        pos_z = old_position.Z;
                        return;
                    }

                    pos_x = path.position.X;
                    pos_y = path.position.Y;
                    pos_z = path.position.Z;

                    onChangeDir();
                    toWalk.RemoveAt(0);
                }
            }
        }

        public void Stop()
        {
            toWalk.Clear();
            is_target = false;
            isFollow = false;
        }

        public int toPathWalk(VectorInt3 _start, List<VectorInt3> paths)
        {
            toWalk.Clear();

            if (paths != null)
            {
                if (pos_x != _start.X && pos_z == _start.Z || pos_y != _start.Y && pos_z == _start.Z)
                {
                    ToPosition(_start, false);
                }

                foreach (var path in paths)
                {
                    PathFindingModel model = new PathFindingModel();
                    model.position = path;
                    toWalk.Add(model);
                }

                if (isMe)
                {
                    if (toWalk.Count > 0)
                    {
                       // TouchManager.onDestine(toWalk[toWalk.Count - 1].position);
                    }
                }
            }

            return toWalk.Count;
        }

        public void onExperience(int level, int exp, int gainExp, int _NextLevel)
        {
            this.level = level;
            this.experience = exp;
            this.next_level = _NextLevel;

            //Effects.TextDamage effect = new Effects.TextDamage(string.Format("{0} EXP!", gainExp), new VectorInt3(pos_x, pos_y, pos_z), Color.Gray);
           // GameManager.effects.Add(effect);
        }

        void onChangeDir()
        {
            if (pos_y < (int)old_position.Y)
            {
                direction = (int)PlayerDir.North;
            }
            if (pos_x > (int)old_position.X)
            {
                direction = (int)PlayerDir.East;
            }
            if (pos_y > (int)old_position.Y)
            {
                direction = (int)PlayerDir.South;
            }
            if (pos_x < (int)old_position.X)
            {
                direction = (int)PlayerDir.West;
            }
        }

        public void ChangeOutfit(int look, int head, int body, int legs, int feet)
        {
            anim = TMCreature.Load($"creatures/chr_{look}");

            if (anim == null)
            {
                throw new NotImplementedException($"[ChangeOutfit] NOT FOUND! 'creatures/chr_{look}'");
            }

            is_offset = anim.is_offset;

            this.dirs = new List<TMDir>();
            this.dirs.Add(new TMDir());
            this.dirs.Add(new TMDir());
            this.dirs.Add(new TMDir());
            this.dirs.Add(new TMDir());
            this.dirs.Add(new TMDir());

            this.look = look;
            this.head = head;
            this.body = body;
            this.legs = legs;
            this.feet = feet;

            int _dir = 0;
            int _anim = 0;

            //Cambiamos los colores del Outfit
            foreach (var dirs in anim.dirs)
            {
                _anim = 0;

                foreach (var tex in dirs.sprites)
                {
                    Texture2D _texture = tex.textures[0].ToTexture2D();

                    if (tex.masks[0] != null)
                    {
                        this.dirs[_dir].sprites[_anim].Sprite1 = _texture.ChangeColor(tex.masks[0].ToTexture2D(), Colors.colors[head].color, Colors.colors[body].color, Colors.colors[legs].color, Colors.colors[feet].color);
                    }

                    _texture = tex.textures[1].ToTexture2D();

                    if (tex.masks[1] != null)
                    {
                        this.dirs[_dir].sprites[_anim].Sprite2 = _texture.ChangeColor(tex.masks[1].ToTexture2D(), Colors.colors[head].color, Colors.colors[body].color, Colors.colors[legs].color, Colors.colors[feet].color);
                    }

                    _texture = tex.textures[2].ToTexture2D();

                    if (tex.masks[2] != null)
                    {
                        this.dirs[_dir].sprites[_anim].Sprite3 = _texture.ChangeColor(tex.masks[2].ToTexture2D(), Colors.colors[head].color, Colors.colors[body].color, Colors.colors[legs].color, Colors.colors[feet].color);
                    }

                    _texture = tex.textures[3].ToTexture2D();

                    if (tex.masks[3] != null)
                    {
                        this.dirs[_dir].sprites[_anim].Sprite4 = _texture.ChangeColor(tex.masks[3].ToTexture2D(), Colors.colors[head].color, Colors.colors[body].color, Colors.colors[legs].color, Colors.colors[feet].color);
                    }

                    _anim++;
                }
                _dir++;
            }

        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {

            if (!isMe && is_target) //Si es enemigo y esta en Target.
            {
                var posSquare = new Vector2((position.X - OffsetFloor.X + offsetFloor.X), (position.Y - OffsetFloor.Y + offsetFloor.Y));

                _spriteBatch.Draw(Textures.Square, Utils.GetTileDestine(posSquare.X, posSquare.Y), null, Color.Red);
            }

            //Dibujamos la creatura
            if (dirs[direction].sprites[AnimationIndex].Sprite1 != null)
            {
                _spriteBatch.Draw(dirs[direction].sprites[AnimationIndex].Sprite1, Utils.GetTileDestine(pos1.X, pos1.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            }

            // DrawRectangle(_spriteBatch, BoundingBox, Color.Red * 0.5f);
        }

        public void Say(string msg)
        {
            //Effects.SayMessage effect = new Effects.SayMessage(string.Format("{0} say:", name), msg, new VectorInt3(pos_x, pos_y, pos_z), Color.Yellow);
            //GameManager.effects.Add(effect);
        }

        public virtual void DrawExtras(SpriteBatch _spriteBatch)
        {
            if (dirs[direction].sprites[AnimationIndex].Sprite2 != null)
                _spriteBatch.Draw(dirs[direction].sprites[AnimationIndex].Sprite2, Utils.GetTileDestine(pos2.X, pos2.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            if (dirs[direction].sprites[AnimationIndex].Sprite3 != null)
                _spriteBatch.Draw(dirs[direction].sprites[AnimationIndex].Sprite3, Utils.GetTileDestine(pos3.X, pos3.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            if (dirs[direction].sprites[AnimationIndex].Sprite4 != null)
                _spriteBatch.Draw(dirs[direction].sprites[AnimationIndex].Sprite4, Utils.GetTileDestine(pos4.X, pos4.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }

        public virtual void DrawName(SpriteBatch _spriteBatch)
        {
            if (!is_dead) //Si no esta muerto Dibujamos la barra de vida
            {
                int bar_healt = (TMBaseMap.TileSize / 2) + 8; //Ancho de la Barra de vida
                int health = (int)((float)(heal_current / (float)heal_max) * bar_healt);

                DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X, (int)(pos1.Y - 8), (bar_healt + 2), 4), Color.Black);

                int porcentaje = (int)((double)((double)heal_current / (double)heal_max) * 100);

                if (porcentaje >= 100)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), Color.Green);
                }
                if (porcentaje < 100 && porcentaje >= 80)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), new Color(0, 85, 0));
                }
                if (porcentaje < 80 && porcentaje >= 60)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), new Color(255, 165, 0));
                }
                if (porcentaje < 60 && porcentaje >= 40)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), new Color(160, 65, 10));
                }
                if (porcentaje < 40 && porcentaje >= 20)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), new Color(120, 0, 10));
                }
                if (porcentaje < 20 && porcentaje >= 0)
                {
                    DrawRectangle(_spriteBatch, new Rectangle((int)pos1.X + 1, (int)(pos1.Y - 7), health, 2), new Color(60, 0, 5));
                }

                //Border
                _spriteBatch.DrawString(Fonts.FontDefault, name, new Vector2((pos1.X + (TMBaseMap.TileSize / 2) + 1) - (_name.X / 2), pos1.Y - 22), Color.Black);
                _spriteBatch.DrawString(Fonts.FontDefault, name, new Vector2((pos1.X + (TMBaseMap.TileSize / 2) - 1) - (_name.X / 2), pos1.Y - 22), Color.Black);
                _spriteBatch.DrawString(Fonts.FontDefault, name, new Vector2((pos1.X + (TMBaseMap.TileSize / 2)) - (_name.X / 2), pos1.Y - 22 + 1), Color.Black);
                _spriteBatch.DrawString(Fonts.FontDefault, name, new Vector2((pos1.X + (TMBaseMap.TileSize / 2)) - (_name.X / 2), pos1.Y - 22 - 1), Color.Black);

                _spriteBatch.DrawString(Fonts.FontDefault, name, new Vector2((pos1.X + (TMBaseMap.TileSize / 2)) - (_name.X / 2), pos1.Y - 22), Color.White);
            }
        }

        void Animator(GameTime gameTime)
        {
            int total_sprites = 3;  // 3 Sprites en animacion

            if (!is_dead)
            {
                if (isMoving())
                {
                    if (anim_count > 8)
                    {
                        anim_count = 0;
                        AnimationIndex++;

                        if (AnimationIndex >= total_sprites)
                        {
                            AnimationIndex = 0;
                        }
                    }

                    anim_count += (float)(gameTime.ElapsedGameTime.TotalSeconds * this.anim_speed);
                    return;
                }
                anim_count = 0;
                AnimationIndex = 0;
            }
            else
            {
                if (anim_count > 3000)
                {
                    anim_count = 0;
                    AnimationIndex++;

                    if (AnimationIndex >= total_sprites)
                    {
                        AnimationIndex = total_sprites;
                    }
                }

                if (AnimationIndex < (dirs[direction].sprites.Count - 1))
                {
                    anim_count += (float)(0.5f * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }

        void DrawRectangle(SpriteBatch _spriteBatch, Rectangle coords, Color color)
        {
            if (_rect == null)
            {
                _rect = new Texture2D(TMInstance.GraphicsDevice, 1, 1);
                _rect.SetData(new[] { Color.White });
            }

            _spriteBatch.Draw(_rect, coords, color);
        }

        bool isMoving()
        {
            if (!is_dead)
            {
                if (!((int)(pos_x * TMBaseMap.TileSize) == (int)offset.X && (int)(pos_y * TMBaseMap.TileSize) == (int)offset.Y))
                {
                    is_walking = true;
                    return true;
                }
            }
            is_walking = false;
            return false;
        }

        void onMoving()
        {
            if (offset.X < (pos_x * TMBaseMap.TileSize)) //Movemos al Norte
            {
                offset = new Vector2(Math.Min(offset.X + _velocity, (pos_x * TMBaseMap.TileSize)), offset.Y);
            }
            else if (offset.X > (pos_x * TMBaseMap.TileSize))  //Movemos al Este
            {
                offset = new Vector2(Math.Max(offset.X - _velocity, (pos_x * TMBaseMap.TileSize)), offset.Y);
            }

            if (offset.Y < (pos_y * TMBaseMap.TileSize)) //Movemos al Sur
            {
                offset = new Vector2(offset.X, Math.Min(offset.Y + _velocity, (pos_y * TMBaseMap.TileSize)));
            }
            else if (offset.Y > (pos_y * TMBaseMap.TileSize)) //Movemos al Oeste
            {
                offset = new Vector2(offset.X, Math.Max(offset.Y - _velocity, (pos_y * TMBaseMap.TileSize)));
            }

        }

        public bool canMove(int x, int y, int z)
        {
            TMItem item;

            item = TMInstance.Map.MapBase.Floors[z][x, y].item;

            if (item == null)
            {
                return false;
            }

            if (item.Id == 0)
            {
                return false;
            }

            if (item.Block)
            {
                return false;
            }

            if (TMInstance.Map.MapBase.Floors[z][x, y].items != null)
            {
                foreach (var _item in TMInstance.Map.MapBase.Floors[z][x, y].items)
                {
                    if (_item.Block || _item.Block && _item.Type != (int)TypeItem.Stair)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        public void ToPosition(VectorInt3 _destine, bool clear = true)
        {
            if (clear)
            {
                toWalk.Clear(); //Limpiamos el path de caminar.
            }

            int x = _destine.X;
            int y = _destine.Y;
            int z = _destine.Z;

            old_position.X = pos_x;
            old_position.Y = pos_y;
            old_position.Z = pos_z;

            pos_x = x;
            pos_y = y;
            pos_z = z;

            offset = new Vector2((pos_x * TMBaseMap.TileSize), (pos_y * TMBaseMap.TileSize));

            if (isMe)
            {
                Camera.Screen.X = (int)(offset.X - Camera.Center.X);
                Camera.Screen.Y = (int)(offset.Y - Camera.Center.Y);
            }
        }

        public void Dispose()
        {
            /*
            if (TouchManager.Target != null)
            {
                if (TouchManager.Target == this)
                {
                    TouchManager.Target = null;
                }
            }
            */
            Debug.WriteLine("[Dispose]");
        }
    }

}
