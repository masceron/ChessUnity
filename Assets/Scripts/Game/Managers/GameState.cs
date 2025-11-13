using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Relics;
using Game.Relics.EyeOfMimic;
using Game.Relics.FrostSigil;
using Game.Relics.Pearl;
using Game.Relics.RottingScythe;
using Game.Relics.StormCapacitor;
using Game.Relics.SeafoamPhial;
using Game.Relics.SirensHarpoon;
using Game.Relics.MangroveCharm;
using UnityEngine;
using static Game.Common.BoardUtils;
using Game.Effects.RegionalEffect;

namespace Game.Managers
{
    public interface ISubscriber
    {
        // ObserverActivateWhen GetObserverActivate();
        // ObserverPriority GetPriority();
        public void OnCall(Action.Action action);
        public void OnCallEnd(bool color);
    }

    public enum ObserverActivateWhen : byte
    {
        None,
        Captures,
        Moves,
        SwitchTurn,
        MoveGeneration,
        EffectApplied
    }

    public enum Color : byte
    {
        White,
        Black,
        None
    }


    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameState
    {
        public readonly bool OurSide;
        public readonly PieceLogic[] PieceBoard;
        public readonly BitArray ActiveBoard;
        public readonly BitArray SquareColor;
        public bool SideToMove;
        public RelicLogic WhiteRelic;
        public RelicLogic BlackRelic;
        public int WhiteSkillUses;
        public int BlackSkillUses;
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();
        private readonly List<Effect> effectObservers = new();
        public RegionalEffect RegionalEffect;
        public bool IsDay { get; private set; }
        public int CurrentTurn { get; private set; }
        private int countTurn;
        private const int NumberOfTurnToChange = 10;

        public readonly List<ISubscriber> Subscribers = new();

        public System.Action<int> OnIncreaseTurn;

        public GameState(int maxLength, Vector2Int startingSize, bool side, bool ourSide)
        {
            OurSide = ourSide;

            PieceBoard = new PieceLogic[maxLength * maxLength];
            ActiveBoard = new BitArray(maxLength * maxLength);
            SquareColor = new BitArray(maxLength * maxLength);

            SideToMove = side;

            for (var i = 0; i < SquareColor.Count; i++)
            {
                SquareColor[i] = (RankOf(i) + FileOf(i)) % 2 != 0;
            }

            var rankStart = (maxLength - startingSize.x) / 2;
            var fileStart = (maxLength - startingSize.y) / 2;

            for (var offRank = 0; offRank < startingSize.x; offRank++)
            {
                var rank = rankStart + offRank;
                for (var offFile = 0; offFile < startingSize.y; offFile++)
                {
                    var file = fileStart + offFile;
                    ActiveBoard[IndexOf(rank, file)] = true;
                }
            }

            IsDay = true;
            CurrentTurn = 1;
            countTurn = 0;
        }

        public void SpawnPiece(PieceConfig piece)
        {
            PieceBoard[piece.Index] = PieceMaker.Get(piece);
        }

        public static RelicLogic GetRelicLogicByConfig(RelicConfig cfg)
        {
            RelicLogic rl = cfg.Type switch
            {
                RelicType.RottingScythe => new RottingScythe(cfg),
                RelicType.EyeOfMimic => new EyeOfMimic(cfg),
                RelicType.FrostSigil => new FrostSigil(cfg),
                RelicType.CommonPearl => new CommonPearl(cfg),
                RelicType.BlackPearl => new BlackPearl(cfg),
                RelicType.SeafoamPhial => new SeafoamPhial(cfg),
                RelicType.StormCapacitor => new StormCapacitor(cfg),
                RelicType.SirensHarpoon => new SirensHarpoon(cfg),
                RelicType.MangroveCharm => new MangroveCharm(cfg),
                _ => null
            };
            return rl;
        }

        public void MakeRegionalEffect(RegionalEffectType ret)
        {
            RegionalEffect = GetRegionalEffectByType(ret);
        }

        public RegionalEffect GetRegionalEffectByType(RegionalEffectType ret)
        {
            RegionalEffect re = ret switch
            {
                RegionalEffectType.Whirpool => new Whirlpool(),
                RegionalEffectType.PsionicShock => new PsionicShock(),
                RegionalEffectType.BloodMoon => new BloodMoon(),
                RegionalEffectType.RedTide => new RedTide(),
                _ => null
            };

            return re;
        }

        public static Effect CreateEffect(EffectName effectName, sbyte duration,sbyte strength, PieceLogic piece)
        {

            return effectName switch
            {
                // Buffs 
                EffectName.Carapace => new Effects.Buffs.Carapace(duration, piece),
                EffectName.HardenedShield => new Effects.Buffs.HardenedShield(piece),
                EffectName.Piercing => new Effects.Buffs.Piercing(duration, piece),
                EffectName.Shield => new Effects.Buffs.Shield(piece),
                EffectName.Camouflage => new Effects.Buffs.Camouflage(piece, strength),
                EffectName.Haste => new Effects.Buffs.Haste(duration, strength, piece),
                
                // Traits 
                EffectName.Evasion => new Effects.Traits.Evasion(duration, strength, piece),
                EffectName.Construct => new Effects.Traits.Construct(piece),
                EffectName.Demolisher => new Effects.Traits.Demolisher(piece),
                EffectName.Consume => new Effects.Traits.Consume(piece),
                EffectName.Surpass => new Effects.Traits.Surpass(piece),
                EffectName.Ambush => new Effects.Traits.Ambush(piece),
                EffectName.QuickReflex => new Effects.Traits.QuickReflex(piece),

                // Debuffs
                EffectName.Slow => new Effects.Debuffs.Slow(strength,duration, piece),
                EffectName.Blinded => new Effects.Debuffs.Blinded(duration, strength, piece),
                EffectName.Stunned => new Effects.Debuffs.Stunned(duration, piece),
                EffectName.Poison => new Effects.Debuffs.Poison(duration, piece),
                EffectName.Bleeding => new Effects.Debuffs.Bleeding(duration, piece),
                EffectName.Bound => new Effects.Debuffs.Bound(duration, piece),
                EffectName.Taunted => new Effects.Debuffs.Taunted(duration, piece),

                _ => new Effects.Buffs.Shield(piece)
            };
        }

        public void EffectCountdown()
        {
            foreach (var piece in PieceBoard)
            {
                if (piece == null || piece.Color != SideToMove) continue;

                piece.PassTurn();

                foreach (var effect in piece.Effects.Where(effect => effect.Duration >= 0))
                {
                    effect.Duration -= 1;

                    if (effect.Duration == 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(effect));
                    }
                }
            }

            WhiteRelic?.PassTurn();
            BlackRelic?.PassTurn();
        }

