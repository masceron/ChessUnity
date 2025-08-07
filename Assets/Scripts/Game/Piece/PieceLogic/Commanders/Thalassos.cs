using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Thalassos: PieceLogic, IPieceWithSkill
    {
        public Thalassos(PieceConfig cfg) : base(cfg, ThalassosMoves.Quiets, ThalassosMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ThalassosShielder(this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;

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
            };
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}