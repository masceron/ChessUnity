using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public class NavalMines : Formation
    {
        public NavalMines(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.NavalMines;
        }
        
        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);

            if (!PieceOnFormation.Effects.Any(effect => effect.EffectName == "effect_shield" &&
                                                        effect.EffectName == "effect_hardened_shield"
                                                        && effect.EffectName == "effect_carapace"))
            {
                ActionManager.ExecuteImmediately(new KillPiece(PieceOnFormation.Pos));
            }

            var (rank, file) = RankFileOf(PieceOnFormation.Pos);
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 8))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pOn)));
            }
        }
    }
}