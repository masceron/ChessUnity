using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;

namespace Core
{
    public enum PieceType: sbyte
    {
        Nil = -1,
        
        Velkaris,
        
    }
    
    public enum Color: byte
    {
        White, Black
    }

    public enum MoveFlag : byte
    {
        SwitchSide,
        NormalMove,
        NormalCapture
    }

    public enum Effect : byte
    {
        None,
        Shield,
        Evasion,
        HardenedShield,
    }
    
    public class PieceData
    {
        public readonly PieceType Type;
        public readonly Color Color;
        public sbyte SkillCooldown = -1;
        public Effect Effect = Effect.None;
        
        public PieceData(PieceType type, Color color)
        {
            Type = type;
            Color = color;
        }

        public PieceData()
        {
            Type = PieceType.Nil;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Position
    {
        public PieceData[] main_board;
        public byte[] active_board;
        public Color side_to_move;
        public ushort ply;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Move
    {
        public byte from;
        public byte to;
        public MoveFlag flag;
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public Position Position;
        public readonly Color OurSide = Color.White;

        public GameState(int maxRank, int maxFile, List<PieceConfig> configs, byte[] ac, Color side)
        {
            Position = new Position
            {
                main_board = new PieceData[maxRank * maxFile],
                active_board = new byte[maxRank * maxFile]
            };
            Array.Copy(ac, Position.active_board, maxRank * maxFile);

            foreach (var piece in configs)
            {
                Position.main_board[piece.Index] = new PieceData(piece.Type, piece.Color);
            }

            for (int i = 0; i < maxRank * maxFile; i++)
            {
                Position.main_board[i] ??= new PieceData();
            }
            
            Position.side_to_move = side;
        }

        public bool IsLegal(Move move)
        {
            return true;

            //return is_legal(ref position, ref move);
        }

        public int Execute(Move move)
        {
            var flag = move.flag;
            var from = move.from;
            var to = move.to;
            switch (flag)
            {
                case MoveFlag.SwitchSide:
                    Position.side_to_move = Position.side_to_move == Color.White ? Color.Black : Color.White;
                    break;
                case MoveFlag.NormalMove:
                    Position.main_board[to] = Position.main_board[from];
                    Position.main_board[from] = new PieceData();
                    break;
                case MoveFlag.NormalCapture:
                    Position.main_board[to] = Position.main_board[from];
                    Position.main_board[from] = new PieceData();
                    break;
            }

            return 0;
        } 
    }
}
