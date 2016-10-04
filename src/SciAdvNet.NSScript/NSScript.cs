using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SciAdvNet.NSScript
{
    public sealed class NSScript
    {
        internal NSScript(string name, ImmutableArray<ScriptRoot> syntaxTrees)
        {
            if (syntaxTrees.Length < 1)
            {
                throw new ArgumentException(nameof(syntaxTrees));
            }

            Name = name;
            var main = syntaxTrees[0];
            MainChapter = main.MainChapter;

            var methods = ImmutableArray.CreateBuilder<Method>();
            var scenes = ImmutableArray.CreateBuilder<Scene>();
            methods.AddRange(main.Methods);
            foreach (var dependency in syntaxTrees.Skip(1))
            {
                methods.AddRange(dependency.Methods);
                scenes.AddRange(dependency.Scenes);
            }

            Methods = methods.ToImmutable();
            Scenes = scenes.ToImmutable();
        }

        public string Name { get; }
        public Chapter MainChapter { get; }
        public ImmutableArray<Method> Methods { get; }
        public ImmutableArray<Scene> Scenes { get; }

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var lexer = new Lexer(text);
            SyntaxToken token = null;
            while (token?.Kind != SyntaxTokenKind.EndOfFileToken)
            {
                token = lexer.Lex();
                yield return token;
            }
        }

        public static ScriptRoot ParseScript(string text)
        {
            var parser = new Parser(new Lexer(text));
            return parser.ParseScript();
        }

        public static ScriptRoot ParseScript(string fileName, Stream stream)
        {
            var parser = new Parser(new Lexer(fileName, stream));
            return parser.ParseScript();
        }

        public static Expression ParseExpression(string expression)
        {
            var parser = new Parser(new Lexer(expression));
            return parser.ParseExpression();
        }

        public static Statement ParseStatement(string statement)
        {
            var parser = new Parser(new Lexer(statement));
            return parser.ParseStatement();
        }

        public static DialogueBlock ParseDialogueBlock(string text)
        {
            var parser = new Parser(new Lexer(text));
            return parser.ParseDialogueBlock();
        }
    }
}
