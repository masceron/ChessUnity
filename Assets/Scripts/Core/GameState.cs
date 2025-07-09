using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Board.Action;
using Core.Triggers;
using Unity.IL2CPP.CompilerServices;
using Action = Board.Action.Action;

namespace Core
{
    public enum PieceType: sbyte
    {
        Velkaris,
        GuidingSiren
    }
    
    public enum Color: byte
    {
        White, Black
    }

    public enum EffectType : byte
    {
        None,
        Shield,
        Evasion,
        HardenedShield,
        VelkarisMarked,
        Slow,
        Controlled
    }

    public struct Effect : IEquatable<Effect>
    {
        public readonly EffectType EffectType;
        public sbyte Duration;
        public byte Strength;

        public Effect(EffectType e, sbyte d, byte s)
        {
            EffectType = e;
            Duration = d;
            Strength = s;
        }

        public bool Equals(Effect other)
        {
            return EffectType == other.EffectType;
        }

        public override bool Equals(object obj)
        {
            return obj is Effect other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)EffectType;
        }
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
        public ushort Pos;
        public readonly PieceType Type;
        public Color Color;
        public sbyte SkillCooldown;
        public readonly List<Effect> Effects;
        public readonly List<TriggerElement> Triggers = new();
        
        public PieceData(PieceType type, Color color, ushort pos, List<Effect> effects)
        {
            Type = type;
            Color = color;
            Pos = pos;
            Effects = effects;
        }
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public Color OurSide;
        public readonly int MaxRank;
        public readonly int MaxFile;
        public static readonly Effect Slow = new(EffectType.Slow, 0, 1);
        
        public readonly PieceData[] MainBoard;
        public readonly BitArray ActiveBoard;
        public Color SideToMove;
        public ushort Ply;
        private readonly List<TriggerElement> triggers;
        public Action LastMove;
        public PieceData LastMovedPiece;
        
        public GameState(int maxRank, int maxFile, List<PieceConfig> configs, byte[] ac, Color side, Color ourSide)
        {
            OurSide = ourSide;
            MaxFile = maxFile;
            MaxRank = maxRank;
            LastMove = new SwitchSide();
            LastMovedPiece = null;

            MainBoard = new PieceData[maxRank * maxFile];
            ActiveBoard = new BitArray(maxRank * maxFile);
            triggers = new List<TriggerElement>();
            SideToMove = side;

            for (var i = 0; i < ac.Length; i++)
            {
                if (ac[i] == 0) ActiveBoard[i] = false;
                else ActiveBoard[i] = true;
            }

            foreach (var piece in configs)
            {
                var p = new PieceData(piece.Type, piece.Color, piece.Index, piece.Effects);
                MainBoard[piece.Index] = p;
                MakeTrigger(p);
            }
            
            triggers.Sort((a, b) => b.CompareTo(a));
        }

        private void MakeTrigger(PieceData piece)
        {
            TriggerElement trigger;
            switch (piece.Type)
            {
                case PieceType.Velkaris:
                    trigger = new TriggerElement
                    {
                        Trigger = new VelkarisMarker(this, piece),
                        Priority = 0
                    };
                    break;
                case PieceType.GuidingSiren:
                    trigger = new TriggerElement()
                    {
                        Trigger = new SirenDebuffer(this, piece),
                        Priority = 2
                    };
                    break;
                default:
                    return;
            }
            triggers.Add(trigger);
            piece.Triggers.Add(trigger);
        }

        public void RemoveTrigger(PieceData piece)
        {
            foreach (var toRemove in piece.Triggers)
            {
                triggers.Remove(toRemove);
            }
        }

        public void EndTurn()
        {
            foreach (var piece in MainBoard)
            {
                if (piece == null) continue;
                
                for (var i = 0; i < piece.Effects.Count; i++)
                {
                    var eff = piece.Effects[i];
                    if (eff.Duration < 0) continue;
                    eff.Duration -= 1;
                    piece.Effects[i] = eff;
                }

                piece.Effects.RemoveAll(e => e.Duration == 0);

                if (piece.SkillCooldown > 0) piece.SkillCooldown -= 1;
            }
            
            var triggerToRemove = triggers.Where(trigger => trigger.Trigger.CallTrigger()).ToList();

            foreach (var trigger in triggerToRemove)
            {
                trigger.Trigger.Piece.Triggers.Remove(trigger);
                triggers.Remove(trigger);
            }
        }
    }
}
