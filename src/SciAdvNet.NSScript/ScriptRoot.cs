using System;
using System.Collections.Immutable;

namespace SciAdvNet.NSScript
{
    public sealed class ScriptRoot : SyntaxNode
    {
        internal ScriptRoot(Chapter mainChapter, ImmutableArray<Method> methods,
            ImmutableArray<Scene> scenes, ImmutableArray<string> includes)
        {
            MainChapter = mainChapter;
            Methods = methods;
            Scenes = scenes;
            Includes = includes;
        }

        public Chapter MainChapter { get; }
        public ImmutableArray<Method> Methods { get; }
        public ImmutableArray<Scene> Scenes { get; }
        public ImmutableArray<string> Includes { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.ScriptRoot;

        public override void Accept(SyntaxVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