        public void OnStart()
        {
            OnIncreaseTurn?.Invoke(CurrentTurn);
        }

        public void Destroy(int pos)
        {
            var pieceAffected = PieceBoard[pos];
            PieceBoard[pos] = null;

            pieceAffected.Effects.ForEach(RemoveObserver);
            pieceAffected.Die();
        }

        public void Kill(int pos)
        {
            var pieceAffected = PieceBoard[pos];
            PieceBoard[pos] = null;

            pieceAffected.Effects.ForEach(RemoveObserver);
            pieceAffected.Die();

            (!pieceAffected.Color ? WhiteCaptured : BlackCaptured).Add(new PieceConfig(pieceAffected.Type,
                pieceAffected.Color, pieceAffected.Pos));
        }

        public void Move(int f, int t)
        {
            PieceBoard[t] = PieceBoard[f];
            PieceBoard[t].Pos = (ushort)t;
            PieceBoard[t].PreviousMoves.Add(f);
            PieceBoard[f] = null;
            FormationManager.Ins.TriggerExit(f, t);
            FormationManager.Ins.TriggerEnter(t);
        }

        public void Swap(int a, int b)
        {
            var pieceB = PieceBoard[b];
            PieceBoard[b] = PieceBoard[a];
            PieceBoard[b].Pos = (ushort)b;
            FormationManager.Ins.TriggerEnter(b);
            FormationManager.Ins.TriggerExit(a, b);
            PieceBoard[a] = pieceB;
            PieceBoard[a].Pos = (ushort)a;
            FormationManager.Ins.TriggerEnter(a);
            FormationManager.Ins.TriggerExit(b, a);
        }

        public void FlipSideToMove()
        {
            if (SideToMove)
            {
                countTurn++;
                CurrentTurn++;
                OnIncreaseTurn?.Invoke(CurrentTurn);

                if (countTurn >= NumberOfTurnToChange)
                {
                    IsDay = !IsDay;
                    countTurn = 0;
                }
            }

            SideToMove = !SideToMove;
        }

        public void AddObserver(Effect effect)
        {
            var pos = effectObservers.BinarySearch(effect, effect);
            effectObservers.Insert(pos >= 0 ? pos : ~pos, effect);
        }

        public void RemoveObserver(Effect effect)
        {
            effectObservers.Remove(effect);
        }

        public void NotifyEnd(Action.Action mainAction)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnCallEnd(SideToMove);
            }

            effectObservers.ForEach(effect =>
            {
                if (effect.ObserverActivateWhen != ObserverActivateWhen.SwitchTurn) return;
                if (effect is not IEndTurnEffect turnEffect) return;

                if (turnEffect.EndTurnEffectType == EndTurnEffectType.EndOfAnyTurn)
                {
                    turnEffect.OnCallEnd(mainAction);
                }

                //The next turn is ours.
                else if (SideToMove == effect.Piece.Color)
                {
                    if (turnEffect.EndTurnEffectType == EndTurnEffectType.EndOfEnemyTurn)
                    {
                        turnEffect.OnCallEnd(mainAction);
                    }
                }
                //The next turn is of the opponent.
                else
                {
                    if (turnEffect.EndTurnEffectType == EndTurnEffectType.EndOfAllyTurn)
                    {
                        turnEffect.OnCallEnd(mainAction);
                    }
                }
            });
        }

        public void NotifyMainAction(Action.Action mainAction)
        {
            if (mainAction is ICaptures)
            {
                effectObservers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Captures) effect.OnCallPieceAction(mainAction);
                });
            }

            if (mainAction.DoesMoveChangePos)
            {
                effectObservers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Moves &&
                        effect.Priority != ObserverPriority.AfterAction)
                        effect.OnCallPieceAction(mainAction);
                });
            }
        }

        public void NotifyOnMoveGen(List<Action.Action> actions)
        {
            effectObservers.ForEach(e =>
            {
                if (e.ObserverActivateWhen == ObserverActivateWhen.MoveGeneration)
                {
                    e.OnCallMoveGen(actions);
                }
            });
        }

        public void NotifyWhenApplyEffect(ApplyEffect action)
        {
            effectObservers.ForEach(e =>
            {
                if (e.ObserverActivateWhen == ObserverActivateWhen.EffectApplied)
                {
                    ((IApplyEffect)e).OnCallApplyEffect(action);
                }
            });
        }
        
        public void IncrementSkillUses(Action.Action action)
        {
            if (ColorOfPiece(action.Maker)) BlackSkillUses++;
            else WhiteSkillUses++;
        }
    }
    
    
}