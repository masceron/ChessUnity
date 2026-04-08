using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Game.Action;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Effects.FieldEffect;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using Game.Tile;
using Unity.Properties;
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
    [GeneratePropertyBag]
    [Friend(typeof(BoardUtils), typeof(MatchManager), typeof(ActionManager))]
    public class GameState: INotifyPropertyChanged
    {
        public readonly BitArray ActiveBoard = new(MaxLength * MaxLength);

        public readonly Tuple<List<PieceConfig>, List<PieceConfig>> Captured =
            new(new List<PieceConfig>(), new List<PieceConfig>());

        public readonly BitArray SquareColor = new(MaxLength * MaxLength);
        public readonly PieceLogic[] PieceBoard = new PieceLogic[MaxLength * MaxLength];
        public readonly Formation[] Formations = new Formation[MaxLength * MaxLength];
        public readonly bool OurSide;
        public (RelicLogic, RelicLogic) Relics;
        public FieldEffect FieldEffect;
        public bool SideToMove;
        public (PieceLogic, PieceLogic) Commanders;
        public bool IsDay { get; private set; }
        
        [CreateProperty]
        public int CurrentTurn
        {
            get => _currentTurn;
            private set
            {
                if (_currentTurn == value) return;
                _currentTurn = value;
                NotifyPropertyChanged();
            }
        }
        private int _currentTurn;
        
        public readonly TriggerHooks TriggerHooks = new();
        private int _pieceID = 1;
        private readonly Dictionary<int, Entity> _entityDict = new();
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public GameState(Vector2Int startingSize, bool side, bool ourSide,
            (RelicConfig, RelicConfig) relics, FieldEffectType fieldEffectType)
        {
            OurSide = ourSide;
            Relics = (RelicMaker.Get(relics.Item1), RelicMaker.Get(relics.Item2));
            SideToMove = side;
            FieldEffect = GetFieldEffectByType(fieldEffectType);

            for (var i = 0; i < SquareColor.Count; i++) SquareColor[i] = (RankOf(i) + FileOf(i)) % 2 != 0;

            var rankStart = (MaxLength - startingSize.x) / 2;
            var fileStart = (MaxLength - startingSize.y) / 2;

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
        }

        public PieceLogic SpawnPiece(PieceConfig piece)
        {
            var pieceLogic = PieceMaker.Get(piece);
            PieceBoard[piece.Index] = pieceLogic;
            if (pieceLogic.PieceRank == PieceRank.Commander)
            {
                if (!pieceLogic.Color)
                    Commanders.Item1 = pieceLogic;
                else
                    Commanders.Item2 = pieceLogic;
            }

            PieceBoard[piece.Index] = pieceLogic;
            TriggerHooks.NotifySpawnPiece(pieceLogic);

            var bc = PieceManager.Ins.GetPieceGameObject(piece.Index).gameObject.AddComponent<BrainComponent>();
            bc.Maker = PieceBoard[piece.Index];

            _entityDict.Add(pieceLogic.ID, pieceLogic);
            return pieceLogic;
        }

        public int NextEntityID()
        {
            return _pieceID++;
        }

        private static FieldEffect GetFieldEffectByType(FieldEffectType ret)
        {
            FieldEffect re = ret switch
            {
                FieldEffectType.Whirlpool => new Whirlpool(),
                FieldEffectType.PsionicShock => new PsionicShock(),
                FieldEffectType.BloodMoon => new BloodMoon(),
                FieldEffectType.RedTide => new RedTide(),
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

            Relics.Item1.PassTurn();
            Relics.Item2.PassTurn();
        }

        public void Kill(PieceLogic pieceAffected, bool record)
        {
            PieceBoard[pieceAffected.Pos] = null;
            TriggerHooks.NotifyDead(pieceAffected);

            pieceAffected.Effects.ForEach(RemoveObserver);

            if (!record) return;

            if (!pieceAffected.Color)
                Captured.Item1.Add(new PieceConfig(pieceAffected.Type, pieceAffected.Color, pieceAffected.Pos));
            else Captured.Item2.Add(new PieceConfig(pieceAffected.Type, pieceAffected.Color, pieceAffected.Pos));
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
                CurrentTurn++;
                if (CurrentTurn == 151)
                {
                    UIManager.Ins.Load(CanvasID.EndGameMessage);
                    EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Draw);
                }

                if (CurrentTurn % 10 == 0)
                {
                    IsDay = !IsDay;
                }
            }

            switch (SideToMove)
            {
                case true when Commanders.Item1 != null && !IsAlive(Commanders.Item1):
                {
                    if (Commanders.Item1 != null && !IsAlive(Commanders.Item1))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Lose);
                    }
                    else if (Commanders.Item2 != null && !IsAlive(Commanders.Item2))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                    }

                    break;
                }
                case false when Commanders.Item2 != null && !IsAlive(Commanders.Item2):
                {
                    if (Commanders.Item2 != null && !IsAlive(Commanders.Item2))
                    {
                        UIManager.Ins.Load(CanvasID.EndGameMessage);
                        EndGameUI.Ins.SetMessage(EndGameUI.MessageID.Win);
                    }
                    else if (Commanders.Item1 != null && !IsAlive(Commanders.Item1))
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
            return _entityDict.GetValueOrDefault(id);
        }

        public void SetFieldEffect(FieldEffect e)
        {
            FieldEffect = e;
            TriggerHooks.AddObserver(e);
        }

        public void AddToEntityList(Entity entity)
        {
            _entityDict.Add(entity.ID, entity);
        }

        public void PruneEntityList()
        {
            var keysToRemove = _entityDict.Where(kvp => !IsAlive(kvp.Value))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _entityDict.Remove(key);
            }
        }
    }
}