using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Action;
using Game.Action.Internal;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OliveRidleyTurtle : Commons.PieceLogic, IPieceWithSkill
    {
        public OliveRidleyTurtle(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Sanity(-1, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (!isPlayer) return;

                var activeBoard = ActiveBoard();

                bool IsEdgeTile(int idx)
                {
                    var r = RankOf(idx);
                    var f = FileOf(idx);

                
                    int[] dr = { -1, 1, 0, 0 };
                    int[] df = { 0, 0, -1, 1 };

                    for (int k = 0; k < 4; k++)
                    {
                        int nr = r + dr[k];
                        int nf = f + df[k];

                        
                        if (!VerifyBounds(nr) || !VerifyBounds(nf))
                            return true;

                        int nIdx = IndexOf(nr, nf);
                        if (!activeBoard[nIdx])
                            return true;
                    }

                    return false;
                }

                for (var index = 0; index < BoardSize; index++)
                {
                    if (!activeBoard[index]) continue;
                    if (PieceOn(index) != null) continue;
                    if (!IsEdgeTile(index)) continue;

                    list.Add(new OliveRidleyTurtleActive(this, index));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}

