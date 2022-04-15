using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMFormat.Enums;
using TMFormat.Formats;
using TMFormat.Framework.Creatures;
using TMFormat.Framework.Extentions;
using TMFormat.Framework.Loaders;
using TMFormat.Framework.Maps.Actions;
using TMFormat.Framework.Resolution;
using TMFormat.Models;

namespace TMFormat.Framework.Maps
{
    public class MapManager
    {
        public List<ICreature> players { private set; get; }
        public ICreature player { private set; get; }

        public TMBaseMap MapBase { private set; get; }
        public MapTile mapTile { private set; get; }
        public Camera camera { private set; get; }
        public int FloorDefault { private set; get; }
        public bool isDungeon { private set; get; }

        GraphicsDevice _graphicsDevice;
        SpriteBatch _spriteBatch;

        bool HiddenFloors;
        int ScreenX;
        int ScreenY;
        int ScreenHeight;
        int ScreenWidth;
        int floorStart = 4;
        int floorEnd = 10;
        float Ambient = 0.70f;
        float AmbientDefault = 0.80f;
        float AmbientDungeon = 0.50f;

        RenderTarget2D LightsTarget;
        RenderTarget2D MainTarget;

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public MapManager(GraphicsDevice graphicsDevice, List<TMSprite> items)
        {
            FloorDefault = 7;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            mapTile = new MapTile(this, _spriteBatch);
            camera = new Camera(this);
            players = new List<ICreature>();
            player = new ICreature(0, "Player", 1, 0, 0.35f, 100, 100, 35, 35, new VectorInt3(500,500,7), 0 , 128,0,0,0,0, true, true);
            //Initialize render targets
            var pp = _graphicsDevice.PresentationParameters;
            LightsTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            MainTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            MapBase = new TMBaseMap(items);
        }

        public void Update(GameTime gameTime)
        {
            onCameraScreens();

            onEffects(gameTime);
            onTileAnimate(gameTime);
            onServerMessage(gameTime);

            if (player != null)
            {
                player.Update(gameTime);
                onDungeon();
                onCheckHiddenFloors();
            }

            onPlayersUpdate(gameTime);

        }

        void onServerMessage(GameTime gameTime)
        {
            /*
            if (messages.Count > 0)
            {
                messages[0].Update(gameTime);
            }
            if (infos.Count > 0)
            {
                infos[0].Update(gameTime);
            }
            */
        }

        void onCameraScreens()
        {
            var screen = camera.onCameraScreen();
            ScreenX = screen.X;
            ScreenY = screen.Y;
            ScreenHeight = screen.Height;
            ScreenWidth = screen.Width;
        }

