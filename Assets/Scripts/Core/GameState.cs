using System;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;

namespace Core
{
    public enum PieceType: byte
    {
        WPawn, WKnight, WBishop, Wrook, Wqueen, Wking,
        BPawn, BKnight, BBishop, Brook, Bqueen, Bking,

        Nil
    }
    
    public enum Color: byte
    {
        White, Black
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Position
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144, ArraySubType = UnmanagedType.U1)] public byte[] main_board;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144, ArraySubType = UnmanagedType.U1)] public byte[] active_board;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U1)] public byte[] count_white;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U1)] public byte[] count_black;
        [MarshalAs(UnmanagedType.U1)] public bool side_to_move;
        [MarshalAs(UnmanagedType.U1)] public byte castling_rights;
        [MarshalAs(UnmanagedType.U2)] public ushort ply;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Move
    {
        [MarshalAs(UnmanagedType.U2)] public ushort from_to;
        [MarshalAs(UnmanagedType.U1)] public byte flag;
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        private Position position;
        public readonly Color OurSide = Color.White;

        public GameState(int maxRank, int maxFile, int pieceType)
        {
            position = new Position
            {
                main_board = new byte[maxRank * maxFile],
                active_board = new byte[maxRank * maxFile],
                count_white = new byte[pieceType],
                count_black = new byte[pieceType]
            };
        }
        
        [DllImport("libInternal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void set_board(ref Position pos);

        [DllImport("libInternal", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool is_legal(ref Position pos, ref Move move);

        [DllImport("libInternal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int make_move(ref Position pos, ref Move move);

        public bool IsLegal(Move move)
        {
            return true;

            //return is_legal(ref position, ref move);
        }

        public int Execute(Move move)
        {
            return make_move(ref position, ref move);
        } 
    }
}
