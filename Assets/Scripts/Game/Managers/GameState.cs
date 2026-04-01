using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Action;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using Game.Tile;
using UnityEngine;
using UX.UI;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Managers
{
    public enum Color : byte
    {
        White,
        Black,
        None
    }


    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Friend(typeof(BoardUtils))]
    public class GameState
    {
        private int _pieceID = 1;
        private const int NumberOfTurnToChange = 10;
        public readonly BitArray ActiveBoard;
        public readonly ObservableCollection<PieceConfig> BlackCaptured = new();
        public readonly Formation[] Formations;
        public readonly bool OurSide;
        public readonly PieceLogic[] PieceBoard;
        public readonly BitArray SquareColor;
        public readonly TriggerHooks TriggerHooks = new();
        public readonly ObservableCollection<PieceConfig> WhiteCaptured = new();
        private int _countTurn;

        private Action.Action _lastMainAction;
        public RelicLogic BlackRelic;

        public Action<int> OnIncreaseTurn;
        public RegionalEffect RegionalEffect;
        public bool SideToMove;
        public PieceLogic WhiteCommander, BlackCommander;
        public RelicLogic WhiteRelic;
        
        public readonly Dictionary<int, Entity> EntityDict = new();

        public GameState(int maxLength, Vector2Int startingSize, bool side, bool ourSide)
        {
            OurSide = ourSide;

            PieceBoard = new PieceLogic[maxLength * maxLength];
            ActiveBoard = new BitArray(maxLength * maxLength);
            SquareColor = new BitArray(maxLength * maxLength);
            Formations = new Formation[maxLength * maxLength];

            SideToMove = side;

            for (var i = 0; i < SquareColor.Count; i++) SquareColor[i] = (RankOf(i) + FileOf(i)) % 2 != 0;

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
            _countTurn = 0;
        }

        public bool IsDay { get; private set; }
        public int CurrentTurn { get; private set; }

        public PieceLogic SpawnPiece(PieceConfig piece)
        {
            var pieceLogic = PieceMaker.Get(piece);
            PieceBoard[piece.Index] = pieceLogic;
            if (pieceLogic.PieceRank == PieceRank.Commander)
            {
                if (!pieceLogic.Color)
                    WhiteCommander = pieceLogic;
                else
                    BlackCommander = pieceLogic;
            }

            PieceBoard[piece.Index] = pieceLogic;
            TriggerHooks.NotifySpawnPiece(pieceLogic);

            var bc = PieceManager.Ins.GetPieceGameObject(piece.Index).gameObject.AddComponent<BrainComponent>();
            bc.Maker = PieceBoard[piece.Index];
            
            EntityDict.Add(pieceLogic.ID, pieceLogic);
            return pieceLogic;
        }

        public int NextEntityID()
        {
            return _pieceID++;
        }

        public void MakeRegionalEffect(RegionalEffectType ret)
        {
            RegionalEffect = GetRegionalEffectByType(ret);
        }

        private static RegionalEffect GetRegionalEffectByType(RegionalEffectType ret)
        {
            RegionalEffect re = ret switch
            {
                RegionalEffectType.Whirlpool => new Whirlpool(),
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

                foreach (var effect in piece.Effects.Where(effect => effect.Duration > 0))
                {
                    effect.Duration -= 1;

                    if (effect.Duration == 0) ActionManager.EnqueueAction(new RemoveEffect(effect));
                }
            }

            foreach (var format in Formations)
            {
                if (format is not { HaveDuration: true } || SideToMove != format.GetColor()) continue;

                format.SetDuration(format.Duration - 1);
                if (format.Duration <= 0) FormationManager.Ins.RemoveFormation(format);
            }

            WhiteRelic?.PassTurn();
            BlackRelic?.PassTurn();
        }

        public void OnStart()
        {
            OnIncreaseTurn?.Invoke(CurrentTurn);
        }

        public void Destroy(PieceLogic pieceAffected)
        {
            PieceBoard[pieceAffected.Pos] = null;
            TriggerHooks.NotifyDead(pieceAffected);

            pieceAffected.Effects.ForEach(RemoveObserver);
        }

        public void Kill(PieceLogic pieceAffected)
        {
            PieceBoard[pieceAffected.Pos] = null;
            TriggerHooks.NotifyDead(pieceAffected);

            pieceAffected.Effects.ForEach(RemoveObserver);

            (!pieceAffected.Color ? WhiteCaptured : BlackCaptured).Add(new PieceConfig(pieceAffected.Type,
                pieceAffected.Color, pieceAffected.Pos));
        }

        public void Move(PieceLogic piece, int t)
        {
            PieceBoard[t] = piece;
            PieceBoard[piece.Pos] = null;
            piece.Pos = t;
        }

        public void Swap(PieceLogic a, PieceLogic b)
        {
            var oldPosA = a.Pos;
            a.Pos = b.Pos;
            PieceBoard[a.Pos] = a;
            b.Pos = oldPosA;
            PieceBoard[b.Pos] = b;
        }

        public void FlipSideToMove()
        {
            if (SideToMove)
            {
                _countTurn++;
                CurrentTurn++;
                OnIncreaseTurn?.Invoke(CurrentTurn);
                if (_countTurn == 151)
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Draw);
                }

                if (_countTurn >= NumberOfTurnToChange)
                {
                    IsDay = !IsDay;
                    _countTurn = 0;
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

        public Entity GetEntityByID(int id)
        {
            return EntityDict.GetValueOrDefault(id);
        }

        public void SetRegionalEffect(RegionalEffect e)
        {
            RegionalEffect = e;
            TriggerHooks.AddObserver(e);
        }
    }
}