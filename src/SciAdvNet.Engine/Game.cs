using System;
using System.IO;

namespace SciAdvNet.Engine
{
    public abstract class Game : IDisposable
    {
        private bool _isRunning;
        private readonly GameWindow _gameWindow;

        public Game(Platform platform, GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
            switch (platform)
            {
                case Platform.WindowsUniversal:
                    Renderer = new D2DRenderer(gameWindow);
                    break;
            }

            Assets = new AssetManager(Renderer);
        }

        public AssetManager Assets { get; }
        public D2DRenderer Renderer { get; }

        public abstract Stream GetAsset(string assetName);

        public void Interact()
        {
            _isRunning = true;
            while (_isRunning)
            {
                _gameWindow.ProcessEventsFunc();
                Renderer.RenderFrame();
            }
        }

        public void Dispose()
        {
            _isRunning = false;
            Renderer.Dispose();
        }
    }
}
