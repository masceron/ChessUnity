using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Triggers;
using Unity.IL2CPP.CompilerServices;

namespace Core
{
    public enum PieceType: sbyte
    {
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
        NormalCapture,
        VelkarisMark,
        VelkarisKill
    }

    public enum Effect : byte
    {
        None,
        Shield,
        Evasion,
        HardenedShield,
        VelkarisMarked,
    }

    public struct TriggerElement : IEquatable<TriggerElement>, IComparable<TriggerElement>
    {
        public Trigger Trigger;
        public sbyte Priority;

        public bool Equals(TriggerElement other)
        {
            return Equals(Trigger, other.Trigger);
        }

        public int CompareTo(TriggerElement other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public override bool Equals(object obj)
        {
            return obj is TriggerElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Trigger != null ? Trigger.GetHashCode() : 0);
        }
    }
    
    public class PieceData
    {
        public byte Pos;
        public readonly PieceType Type;
        public readonly Color Color;
        public sbyte SkillCooldown = -1;
        public readonly List<Effect> Effects = new();
        public readonly List<TriggerElement> Triggers = new();
        
        public PieceData(PieceType type, Color color, byte pos)
        {
            Type = type;
            Color = color;
            Pos = pos;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Position
    {
        public PieceData[] main_board;
        public byte[] active_board;
        public Color side_to_move;
        public ushort ply;
        public List<TriggerElement> triggers;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Move
    {
        public byte from;
        public byte to;
        public MoveFlag flag;

        public bool DoesMoveChangePos()
        {
            return flag is MoveFlag.NormalCapture or MoveFlag.NormalMove;
        }
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public Position Position;
        public Color OurSide;
        public readonly int MaxRank;
        public readonly int MaxFile;

        public GameState(int maxRank, int maxFile, List<PieceConfig> configs, byte[] ac, Color side, Color ourSide)
        {
            OurSide = ourSide;
            MaxFile = maxFile;
            MaxRank = maxRank;
            
            Position = new Position
            {
                main_board = new PieceData[maxRank * maxFile],
                active_board = new byte[maxRank * maxFile],
                triggers = new List<TriggerElement>(),
                side_to_move = side
            };
            Array.Copy(ac, Position.active_board, maxRank * maxFile);

            foreach (var piece in configs)
            {
                var p = new PieceData(piece.Type, piece.Color, piece.Index);
                Position.main_board[piece.Index] = p;
                MakeTrigger(p, Position.triggers, this);
            }
            
            Position.triggers.Sort((a, b) => b.CompareTo(a));
        }

        private static void MakeTrigger(PieceData piece, List<TriggerElement> triggers, GameState state)
        {
            TriggerElement trigger;
            switch (piece.Type)
            {
                case PieceType.Velkaris:
                    trigger = new TriggerElement
                    {
                        Trigger = new VelkarisMarker(piece, state),
                        Priority = 0
                    };
                    break;
                default:
                    return;
            }
            triggers.Add(trigger);
            piece.Triggers.Add(trigger);
        }

        public bool IsLegal(Move move)
        {
            return true;
        }

        private static void RemoveTrigger(PieceData piece, List<TriggerElement> list)
        {
            foreach (var toRemove in piece.Triggers)
            {
                list.Remove(toRemove);
            }
        }

        public int Execute(Move move)
        {
            var flag = move.flag;
            var from = move.from;
            var to = move.to;
            var pieceToMove = Position.main_board[from];
            var pieceAffected = Position.main_board[to];
            switch (flag)
            {
                case MoveFlag.SwitchSide:
                    Position.side_to_move = Position.side_to_move == Color.White ? Color.Black : Color.White;
                    break;
                case MoveFlag.NormalMove:
                    Position.main_board[to] = pieceToMove;
                    Position.main_board[to].Pos = to;
                    Position.main_board[from] = null;
                    break;
                case MoveFlag.NormalCapture:
                    RemoveTrigger(pieceAffected, Position.triggers);
                    Position.main_board[to] = pieceToMove;
                    Position.main_board[to].Pos = to;
                    Position.main_board[from] = null;
                    break;
                case MoveFlag.VelkarisKill:
                    RemoveTrigger(pieceAffected, Position.triggers);
                    Position.main_board[to] = null;
                    pieceToMove.SkillCooldown = -1;
                    break;
                case MoveFlag.VelkarisMark:
                    Position.main_board[to].Effects.Add(Effect.VelkarisMarked);
                    pieceToMove.SkillCooldown = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var triggerToRemove = Position.triggers.Where(trigger => trigger.Trigger.CallTrigger(pieceToMove, move)).ToList();

            foreach (var trigger in triggerToRemove)
            {
                trigger.Trigger.Piece.Triggers.Remove(trigger);
                Position.triggers.Remove(trigger);
            }

            return 0;
        } 
    }
}
