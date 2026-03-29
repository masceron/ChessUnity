using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

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

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);

            if (!PieceOnFormation.Effects.Any(effect => effect.EffectName == "effect_shield" &&
                                                        effect.EffectName == "effect_hardened_shield"
                                                        && effect.EffectName == "effect_carapace"))
                ActionManager.EnqueueAction(new KillPiece(PieceOnFormation.Pos));

            var (rank, file) = RankFileOf(PieceOnFormation.Pos);
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 8))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pOn)));
            }
        }

        public override int GetValueForAI()
        {
            return -150;
        }
    }
}