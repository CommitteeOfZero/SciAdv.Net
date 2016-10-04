﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace SciAdvNet.NSScript
{
    public static class SyntaxFacts
    {
        private static readonly Dictionary<string, SyntaxTokenKind> s_keywords = new Dictionary<string, SyntaxTokenKind>()
        {
            ["chapter"] = SyntaxTokenKind.ChapterKeyword,
            ["function"] = SyntaxTokenKind.FunctionKeyword,
            ["scene"] = SyntaxTokenKind.SceneKeyword,
            ["call_scene"] = SyntaxTokenKind.CallSceneKeyword,
            ["call_chapter"] = SyntaxTokenKind.CallChapterKeyword,
            ["null"] = SyntaxTokenKind.NullKeyword,
            ["true"] = SyntaxTokenKind.TrueKeyword,
            ["false"] = SyntaxTokenKind.FalseKeyword,
            ["while"] = SyntaxTokenKind.WhileKeyword,
            ["if"] = SyntaxTokenKind.IfKeyword,
            ["else"] = SyntaxTokenKind.ElseKeyword,
            ["select"] = SyntaxTokenKind.SelectKeyword,
            ["case"] = SyntaxTokenKind.CaseKeyword,
            ["break"] = SyntaxTokenKind.BreakKeyword,
            ["return"] = SyntaxTokenKind.ReturnKeyword
        };

        public static bool IsLetter(char c) => char.IsLetter(c);
        public static bool IsLatinLetter(char c) => c >= 'A' && c <= 'z';
        public static bool IsDecDigit(char c) => c >= '0' && c <= '9';
        public static bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }

        public static bool IsWhitespace(char c)
        {
            // SPACE
            // CHARACTER TABULATION (U+0009)
            // LINE TABULATION (U+000B)
            // FORM FEED (U+000C)
            // Any other character with Unicode class Zs

            return c == ' '
                || c == '\t'
                || c == '\v'
                || c == '\f'
                || c == '\u001A'
                || (c > 255 && CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator);
        }

        public static bool IsNewLine(char c)
        {
            // CR, LF
            // NEXT LINE (U+0085)
            // LINE SEPARATOR (U+2028)
            // PARAGRAPH SEPARATOR (U+2029)

            return c == '\r'
                || c == '\n'
                || c == '\u0085'
                || c == '\u2028'
                || c == '\u2029';
        }

        public static bool IsSigil(char c)
        {
            switch (c)
            {
                case '$':
                case '#':
                case '@':
                    return true;

                default:
                    return false;
            }
        }

        public static SyntaxTokenKind GetKeywordKind(string keyword)
        {
            SyntaxTokenKind kind = SyntaxTokenKind.None;
            s_keywords.TryGetValue(keyword.ToLowerInvariant(), out kind);
            return kind;
        }

        public static bool IsIdentifierPartCharacter(char c)
        {
            switch (c)
            {
                case ' ':
                case '"':
                case '\t':
                case '\r':
                case '\n':
                case ',':
                case ':':
                case ';':
                case '{':
                case '}':
                case '(':
                case ')':
                case '=':
                case '+':
                case '-':
                case '*':
                case '<':
                case '>':
                case '%':
                case '!':
                case '|':
                case '&':
                    return false;

                default:
                    return true;
            }
        }

        public static bool IsIdentifierStopCharacter(char c) => !IsIdentifierPartCharacter(c);

        public static bool IsPrefixUnaryOperator(SyntaxTokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxTokenKind.ExclamationToken:
                case SyntaxTokenKind.PlusToken:
                case SyntaxTokenKind.MinusToken:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsPostfixUnaryOperator(SyntaxTokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxTokenKind.PlusPlusToken:
                case SyntaxTokenKind.MinusMinusToken:
                    return true;

                default:
                    return false;
            }
        }

        public static UnaryOperationKind GetPrefixUnaryOperationKind(SyntaxTokenKind operatorTokenKind)
        {
            switch (operatorTokenKind)
            {
                case SyntaxTokenKind.ExclamationToken:
                    return UnaryOperationKind.LogicalNegation;
                case SyntaxTokenKind.PlusToken:
                    return UnaryOperationKind.UnaryPlus;
                case SyntaxTokenKind.MinusToken:
                    return UnaryOperationKind.UnaryMinus;

                default:
                    throw new ArgumentException(nameof(operatorTokenKind));
            }
        }

        public static UnaryOperationKind GetPostfixUnaryOperationKind(SyntaxTokenKind operatorTokenKind)
        {
            switch (operatorTokenKind)
            {
                case SyntaxTokenKind.PlusPlusToken:
                    return UnaryOperationKind.PostfixIncrement;
                case SyntaxTokenKind.MinusMinusToken:
                    return UnaryOperationKind.PostfixDecrement;

                default:
                    throw new ArgumentException(nameof(operatorTokenKind));
            }
        }

        public static bool IsBinaryOperator(SyntaxTokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxTokenKind.PlusToken:
                case SyntaxTokenKind.MinusToken:
                case SyntaxTokenKind.AsteriskToken:
                case SyntaxTokenKind.SlashToken:
                case SyntaxTokenKind.LessThanToken:
                case SyntaxTokenKind.LessThanEqualsToken:
                case SyntaxTokenKind.GreaterThanToken:
                case SyntaxTokenKind.GreaterThanEqualsToken:
                case SyntaxTokenKind.BarBarToken:
                case SyntaxTokenKind.AmpersandAmpersandToken:
                case SyntaxTokenKind.EqualsEqualsToken:
                case SyntaxTokenKind.ExclamationEqualsToken:
                    return true;

                default:
                    return false;
            }
        }

        public static BinaryOperationKind GetBinaryOperationKind(SyntaxTokenKind operatorTokenKind)
        {
            switch (operatorTokenKind)
            {
                case SyntaxTokenKind.PlusToken:
                    return BinaryOperationKind.Addition;
                case SyntaxTokenKind.MinusToken:
                    return BinaryOperationKind.Subtraction;
                case SyntaxTokenKind.AsteriskToken:
                    return BinaryOperationKind.Multiplication;
                case SyntaxTokenKind.SlashToken:
                    return BinaryOperationKind.Division;
                case SyntaxTokenKind.LessThanToken:
                    return BinaryOperationKind.LessThan;
                case SyntaxTokenKind.LessThanEqualsToken:
                    return BinaryOperationKind.LessThanOrEqual;
                case SyntaxTokenKind.GreaterThanToken:
                    return BinaryOperationKind.GreaterThan;
                case SyntaxTokenKind.GreaterThanEqualsToken:
                    return BinaryOperationKind.GreaterThanOrEqual;
                case SyntaxTokenKind.BarBarToken:
                    return BinaryOperationKind.LogicalOr;
                case SyntaxTokenKind.AmpersandAmpersandToken:
                    return BinaryOperationKind.LogicalAnd;
                case SyntaxTokenKind.EqualsEqualsToken:
                    return BinaryOperationKind.Equal;
                case SyntaxTokenKind.ExclamationEqualsToken:
                    return BinaryOperationKind.NotEqual;

                default:
                    throw new ArgumentException(nameof(operatorTokenKind));
            }
        }

        public static bool IsAssignmentOperator(SyntaxTokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxTokenKind.EqualsToken:
                case SyntaxTokenKind.PlusEqualsToken:
                case SyntaxTokenKind.MinusEqualsToken:
                case SyntaxTokenKind.AsteriskEqualsToken:
                case SyntaxTokenKind.SlashEqualsToken:
                    return true;

                default:
                    return false;
            }
        }

        public static AssignmentOperationKind GetAssignmentOperationKind(SyntaxTokenKind operatorTokenKind)
        {
            switch (operatorTokenKind)
            {
                case SyntaxTokenKind.EqualsToken:
                    return AssignmentOperationKind.SimpleAssignment;
                case SyntaxTokenKind.PlusEqualsToken:
                    return AssignmentOperationKind.AddAssignment;
                case SyntaxTokenKind.MinusEqualsToken:
                    return AssignmentOperationKind.SubtractAssignment;
                case SyntaxTokenKind.AsteriskEqualsToken:
                    return AssignmentOperationKind.MultiplyAssignment;
                case SyntaxTokenKind.SlashEqualsToken:
                    return AssignmentOperationKind.DivideAssignment;

                default:
                    throw new ArgumentException(nameof(operatorTokenKind));
            }
        }

        public static string GetText(SyntaxTokenKind kind)
        {
            switch (kind)
            {
                case SyntaxTokenKind.ChapterKeyword:
                    return "chapter";
                case SyntaxTokenKind.FunctionKeyword:
                    return "function";
                case SyntaxTokenKind.SceneKeyword:
                    return "scene";
                case SyntaxTokenKind.CallSceneKeyword:
                    return "call_scene";
                case SyntaxTokenKind.CallChapterKeyword:
                    return "call_chapter";
                case SyntaxTokenKind.NullKeyword:
                    return "null";
                case SyntaxTokenKind.TrueKeyword:
                    return "true";
                case SyntaxTokenKind.FalseKeyword:
                    return "false";
                case SyntaxTokenKind.WhileKeyword:
                    return "while";
                case SyntaxTokenKind.IfKeyword:
                    return "if";
                case SyntaxTokenKind.ElseKeyword:
                    return "else";
                case SyntaxTokenKind.SelectKeyword:
                    return "select";
                case SyntaxTokenKind.CaseKeyword:
                    return "case";
                case SyntaxTokenKind.BreakKeyword:
                    return "break";

                default:
                    return string.Empty;
            }
        }
    }
}
