using SciAdvNet.Engine;
using System.Collections.Generic;

namespace ChaosHeadNoah
{
    //public sealed class NssExports : SciAdvNet.NSScript.NssExports
    //{
    //    private Dictionary<string, Asset> _assets = new Dictionary<string, Asset>();
    //    private readonly Game _game;

    //    public NssExports(Game game)
    //    {
    //        _game = game;
    //    }

    //    public override void LoadTexture(string assetKey, int zLevel, int x, int y, string fileName)
    //    {
    //        var stream = _game.GetAsset(fileName);
    //        var texture = _game.Assets.LoadAsset<Texture>(fileName, stream);
    //        _assets[assetKey] = texture;
    //    }

    //    public override void PlaySound(string assetKey, int duration, int volume, int speed, bool loop)
    //    {
    //        //string fileName = _assets[assetKey];
    //        //var stream = _game.AssetManager.GetAudio(fileName);
    //        //_audioSubsystem.PlaySound(stream);
    //    }

    //    public void CreateColor(string assetKey, int zLevel, int x, int y, int width, int height, string colorName)
    //    {
    //        //var rectangle = new RectangleVisual(x, y, width, height, color);
    //        //_game.Renderer.AddVisual(rectangle);
    //    }

    //    public void Dialogue()
    //    {
    //        _game.Interact();
    //    }
    //}
}
