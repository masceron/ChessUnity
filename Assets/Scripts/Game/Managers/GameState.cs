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
using Game.Piece.PieceLogic.Elites;
using Game.Piece.PieceLogic.Summon;
using Game.Piece.PieceLogic.Swarm;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    
    public enum ObserverActivateWhen: byte
    {
        None, Captures, Moves, EndTurn, MoveGeneration, EffectApplied
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
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();
        private readonly List<Effect> observers = new();
        
        //The main action taken this turn.
        public Action.Action MainAction;

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
                PieceType.Humilitas => new Humilitas(piece),
                PieceType.StoneGrab => new StoneGrab(piece),
                _ => null
            };

            PieceBoard[piece.Index] = p;    
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

        public void Move(ushort f, ushort t)
        {
            PieceBoard[t] = PieceBoard[f];
            PieceBoard[t].Pos = t;
            PieceBoard[t].PreviousMoves.Add(f);
            PieceBoard[f] = null;
        }
        
        public void Swap(ushort a, ushort b)
        {
            var pieceB = PieceBoard[b];
            PieceBoard[b] = PieceBoard[a];
            PieceBoard[b].Pos = b;
            PieceBoard[a] = pieceB;
            PieceBoard[a].Pos = a;
        }

        public void FlipSideToMove()
        {
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

        public void NotifyEnd()
        {
            observers.ForEach(effect =>
            {
                if (effect.ObserverActivateWhen != ObserverActivateWhen.EndTurn) return;
                //The next turn is of the opponent.
                if (SideToMove != effect.Piece.Color)
                {
                    if (((IEndTurnEffect)effect).EndTurnEffectType == EndTurnEffectType.EndOfAllyTurn)
                    {
                        ((IEndTurnEffect)effect).OnCallEnd(MainAction);
                    }
                }
                //The next turn is ours.
                else
                {
                    if (((IEndTurnEffect)effect).EndTurnEffectType == EndTurnEffectType.EndOfEnemyTurn)
                    {
                        ((IEndTurnEffect)effect).OnCallEnd(MainAction);
                    }
                }
            });
            
            MainAction = null;
        }
        
        public void Notify()
        {
            MainAction ??= MainAction;
            
            if (MainAction is ICaptures)
            {
                observers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Captures) effect.OnCall(MainAction);
                });
            }

            if (MainAction.DoesMoveChangePos)
            {
                observers.ForEach(effect =>
                {
                    if (effect.ObserverActivateWhen == ObserverActivateWhen.Moves)
                        effect.OnCall(MainAction);
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
