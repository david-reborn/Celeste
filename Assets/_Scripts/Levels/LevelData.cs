using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace myd.celeste
{
    public class LevelData
    {
        public string Solids = "";
        public string Bg = "";
        public string FgTiles = "";
        public string BgTiles = "";
        public string ObjTiles = "";
        public string Music = "";
        public string AltMusic = "";
        public string Ambience = "";
        public float[] MusicLayers = new float[4];
        public int MusicProgress = -1;
        public int AmbienceProgress = -1;
        public string Name;
        public bool Dummy;
        public int Strawberries;
        public bool HasGem;
        public bool HasHeartGem;
        public bool HasCheckpoint;
        public bool DisableDownTransition;
        public Rect Bounds;
        public List<EntityData> Entities;
        public List<EntityData> Triggers;
        public List<Vector2> Spawns;
        public List<DecalData> FgDecals;
        public List<DecalData> BgDecals;
        public WindController.Patterns WindPattern;
        public Vector2 CameraOffset;
        public bool Dark;
        public bool Underwater;
        public bool Space;
        public bool MusicWhispers;
        public bool DelayAltMusic;
        public int EnforceDashNumber;
        public int EditorColorIndex;

        public LevelData(BinaryPacker.Element data)
        {
            this.Bounds = new Rect();
            foreach (KeyValuePair<string, object> attribute in data.Attributes)
            {
                switch (attribute.Key)
                {
                    case "alt_music":
                        this.AltMusic = (string)attribute.Value;
                        break;
                    case "ambience":
                        this.Ambience = (string)attribute.Value;
                        break;
                    case "ambienceProgress":
                        string s1 = attribute.Value.ToString();
                        if (string.IsNullOrEmpty(s1) || !int.TryParse(s1, out this.AmbienceProgress))
                        {
                            this.AmbienceProgress = -1;
                            break;
                        }
                        break;
                    case "c":
                        this.EditorColorIndex = (int)attribute.Value;
                        break;
                    case "cameraOffsetX":
                        this.CameraOffset.x = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                        break;
                    case "cameraOffsetY":
                        this.CameraOffset.y = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                        break;
                    case "dark":
                        this.Dark = (bool)attribute.Value;
                        break;
                    case "delayAltMusicFade":
                        this.DelayAltMusic = (bool)attribute.Value;
                        break;
                    case "disableDownTransition":
                        this.DisableDownTransition = (bool)attribute.Value;
                        break;
                    case "enforceDashNumber":
                        this.EnforceDashNumber = (int)attribute.Value;
                        break;
                    case "height":
                        this.Bounds.height = (int)attribute.Value;
                        if (this.Bounds.height == 184)
                        {
                            this.Bounds.height = 180;
                            break;
                        }
                        break;
                    case "music":
                        this.Music = (string)attribute.Value;
                        break;
                    case "musicLayer1":
                        this.MusicLayers[0] = (bool)attribute.Value ? 1f : 0.0f;
                        break;
                    case "musicLayer2":
                        this.MusicLayers[1] = (bool)attribute.Value ? 1f : 0.0f;
                        break;
                    case "musicLayer3":
                        this.MusicLayers[2] = (bool)attribute.Value ? 1f : 0.0f;
                        break;
                    case "musicLayer4":
                        this.MusicLayers[3] = (bool)attribute.Value ? 1f : 0.0f;
                        break;
                    case "musicProgress":
                        string s2 = attribute.Value.ToString();
                        if (string.IsNullOrEmpty(s2) || !int.TryParse(s2, out this.MusicProgress))
                        {
                            this.MusicProgress = -1;
                            break;
                        }
                        break;
                    case "name":
                        this.Name = attribute.Value.ToString().Substring(4);
                        break;
                    case "space":
                        this.Space = (bool)attribute.Value;
                        break;
                    case "underwater":
                        this.Underwater = (bool)attribute.Value;
                        break;
                    case "whisper":
                        this.MusicWhispers = (bool)attribute.Value;
                        break;
                    case "width":
                        this.Bounds.width = (int)attribute.Value;
                        break;
                    case "windPattern":
                        this.WindPattern = (WindController.Patterns)Enum.Parse(typeof(WindController.Patterns), (string)attribute.Value);
                        break;
                    case "x":
                        this.Bounds.x = (int)attribute.Value;
                        break;
                    case "y":
                        this.Bounds.y = (int)attribute.Value;
                        break;
                }
            }
            this.Spawns = new List<Vector2>();
            this.Entities = new List<EntityData>();
            this.Triggers = new List<EntityData>();
            this.BgDecals = new List<DecalData>();
            this.FgDecals = new List<DecalData>();
            foreach (BinaryPacker.Element child1 in data.Children)
            {
                if (child1.Name == "entities")
                {
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                        {
                            if (child2.Name == "player")
                                this.Spawns.Add(new Vector2((float)this.Bounds.x + Convert.ToSingle(child2.Attributes["x"], (IFormatProvider)CultureInfo.InvariantCulture), (float)this.Bounds.y + Convert.ToSingle(child2.Attributes["y"], (IFormatProvider)CultureInfo.InvariantCulture)));
                            else if (child2.Name == "strawberry" || child2.Name == "snowberry")
                                ++this.Strawberries;
                            else if (child2.Name == "shard")
                                this.HasGem = true;
                            else if (child2.Name == "blackGem")
                                this.HasHeartGem = true;
                            else if (child2.Name == "checkpoint")
                                this.HasCheckpoint = true;
                            if (!child2.Name.Equals("player"))
                                this.Entities.Add(this.CreateEntityData(child2));
                        }
                    }
                }
                else if (child1.Name == "triggers")
                {
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                            this.Triggers.Add(this.CreateEntityData(child2));
                    }
                }
                else if (child1.Name == "bgdecals")
                {
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                            this.BgDecals.Add(new DecalData()
                            {
                                Position = new Vector2(Convert.ToSingle(child2.Attributes["x"], (IFormatProvider)CultureInfo.InvariantCulture), Convert.ToSingle(child2.Attributes["y"], (IFormatProvider)CultureInfo.InvariantCulture)),
                                Scale = new Vector2(Convert.ToSingle(child2.Attributes["scaleX"], (IFormatProvider)CultureInfo.InvariantCulture), Convert.ToSingle(child2.Attributes["scaleY"], (IFormatProvider)CultureInfo.InvariantCulture)),
                                Texture = (string)child2.Attributes["texture"]
                            });
                    }
                }
                else if (child1.Name == "fgdecals")
                {
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                            this.FgDecals.Add(new DecalData()
                            {
                                Position = new Vector2(Convert.ToSingle(child2.Attributes["x"], (IFormatProvider)CultureInfo.InvariantCulture), Convert.ToSingle(child2.Attributes["y"], (IFormatProvider)CultureInfo.InvariantCulture)),
                                Scale = new Vector2(Convert.ToSingle(child2.Attributes["scaleX"], (IFormatProvider)CultureInfo.InvariantCulture), Convert.ToSingle(child2.Attributes["scaleY"], (IFormatProvider)CultureInfo.InvariantCulture)),
                                Texture = (string)child2.Attributes["texture"]
                            });
                    }
                }
                else if (child1.Name == "solids")
                    this.Solids = child1.Attr("innerText", "");
                else if (child1.Name == "bg")
                    this.Bg = child1.Attr("innerText", "");
                else if (child1.Name == "fgtiles")
                    this.FgTiles = child1.Attr("innerText", "");
                else if (child1.Name == "bgtiles")
                    this.BgTiles = child1.Attr("innerText", "");
                else if (child1.Name == "objtiles")
                    this.ObjTiles = child1.Attr("innerText", "");
            }
            this.Dummy = this.Spawns.Count <= 0;
        }

        private EntityData CreateEntityData(BinaryPacker.Element entity)
        {
            EntityData entityData = new EntityData();
            entityData.Name = entity.Name;
            entityData.Level = this;
            if (entity.Attributes != null)
            {
                foreach (KeyValuePair<string, object> attribute in entity.Attributes)
                {
                    if (attribute.Key == "id")
                        entityData.ID = (int)attribute.Value;
                    else if (attribute.Key == "x")
                        entityData.Position.x = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                    else if (attribute.Key == "y")
                        entityData.Position.y = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                    else if (attribute.Key == "width")
                        entityData.Width = (int)attribute.Value;
                    else if (attribute.Key == "height")
                        entityData.Height = (int)attribute.Value;
                    else if (attribute.Key == "originX")
                        entityData.Origin.x = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                    else if (attribute.Key == "originY")
                    {
                        entityData.Origin.y = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (entityData.Values == null)
                            entityData.Values = new Dictionary<string, object>();
                        entityData.Values.Add(attribute.Key, attribute.Value);
                    }
                }
            }
            entityData.Nodes = new Vector2[entity.Children == null ? 0 : entity.Children.Count];
            for (int index = 0; index < entityData.Nodes.Length; ++index)
            {
                foreach (KeyValuePair<string, object> attribute in entity.Children[index].Attributes)
                {
                    if (attribute.Key == "x")
                        entityData.Nodes[index].x = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                    else if (attribute.Key == "y")
                        entityData.Nodes[index].y = Convert.ToSingle(attribute.Value, (IFormatProvider)CultureInfo.InvariantCulture);
                }
            }
            return entityData;
        }

        public bool Check(Vector2 at)
        {
            return (double)at.x >= (double)this.Bounds.xMin && (double)at.y >= (double)this.Bounds.yMin && (double)at.x < (double)this.Bounds.xMax && (double)at.y < (double)this.Bounds.yMax;
        }

        public Rect TileBounds
        {
            get
            {
                return new Rect(this.Bounds.x / 8, this.Bounds.y / 8, (int)Math.Ceiling((double)this.Bounds.width / 8.0), (int)Math.Ceiling((double)this.Bounds.height / 8.0));
            }
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2((float)this.Bounds.x, (float)this.Bounds.y);
            }
            set
            {
                for (int index = 0; index < this.Spawns.Count; ++index)
                    this.Spawns[index] -= this.Position;
                this.Bounds.x = (int)value.x;
                this.Bounds.y = (int)value.y;
                for (int index = 0; index < this.Spawns.Count; ++index)
                    this.Spawns[index] += this.Position;
            }
        }

        public int LoadSeed
        {
            get
            {
                int num = 0;
                foreach (char ch in this.Name)
                    num += (int)ch;
                return num;
            }
        }
    }
}