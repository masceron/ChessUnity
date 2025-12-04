using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Thalassos : Commons.PieceLogic, IPieceWithSkill
    {
        public Thalassos(PieceConfig cfg) : base(cfg, ThalassosMoves.Quiets, ThalassosMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ThalassosShielder(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    for (var rankOff = -1; rankOff <= 1; rankOff++)
                    {
                        var rank = RankOf(Pos) + rankOff;
                        if (!VerifyBounds(rank)) continue;

                        for (var fileOff = -1; fileOff <= 1; fileOff++)
                        {
                            if (rankOff == 0 && fileOff == 0) continue;
                            var file = FileOf(Pos) + fileOff;
                            if (!VerifyBounds(file)) continue;
                            var posTo = IndexOf(rank, file);

                            if (PieceOn(posTo) == null)
                            {
                                list.Add(new ThalassosResurrectCandidate(Pos, posTo));
                            }
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}