using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Internal.Pending;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using UnityEngine;
using static Game.Common.BoardUtils;
using static Game.Board.General.MatchManager;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    public class Chrysos: PieceLogic
    {
        public byte Coin;
        public byte SkillCooldown;
        public Chrysos(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlayersCoin(this)));
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown > 0) return;
            for (var i = 0; i < BoardSize; i++)
            {
                var piece = gameState.MainBoard[i];
                if (piece == null || piece.color != color) continue;
                
                var upgradableTo = UpgradableTo(piece.pieceRank);
                if (upgradableTo == PieceRank.None) continue;
                
                var cost = CalculateCost(piece.pieceRank, upgradableTo);
                if (Coin >= cost)
                {
                    list.Add(new ChrysosUpgradeCandidate(pos, i, cost));
                }
            }
        }
        
        private bool MakeMove(List<Action.Action> list, int index)
        {
            if (!gameState.ActiveBoard[index]) return false;
            var pieceOn = gameState.MainBoard[index];
            if (pieceOn != null)
            {
                if (pieceOn.color != color) list.Add(new NormalCapture(pos, pos, index));
                return false;
            }
            list.Add(new NormalMove(pos, pos, index));
            return true;
        }

        private void Moves(List<Action.Action> list)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            
            for (var rankOff = rank - 1; rankOff >= 0; rankOff--)
            {
                var index = IndexOf(rankOff, file);
                if (!MakeMove(list, index)) break;
            }
            
            for (var rankOff = rank + 1; rankOff < MaxLength; rankOff++)
            {
                var index = IndexOf(rankOff, file);
                if (!MakeMove(list, index)) break;
            }
            
            for (var fileOff = file - 1; fileOff >= 0; fileOff--)
            {
                var index = IndexOf(rank, fileOff);
                if (!MakeMove(list, index)) break;
            }
            
            for (var fileOff = file + 1; fileOff < MaxLength; fileOff++)
            {
                var index = IndexOf(rank, fileOff);
                if (!MakeMove(list, index)) break;
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            Moves(list);
            Skill(list);

            return list;
        }

        public static PieceRank UpgradableTo(PieceRank from)
        {
            return from switch
            {
                PieceRank.Swarm => PieceRank.Common,
                PieceRank.Common => PieceRank.Elite,
                PieceRank.Elite or PieceRank.Champion => PieceRank.Champion,
                _ => PieceRank.None
            };
        }

        private static int CalculateCost(PieceRank from, PieceRank to)
        {
            switch (to)
            {
                case PieceRank.Common:
                    return 1;
                case PieceRank.Elite:
                    return 3;
                case PieceRank.Champion:
                    return from switch
                    {
                        PieceRank.Elite => 5,
                        PieceRank.Champion => 6,
                        _ => -1
                    };
                case PieceRank.Commander:
                case PieceRank.Construct:
                case PieceRank.Summoned:
                case PieceRank.None:
                case PieceRank.Swarm:
                default:
                    break;
            }

            return -1;
        }
    }
}