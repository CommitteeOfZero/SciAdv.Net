using System;
using System.Collections.Immutable;

namespace SciAdvNet.NSScript
{
    public interface INssExports
    {
        void Unknown(string methodName, ImmutableArray<ConstantValue> args);

        void SetAlias(string symbol, string alias);
        void Wait(TimeSpan delay);
        void WaitForClick();
        void WaitForClick(TimeSpan timeout);

        // loading assets
        void LoadTexture(string symbol, string fileName);
        void LoadAudio(string symbol, AudioKind kind, string fileName);

        void CreateDialogueWindow(string symbol, int zLevel, int x, int y, int width, int height);

        void DrawText(string symbol, int zLevel, int x, int y, int width, int height, string text);
        void DrawTexture(string symbol, int zLevel, int x, int y, string fileNameOrSymbol);
        void DrawRectangle(string symbol, int zLevel, int x, int y, int width, int height, string colorName);

        void SetLoopPoint(string symbol, TimeSpan loopStart, TimeSpan loopEnd);
        void SetLoop(string symbol, bool loop);
        void SetVolume(string symbol, TimeSpan duration, int volume);

        void FadeIn(string symbol, TimeSpan duration, int finalOpacity, bool wait);
        void Move(string symbol, TimeSpan duration, int x, int y, bool wait);
        void Zoom(string symbol, TimeSpan duration, int x, int y, bool wait);

        void Delete(string symbol);
        
        void Request(string symbol, NssAction action);
        void CreateChoice(string symbol);
        
        void PlayCutscene(string symbol, int zLevel, bool loop, bool alpha, string fileName, bool enableAudio);
        
        void DrawTransition(int time, int start, int end, int unk, string filename, bool wait);
        
        //void SetVolume(string symbol, )
        //void CrateMask
    }

    public enum AudioKind
    {
        BackgroundMusic,
        SoundEffect
    }

    public enum NssAction
    {
        Unlock,
        Lock
    }
}
