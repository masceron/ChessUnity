using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Action;
using Game.Action.Internal;
using Game.Piece;
using UnityEngine;
using static Game.Common.BoardUtils;
using Game.Effects.RegionalEffect;
using UX.UI;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Managers
{
    
    public interface ISubscriber
    {
        public void OnCall(Action.Action action);
        public void OnCallEnd(bool color);
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
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();
        public readonly EffectHooks effectHooks = new();
        public RegionalEffect RegionalEffect;
        public readonly List<ISubscriber> Subscribers = new();
        public bool IsDay { get; private set; }
        public int CurrentTurn { get; private set; }
        private int countTurn;
        private const int NumberOfTurnToChange = 10;

        public System.Action<int> OnIncreaseTurn;

        private Action.Action lastMainAction;

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
                if (!pieceLogic.Color)
                {
                    WhiteCommander = pieceLogic;
                }
                else
                {
                    BlackCommander = pieceLogic;
                }
            }

            PieceBoard[piece.Index] = pieceLogic;

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
            effectHooks.NotifyDead(pieceAffected);

            pieceAffected.Effects.ForEach(RemoveEffectObserver);
        }

        public void Kill(int pos)
        {
            var pieceAffected = PieceBoard[pos];
            PieceBoard[pos] = null;
            effectHooks.NotifyDead(pieceAffected);

            pieceAffected.Effects.ForEach(RemoveEffectObserver);

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
            switch (SideToMove)
            {
                case true when WhiteCommander != null && !IsAlive(WhiteCommander):
                {
                    if (WhiteCommander != null && !IsAlive(WhiteCommander))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Lose);
                    }
                    else if (BlackCommander != null && !IsAlive(BlackCommander))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                    }

                    break;
                }
                case false when BlackCommander != null && !IsAlive(BlackCommander):
                {
                    if (BlackCommander != null && !IsAlive(BlackCommander))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                    }
                    else if (WhiteCommander != null && !IsAlive(WhiteCommander))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Lose);
                    }

                    break;
                }
            }
            SideToMove = !SideToMove;
        }
    }
    
    
}