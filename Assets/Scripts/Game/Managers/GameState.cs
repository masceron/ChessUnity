using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects;
using Game.Piece;
using UnityEngine;
using static Game.Common.BoardUtils;
using Game.Effects.RegionalEffect;
using UX.UI;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;

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
        public PieceLogic WhiteCommander, BlackCommander;
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
            var pieceLogic = PieceMaker.Get(piece);
            PieceBoard[piece.Index] = pieceLogic;
            if (pieceLogic.PieceRank == PieceRank.Commander)
            {
                if (pieceLogic.Color == false)
                {
                    WhiteCommander = pieceLogic;
                }
                else
                {
                    BlackCommander = pieceLogic;
                }
            }
            PieceBoard[piece.Index] = PieceMaker.Get(piece);

            var bc = PieceManager.Ins.GetPieceGameObject(piece.Index).gameObject.AddComponent<AI.BrainComponent>();
            bc.Maker = PieceBoard[piece.Index];    
        }

        public void MakeRegionalEffect(RegionalEffectType ret)
        {
            RegionalEffect = GetRegionalEffectByType(ret);
        }

        private static RegionalEffect GetRegionalEffectByType(RegionalEffectType ret)
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

            (!pieceAffected.Color ? WhiteCaptured : BlackCaptured).Add(new PieceConfig(pieceAffected.Type, pieceAffected.Color, pieceAffected.Pos));
            
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
                if (countTurn == 151)
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Draw);
                }
                if (countTurn >= NumberOfTurnToChange)
                {
                    IsDay = !IsDay;
                    countTurn = 0;
                }
            }
            if (SideToMove && WhiteCommander != null && WhiteCommander.IsDead())
            {
                if (WhiteCommander != null && WhiteCommander.IsDead())
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Lose);
                }
                else if (BlackCommander != null && BlackCommander.IsDead())
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                }
            }
            else if (!SideToMove && BlackCommander != null && BlackCommander.IsDead())
            {
                if (BlackCommander != null && BlackCommander.IsDead())
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                }
                else if (WhiteCommander != null && WhiteCommander.IsDead())
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Lose);
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