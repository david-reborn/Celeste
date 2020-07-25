using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace myd.celeste
{
    public class Gameplay : MonoBehaviour
    {
        public static Gameplay instance;

        public Texture2D TileSet;
        private Dictionary<string, ExtSprite> Images = new Dictionary<string, ExtSprite>(StringComparer.OrdinalIgnoreCase);
        //private AutoTiler foreground, background;

        public string name;
        public Sprite sprite;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            ParseItems();
            //foreground = new AutoTiler(Util.ReadResource("Resources.ForegroundTiles.xml"));
            //background = new AutoTiler(Util.ReadResource("Resources.BackgroundTiles.xml"));
            Debug.Log(Images.Count);

            int i = 0;
            foreach (string key in Images.Keys)
            {
                i++;
                Debug.Log(key);
                if (i > 1000)
                {
                    break;
                }
            }

            

        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                Debug.Log(111);
                sprite = GetImage(name);
            }
        }


        private void ParseItems()
        {
            using (Stream stream = Util.ReadResourceStream("Gameplay.meta"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.ReadInt32();
                    reader.ReadString();
                    reader.ReadInt32();

                    int count = reader.ReadInt16();
                    for (int i = 0; i < count; i++)
                    {
                        string dataFile = reader.ReadString();
                        int spriteCount = reader.ReadInt16();
                        for (int j = 0; j < spriteCount; j++)
                        {
                            string path = reader.ReadString().Replace('\\', '/');
                            if (path.Equals("bgs/01/bg0"))
                            {
                                Debug.Log(111);
                            }
                            int x = reader.ReadInt16();
                            int y = reader.ReadInt16();
                            int width = reader.ReadInt16();
                            int height = reader.ReadInt16();

                            int xOffset = reader.ReadInt16();
                            int yOffset = reader.ReadInt16();
                            int realWidth = reader.ReadInt16();
                            int realHeight = reader.ReadInt16();

                            Images.Add(path, new ExtSprite(x, 4096-y, width, height, xOffset, yOffset, realWidth, realHeight));
                        }
                    }
                }
            }
        }
        public Sprite GetImage(string path)
        {
            if (!Images.TryGetValue(path, out ExtSprite bounds))
            {
                return null;
            }
            return Util.GetSubImage(TileSet, bounds);
        }

    }
}