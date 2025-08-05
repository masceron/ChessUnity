using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Action.Quiets;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    public class Chrysos: PieceLogic, IPieceWithSkill
    {
        public byte Coin;

        public Chrysos(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlayersCoin(this)));
        }

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown > 0) return;
            var pieceBoard = PieceBoard();
            for (var i = 0; i < BoardSize; i++)
            {
                var piece = pieceBoard[i];
                if (piece == null || piece.Color != Color) continue;
                
                var upgradableTo = UpgradableTo(piece.PieceRank);
                if (upgradableTo == PieceRank.None) continue;
                
                var cost = CalculateCost(piece.PieceRank, upgradableTo);
                if (Coin >= cost)
                {
                    list.Add(new ChrysosUpgradeCandidate(Pos, i, cost));
                }
            }
        }
        
        private bool MakeMove(List<Action.Action> list, int index)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                if (pieceOn.Color != Color) list.Add(new NormalCapture(Pos, index));
                return false;
            }
            list.Add(new NormalMove(Pos, index));
            return true;
        }

        private void Moves(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            
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

        protected override void MoveToMake(List<Action.Action> list)
        {
            Moves(list);
            Skill(list);
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

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}