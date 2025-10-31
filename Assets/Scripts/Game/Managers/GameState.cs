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
using Game.Piece.PieceLogic.Champions;
using Game.Piece.PieceLogic.Commanders;
using Game.Piece.PieceLogic.Commons;
using Game.Piece.PieceLogic.Construct;
using Game.Piece.PieceLogic.Construct.Bioluminescent_Beacon;
using Game.Piece.PieceLogic.Construct.Fracture_Zone;
using Game.Piece.PieceLogic.Construct.KelpForest;
using Game.Piece.PieceLogic.Elites;
using Game.Piece.PieceLogic.Summon;
using Game.Piece.PieceLogic.Swarm;
using Game.Relics;
using Game.Relics.EyeOfMimic;
using Game.Relics.FrostSigil;
using Game.Relics.Pearl;
using Game.Relics.RottingScythe;
using Game.Relics.StormCapacitor;
using Game.Relics.SeafoamPhial;
using Game.Relics.SirensHarpoon;
using UnityEngine;
using static Game.Common.BoardUtils;
using Game.Effects.RegionalEffect;

namespace Game.Managers
{
    public interface ISubscriber {
        // ObserverActivateWhen GetObserverActivate();
        // ObserverPriority GetPriority();
        public void OnCall(Action.Action action);
        public void OnCallEnd(bool color);
    }
    public enum ObserverActivateWhen: byte
    {
        None, Captures, Moves, SwitchTurn, MoveGeneration, EffectApplied
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
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();
        public RegionalEffect RegionalEffect;
        private readonly List<Effect> observers = new();
        public bool IsDay { get; private set; }
        private int CurrentTurn { get; set; }
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
            PieceLogic p = piece.Type switch
            {
                PieceType.Velkaris => new Velkaris(piece),
                PieceType.GuidingSiren => new GuidingSiren(piece),
                PieceType.Barracuda => new Barracuda(piece),
                PieceType.SeaUrchin => new SeaUrchin(piece),
                PieceType.ElectricEel => new ElectricEel(piece),
                PieceType.FlyingFish => new FlyingFish(piece),
                PieceType.Chrysos => new Chrysos(piece),
                PieceType.Anomalocaris => new Anomalocaris(piece),
                PieceType.Archelon => new Archelon(piece),
                PieceType.Thalassos => new Thalassos(piece),
                PieceType.Pufferfish => new Pufferfish(piece),
                PieceType.Swordfish => new Swordfish(piece),
                PieceType.Lionfish => new Lionfish(piece),
                PieceType.MorayEel => new MorayEel(piece),
                PieceType.Stingray => new Stingray(piece),
                PieceType.Seahorse => new Seahorse(piece),
                PieceType.SeaStar => new SeaStar(piece),
                PieceType.Anglerfish => new Anglerfish(piece),
                PieceType.Remora => new Remora(piece),
                PieceType.MedicinalLeach => new MedicinalLeech(piece),
                PieceType.KelpBass => new KelpBass(piece),
                PieceType.HourglassJelly => new HourglassJelly(piece),
                PieceType.Archerfish => new Archerfish(piece),
                PieceType.MoorishIdols => new MoorishIdols(piece),
                PieceType.Helicoprion => new Helicoprion(piece),
                PieceType.HermitCrab => new HermitCrab(piece),
                PieceType.SeaTurtle => new SeaTurtle(piece),
                PieceType.HorseLeech => new HorseLeech(piece),
                PieceType.Megalodon => new Megalodon(piece),
                PieceType.Temperantia => new Temperantia(piece),
                PieceType.BobtailSquid => new BobtailSquid(piece),
                PieceType.ClownFish => new ClownFish(piece),
                PieceType.LivingCoral => new LivingCoral(piece),
                PieceType.Humilitas => new Humilitas(piece),
                PieceType.StoneCrab => new StoneCrab(piece),
                PieceType.PhantomJelly => new PhantomJelly(piece),
                PieceType.ChamberedNautilus => new ChamberedNautilus(piece),
                PieceType.EpauletteShark => new EpauletteShark(piece),
                PieceType.FractureZone => new FractureZone(piece),
                PieceType.BioluminescentBeacon => new BioluminescentBeacon(piece),
                PieceType.Sunfish => new Sunfish(piece),
                PieceType.ContagionCorpse => new ContagionCorpse(piece),
                PieceType.TigerPrawn => new TigerPrawn(piece),
                PieceType.HammerOyster => new HammerOyster(piece),
                PieceType.BottlenoseDolphin => new BottlenoseDolphin(piece),
                PieceType.KelpForest => new KelpForest(piece),
                PieceType.Melibe => new Melibe(piece),
                PieceType.BlueDragon => new BlueDragon(piece),
                PieceType.Fangtooth => new Fangtooth(piece),
                PieceType.GulperEel => new GulperEel(piece),
                PieceType.Hatchetfish => new Hatchetfish(piece),
                PieceType.Lizardfish => new Lizardfish(piece),
                _ => null
            };

            PieceBoard[piece.Index] = p;    
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
                _ => null
            };

            return re;
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
            var pos = observers.BinarySearch(effect, effect);
            observers.Insert(pos >= 0 ? pos : ~pos, effect);
        }
        
        public void RemoveObserver(Effect effect)
        {
            observers.Remove(effect);
        }
        
        public void NotifyEnd(Action.Action mainAction)
        {
            foreach(var subscriber in Subscribers) {
                subscriber.OnCallEnd(SideToMove);
            }
            
            observers.ForEach(effect =>
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
        
        public void Notify(Action.Action mainAction)
        {
            if (mainAction is ICaptures)
            {
                observers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Captures) effect.OnCallPieceAction(mainAction);
                });
            }

            if (mainAction.DoesMoveChangePos)
            {
                observers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Moves)
                        effect.OnCallPieceAction(mainAction);
                });
            }
        }
        
        public void NotifyOnMoveGen(List<Action.Action> actions)
        {
            observers.ForEach(e =>
            {
                if (e.ObserverActivateWhen == ObserverActivateWhen.MoveGeneration)
                {
                    e.OnCallMoveGen(actions);
                }
            });
        }

        public void NotifyWhenApplyEffect(ApplyEffect action)
        {
            observers.ForEach(e =>
            {
                if (e.ObserverActivateWhen == ObserverActivateWhen.EffectApplied)
                {
                    ((IApplyEffect)e).OnCallApplyEffect(action);
                }
            });
        }
    }
}
