using UnityEngine;
using System.Collections;
using System.IO;
using myd.celeste;

public class MyGameLoader : MonoBehaviour
{
    public static MyGameLoader instance;

    public GameObject spritePrefab;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //读取配置文件
        Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
        Gfx.FGAutotiler = new Autotiler(Path.Combine("Graphics", "ForegroundTiles.xml"));

        //MTexture mTexture = Gfx.Game["tilesets/dirt"];
        //草地等等
        //MTexture mTexture = Gfx.Game["tilesets/scenery"];
        //MyGameLoader.instance.ShowSprite(mTexture);
    }

    void Update()
    {

    }

    private void ShowSprite(MTexture mTexture, string name = null)
    {
        GameObject gameObject = Instantiate(spritePrefab);
        if (name != null)
        {
            gameObject.name = name;
        }
        gameObject.transform.SetParent(this.transform, false);
        gameObject.GetComponent<SpriteRenderer>().sprite = mTexture.GetSprite();
    }
}
