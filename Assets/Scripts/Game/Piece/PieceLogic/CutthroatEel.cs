using Game.Movesets;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Common;
using System.Linq;
using Game.Effects.Debuffs;
using Game.Effects;

namespace Game.Piece.PieceLogic
{
    public class CutthroatEel : Commons.PieceLogic, IPieceWithSkill
    {
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public CutthroatEel(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    foreach(var (rank, file) in MoveEnumerators.Around(RankOf(Pos), FileOf(Pos), 4))
                    {
                        int targetPos = IndexOf(rank, file);
                        Commons.PieceLogic pieceOn = PieceOn(targetPos);

                        if (pieceOn != null && pieceOn.Color != this.Color && pieceOn.Effects.Any(e => e.EffectName == "effect_bleeding"))
                        {
                            foreach(Effect effect in pieceOn.Effects)
                            {
                                if (effect is Bleeding bleeding)
                                {
                                    list.Add(new CutthroatEelActive(Pos, targetPos, bleeding));
                                    break;
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}