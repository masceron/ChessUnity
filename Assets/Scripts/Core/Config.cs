using UnityEngine;

namespace Core
{
    public class Config: ScriptableObject
    {
        public static PieceType[] boardConfig = new[]
        {
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Brook, PieceType.BKnight, PieceType.BBishop, PieceType.Bqueen, PieceType.Bking, PieceType.BBishop, PieceType.BKnight, PieceType.Brook, PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.BPawn, PieceType.BPawn,   PieceType.BPawn,   PieceType.BBishop,  PieceType.BBishop, PieceType.BPawn,   PieceType.BPawn,   PieceType.BPawn, PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.WPawn, PieceType.WKnight,   PieceType.WPawn,   PieceType.WPawn,  PieceType.WPawn, PieceType.WPawn,   PieceType.WKnight,   PieceType.WPawn, PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Wrook, PieceType.WKnight, PieceType.WBishop, PieceType.Wqueen, PieceType.Wking, PieceType.WBishop, PieceType.WKnight, PieceType.Wrook, PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
            PieceType.Nil, PieceType.Nil, PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,    PieceType.Nil,   PieceType.Nil,     PieceType.Nil,     PieceType.Nil,   PieceType.Nil, PieceType.Nil,
        };

        public static bool[] boardActive = new[]
        {
            false, false, false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, true, true, true, true, true, true, true, true, false, false,
            false, false, false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false, false, false
        };
    }
}