        void onPlayersUpdate(GameTime gameTime)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Update(gameTime);
            }
        }

        void onDungeon()
        {
            if (player == null)
            {
                return;
            }

            isDungeon = (player.pos_z > FloorDefault);

            if (isDungeon)
            {
                Ambient = AmbientDungeon;
                return;
            }
            Ambient = AmbientDefault;

        }

        void onEffects(GameTime gameTime)
        {
            /*
            for (var i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                effect.Update(gameTime);
            }
            */
        }

        void onTileAnimate(GameTime gameTime)
        {
            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    for (int z = floorStart; z < floorEnd; z++)
                    {
                        if (MapBase.Floors[z][x, y].item != null)
                        {
                            if (MapBase.Floors[z][x, y].item.isAnimation)
                            {
                                MapBase.Floors[z][x, y].item.TimeAnimation += (float)(MapBase.Floors[z][x, y].item.AniSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                                if (MapBase.Floors[z][x, y].item.TimeAnimation > 250)
                                {
                                    MapBase.Floors[z][x, y].item.TimeAnimation = 0;
                                    MapBase.Floors[z][x, y].item.IndexAnimation++;

                                    if (MapBase.Floors[z][x, y].item.IndexAnimation == MapBase.Floors[z][x, y].item.Textures.Count)
                                    {
                                        MapBase.Floors[z][x, y].item.IndexAnimation = 0;
                                    }
                                }
                            }
                        }

                        if (MapBase.Floors[z][x, y].items != null)
                        {
                            for (var a = 0; a < MapBase.Floors[z][x, y].items.Count; a++)
                            {
                                if (!MapBase.Floors[z][x, y].isCreature)
                                {
                                    player.onFieldStay(x, y, z, MapBase.Floors[z][x, y].items[a]);
                                }

                                if (MapBase.Floors[z][x, y].items[a].isAnimation)
                                {
                                    MapBase.Floors[z][x, y].items[a].TimeAnimation += (float)(MapBase.Floors[z][x, y].items[a].AniSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                                    if (MapBase.Floors[z][x, y].items[a].TimeAnimation > 0.5)
                                    {
                                        MapBase.Floors[z][x, y].items[a].TimeAnimation = 0;
                                        MapBase.Floors[z][x, y].items[a].IndexAnimation++;

                                        if (MapBase.Floors[z][x, y].items[a].IndexAnimation == MapBase.Floors[z][x, y].items[a].Textures.Count)
                                        {
                                            MapBase.Floors[z][x, y].items[a].IndexAnimation = 0;
                                        }
                                    }
                                }
                            }
                        }

                    }//Z
                } //Y
            } //X
        }

        void onCheckHiddenFloors()
        {
            if (player == null)
            {
                return;
            }

            int start = 4;
            int Floor = FloorDefault;

            if (isDungeon)
            {
                start = FloorDefault + 1;
                Floor = 10;
            }

            try
            {
                if (MapBase.Floors[player.pos_z - 1][player.pos_x, player.pos_y].item != null)
                {
                    HiddenFloors = true;
                    return;
                }

                if (MapBase.Floors[player.pos_z - 2][player.pos_x, player.pos_y].item != null)
                {
                    HiddenFloors = true;
                    return;
                }

                if (MapBase.Floors[player.pos_z - 3][player.pos_x, player.pos_y].item != null)
                {
                    HiddenFloors = true;
                    return;
                }

                if (MapBase.Floors[player.pos_z - 4][player.pos_x, player.pos_y].item != null)
                {
                    HiddenFloors = true;
                    return;
                }
            }
            catch
            {

            }


            if (MapBase.Floors[player.pos_z][player.pos_x, player.pos_y - 1].item != null) // Verificamos si hay una ventana al norte.
            {
                if (MapBase.Floors[player.pos_z][player.pos_x, player.pos_y - 1].items != null)
                {
                    TMItem item = MapBase.Floors[player.pos_z][player.pos_x, player.pos_y - 1].items.FirstOrDefault();
                    if (item != null)
                    {
                        if (item.isWindow)
                        {
                            HiddenFloors = true;
                            return;
                        }
                    }
                }
            }

            if (MapBase.Floors[player.pos_z][player.pos_x + 1, player.pos_y].item != null) //Verificamos si hay una ventana a la derecha. =>
            {
                if (MapBase.Floors[player.pos_z][player.pos_x + 1, player.pos_y].items != null)
                {
                    TMItem item = MapBase.Floors[player.pos_z][player.pos_x + 1, player.pos_y].items.FirstOrDefault();
                    if (item != null)
                    {
                        if (item.isWindow || item.Type == (int)TypeItem.Wall)
                        {
                            HiddenFloors = true;
                            return;
                        }
                    }
                }
            }

            if (MapBase.Floors[player.pos_z][player.pos_x, player.pos_y + 1].item != null) // Verificamos si hay una ventana al sur.
            {
                if (MapBase.Floors[player.pos_z][player.pos_x, player.pos_y + 1].items != null)
                {
                    var items = MapBase.Floors[player.pos_z][player.pos_x, player.pos_y + 1].items;

                    foreach (var item in items)
                    {
                        if (item.isWindow || item.Type == (int)TypeItem.Wall)
                        {
                            HiddenFloors = true;
                            return;
                        }
                    }
                }
            }

            if (MapBase.Floors[player.pos_z][player.pos_x - 1, player.pos_y].item != null) // Verificamos si hay una ventana a la izquierda. <=
            {
                if (MapBase.Floors[player.pos_z][player.pos_x - 1, player.pos_y].items != null)
                {
                    TMItem item = MapBase.Floors[player.pos_z][player.pos_x - 1, player.pos_y].items.FirstOrDefault();
                    if (item != null)
                    {
                        if (item.isWindow)
                        {
                            HiddenFloors = true;
                            return;
                        }
                    }
                }
            }

            HiddenFloors = false;
        }

        public void Draw(GameTime gameTime) // Floor Current
        {
            onDrawLights(); // Layer de luces
            onDrawMapTiles(); // Layer de Mapa
            onDrawCombine(); // Combinamos los layers anteriores
        }

        void onDrawMapTiles()
        {
            // Seleccionamos el dibuijado en la escena Main.
            _graphicsDevice.SetRenderTarget(MainTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ResolutionManager.TransformationMatrix());

            if (player.pos_z < FloorDefault) //dibujamos si estamos arriba del piso por defecto.
            {
                for (var f = player.pos_z; f <= FloorDefault; f++)
                {
                    onDrawFloorsDown(f);
                }
            }

            onDrawFloorCurrent(); //Dibujamos el piso actual

            if (player.pos_z == FloorDefault) //dibujamos si estamos en el piso por defecto.
            {
                if (!HiddenFloors)
                {
                    onDrawFloorsUP(6);
                    onDrawFloorsUP(5);
                    onDrawFloorsUP(4);
                    //onDrawFloorsUP(3);
                    //onDrawFloorsUP(2);
                    //onDrawFloorsUP(1);
                    //onDrawFloorsUP(0);
                }
            }

            onEffectsDraw(_spriteBatch);
            _spriteBatch.End();
        }

        void onDrawLights()
        {
            int floor_current = player.pos_z;

            _graphicsDevice.SetRenderTarget(LightsTarget);
            _graphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, ResolutionManager.TransformationMatrix());
            Shaders.bluerEffect.CurrentTechnique.Passes[0].Apply();

            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X) - (Textures.LightMask.Width / 2) + (TMBaseMap.TileSize / 2);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y) - (Textures.LightMask.Height / 2) + (TMBaseMap.TileSize / 2);

                    if (MapBase.Floors[floor_current][x, y].item != null)
                    {
                        if (MapBase.Floors[floor_current][x, y].item.isLight)
                        {
                            ItemColor _color = MapBase.Floors[floor_current][x, y].item.LightColor;
                            _spriteBatch.Draw(Textures.LightMask, new Vector2(tmpX, tmpY), null, new Color(_color.R, _color.G, _color.B));
                            break;
                        }
                    }

                    if (MapBase.Floors[floor_current][x, y].items != null)
                    {
                        foreach (var item in MapBase.Floors[floor_current][x, y].items)
                        {
                            if (item.isLight)
                            {
                                ItemColor _color = item.LightColor;
                                _spriteBatch.Draw(Textures.LightMask, new Vector2(tmpX, tmpY), null, new Color(_color.R, _color.G, _color.B));
                                break;
                            }
                        }
                    }
                }
                //Y
            }
            //X

            _spriteBatch.End();
        }


        void onDrawCombine()
        {
            _graphicsDevice.SetRenderTarget(null);
            //_graphicsDevice.Clear(Color.Magenta);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, ResolutionManager.TransformationMatrix());
            Shaders.ambientegEffect.Parameters["intensity"].SetValue(Ambient);
            Shaders.ambientegEffect.Parameters["lightsTexture"].SetValue(LightsTarget);
            Shaders.ambientegEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(MainTarget, ResolutionManager.ScreenArea, Color.White);

            _spriteBatch.End();
        }

        void onEffectsDraw(SpriteBatch spriteBatch)
        {
            /*
            for (var i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                if (effect.pos.Z == player.pos_z)
                {
                    effect.Draw(spriteBatch);
                }
            }
            */
        }

        void onDrawFloorCurrent()
        {
            int floor_current = player.pos_z;

            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor_current][x, y].item != null)
                    {
                        bool isDraw = true;
                        if (!HiddenFloors && floor_current == FloorDefault)
                        {
                            var isTile = MapBase.Floors[floor_current - 1][x + 1, y + 1].item; //Si hay Tile arriba.

                            if (isTile != null)
                            {
                                if (isTile.Id > 1 && isTile.Type == (int)TypeItem.Tile)
                                {
                                    isDraw = false; //No lo dibujamos
                                }
                            }
                        }
                        if (isDraw)
                        {
                            mapTile.DrawTileBase(floor_current, x, y, tmpX, tmpY);
                        }
                    }
                }
            }

            for (int i = 0; i < players.Count; i++)
            {
                if (player.pos_z == players[i].pos_z)
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (players[i].ToBackLayer) // Layer Atras
                        {
                            players[i].offsetFloor = Vector2.Zero;
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }

            if (player.ToBackLayer) // Layer Atras
            {
                player.Draw(_spriteBatch);
            }

            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor_current][x, y].item != null)
                    {
                        mapTile.DrawTileTop(floor_current, x, y, tmpX, tmpY);
                    }
                }
            }

            for (int i = 0; i < players.Count; i++)
            {
                if (player.pos_z == players[i].pos_z)
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (!players[i].ToBackLayer) // Layer Normal
                        {
                            players[i].offsetFloor = Vector2.Zero;
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }

            if (!player.ToBackLayer) // Layer Normal
            {
                player.Draw(_spriteBatch);
            }

            for (int i = 0; i < players.Count; i++)
            {
                bool onSCreen = player.isOnScreen(players[i]);
                if (onSCreen)
                {
                    players[i].DrawExtras(_spriteBatch);
                }
            }

            for (int i = 0; i < players.Count; i++)
            {
                bool onSCreen = player.isOnScreen(players[i]);
                if (onSCreen)
                {
                    players[i].DrawName(_spriteBatch);
                }
            }

            player.DrawName(_spriteBatch);
        }

        void onDrawFloorsDown(int floor) // Floors DOWN
        {
            int current_floor = player.pos_z;
            int tileOffset = 1;

            if (current_floor < floor && current_floor < (FloorDefault - 1))
            {
                current_floor = (current_floor + 1);
            }

            int ScreenFloor = (floor - current_floor);

            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight - tileOffset; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth - tileOffset; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor][x, y].item != null)
                    {
                        bool isDraw = true;
                        tmpX += (TMBaseMap.TileSize * ScreenFloor);
                        tmpY += (TMBaseMap.TileSize * ScreenFloor);

                        var isTile = MapBase.Floors[player.pos_z][x + 1, y + 1].item;

                        if (isTile != null)
                        {
                            if (isTile.Id > 1 && isTile.Type == (int)TypeItem.Tile)
                            {
                                isDraw = false;
                            }
                        }

                        if (isDraw)
                        {
                            mapTile.DrawTileBase(floor, x, y, tmpX, tmpY);
                        }
                    }

                }
            }

            //DUBUJAMOS LOS OTROS PERSONAJES
            for (int i = 0; i < players.Count; i++)
            {
                if (floor == players[i].pos_z) //EN EL PISO CORRESPONDIENTE
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (players[i].ToBackLayer) // Layer Atras
                        {
                            players[i].offsetFloor = new Vector2((floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize), (floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize));
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }

            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor][x, y].item != null)
                    {
                        bool isDraw = true;
                        tmpX += (TMBaseMap.TileSize * ScreenFloor);
                        tmpY += (TMBaseMap.TileSize * ScreenFloor);

                        var isTile = MapBase.Floors[floor - 1][x + 1, y + 1].item;

                        if (isTile != null)
                        {
                            if (isTile.Id > 1 && isTile.Type == (int)TypeItem.Tile && MapBase.Floors[floor][x, y].item.Type != (int)TypeItem.Tile)
                            {
                                isDraw = false;
                            }
                        }

                        if (isDraw)
                        {
                            mapTile.DrawTileTop(floor, x, y, tmpX, tmpY);
                        }
                    }

                }
            }

            //DUBUJAMOS LOS OTROS PERSONAJES
            for (int i = 0; i < players.Count; i++)
            {
                if (floor == players[i].pos_z) //EN EL PISO CORRESPONDIENTE
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (!players[i].ToBackLayer) // Layer Atras
                        {
                            players[i].offsetFloor = new Vector2((floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize), (floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize));
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }
        }

        void onDrawFloorsUP(int floor) // Floors UP
        {
            int current_floor = player.pos_z;
            int tileOffset = 1;

            tileOffset = (player.pos_z - floor); //Offset para acomodar la vista.

            // DRAW TILE LAYER
            for (int y = ScreenY + tileOffset; y <= ScreenHeight + tileOffset; y++)
            {
                for (int x = ScreenX + tileOffset; x <= ScreenWidth + tileOffset; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor][x, y].item != null)
                    {
                        tmpX -= (TMBaseMap.TileSize * (current_floor - floor));
                        tmpY -= (TMBaseMap.TileSize * (current_floor - floor));

                        bool isDraw = true;

                        var isTile = MapBase.Floors[floor - 1][x + 1, y + 1].item; //Si hay Tile arriba.

                        if (isTile != null)
                        {
                            if (isTile.Id > 1 && isTile.Type == (int)TypeItem.Tile)
                            {
                                isDraw = false; //No lo dibujamos
                            }
                        }

                        if (isDraw)
                        {
                            mapTile.DrawTileBase(floor, x, y, tmpX, tmpY);
                        }
                    }
                }
            }

            //DUBUJAMOS LOS OTROS PERSONAJES
            for (int i = 0; i < players.Count; i++)
            {
                if (floor == players[i].pos_z) //EN EL PISO CORRESPONDIENTE
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (players[i].ToBackLayer) // Layer Atras
                        {
                            players[i].offsetFloor = new Vector2((floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize), (floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize));
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }

            // DRAW TILE LAYER
            for (int y = ScreenY; y <= ScreenHeight; y++)
            {
                for (int x = ScreenX; x <= ScreenWidth; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Screen.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Screen.Y);

                    if (MapBase.Floors[floor][x, y].item != null)
                    {
                        tmpX -= (TMBaseMap.TileSize * (current_floor - floor));
                        tmpY -= (TMBaseMap.TileSize * (current_floor - floor));

                        mapTile.DrawTileTop(floor, x, y, tmpX, tmpY);
                    }
                }
            }

            //DUBUJAMOS LOS OTROS PERSONAJES
            for (int i = 0; i < players.Count; i++)
            {
                if (floor == players[i].pos_z) //EN EL PISO CORRESPONDIENTE
                {
                    var resultX = Math.Abs(player.pos_x - players[i].pos_x);
                    var resultY = Math.Abs(player.pos_y - players[i].pos_y);

                    if (resultX <= 10 && resultY <= 10) //Dibujamos solo si esta en rango de pantalla
                    {
                        if (!players[i].ToBackLayer) // Layer Atras
                        {
                            players[i].offsetFloor = new Vector2((floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize), (floor * TMBaseMap.TileSize) - (player.pos_z * TMBaseMap.TileSize));
                            players[i].Draw(_spriteBatch);
                        }
                    }
                }
            }

        }
    }
}
