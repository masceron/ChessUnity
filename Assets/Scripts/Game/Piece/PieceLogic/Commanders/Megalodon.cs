using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Relics;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Megalodon: PieceLogic, IRelicCarriable, IPieceWithSkill
    {
        public Megalodon(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, RookMoves.Quiets,
            MegalodonMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrenziedVeteran(this)));
            
            Skills = list =>
            {
                var (rank, file) = RankFileOf(this.Pos);
                var caller = this;
                var range = caller.AttackRange;
                var color = caller.Color;
                var push = !color ? -1 : 1;
                if (SkillCooldown != 0) return;
                
                for (var nRank = rank - (range - 1) * push; nRank != rank + (range + 1) * push; nRank += push)
                {
                    if (!VerifyBounds(nRank)) continue;
                    for (var nFile = file - range * push; nFile != file + range * push; nFile += push)
                    {
                        if (!VerifyBounds(nFile)) continue;
                        var idx = IndexOf(nRank, nFile);
                        if (!IsActive(idx)) continue;

                        var piece = PieceOn(idx);

                        if (piece == null || piece.Color != color) continue;

                        list.Add(new MegalodonActive(Pos));
                    }
                }
            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public RelicLogic CarriedRelic { get; set; }



    }
    
}