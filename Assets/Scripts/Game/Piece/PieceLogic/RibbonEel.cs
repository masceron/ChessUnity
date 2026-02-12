using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Effects.Buffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class RibbonEel : Commons.PieceLogic, IPieceWithSkill
    {
        public RibbonEel(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    for (var i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        var p = BoardUtils.PieceOn(i);
                        if (p == null || p.Color == Color) continue;
                        list.Add(new DiurnalPending(Pos, p.Pos));
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
}