using System;
using System.Collections.Immutable;

namespace SciAdvNet.NSScript
{
    public class NssExports : INssExports
    {
        public virtual void Unknown(string methodName, ImmutableArray<ConstantValue> args)
        {
        }

        public virtual void CreateChoice(string symbol)
        {
        }

        public virtual void DrawText(string symbol, int zLevel, int x, int y, int width, int height, string text)
        {
        }

        public virtual void CreateDialogueWindow(string symbol, int zLevel, int x, int y, int width, int height)
        {
        }

        public virtual void Delete(string symbol)
        {
        }

        public virtual void DrawRectangle(string symbol, int zLevel, int x, int y, int width, int height, string colorName)
        {
        }

        public virtual void DrawTexture(string symbol, int zLevel, int x, int y, string fileNameOrSymbol)
        {
        }

        public virtual void DrawTransition(int time, int start, int end, int unk, string filename, bool wait)
        {
        }

        public virtual void FadeIn(string symbol, TimeSpan duration, int opacity, bool wait)
        {
        }

        public virtual void LoadAudio(string symbol, AudioKind kind, string fileName)
        {
        }

        public virtual void PlayCutscene(string symbol, int zLevel, bool loop, bool alpha, string fileName, bool enableAudio)
        {
        }

        public virtual void LoadTexture(string symbol, string fileName)
        {
        }

        public virtual void Move(string symbol, TimeSpan duration, int x, int y, bool wait)
        {
        }

        public virtual void Request(string symbol, NssAction action)
        {
        }

        public virtual void SetAlias(string symbol, string alias)
        {
        }

        public virtual void SetLoop(string symbol, bool loop)
        {
        }

        public virtual void SetLoopPoint(string symbol, TimeSpan loopStart, TimeSpan loopEnd)
        {
        }

        public virtual void SetVolume(string symbol, TimeSpan duration, int volume)
        {
        }

        public virtual void Wait(TimeSpan delay)
        {
        }

        public virtual void WaitForClick()
        {
        }

        public virtual void WaitForClick(TimeSpan timeout)
        {
        }

        public virtual void Zoom(string symbol, TimeSpan duration, int x, int y, bool wait)
        {
        }
    }
}
