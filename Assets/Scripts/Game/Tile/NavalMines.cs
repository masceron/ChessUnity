using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic;

namespace Game.Tile
{
    public class NavalMines : Formation
    {
        public NavalMines(bool haveDuration, bool color) : base(color)
        {
            this.HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.NavalMines;
        }
        
        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            ActionManager.ExecuteImmediately(new KillPiece(PieceOnFormation.Pos));
            var (rank, file) = RankFileOf(PieceOnFormation.Pos);
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 8))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null) continue;

                bool canBeStunned = true;
                for (int i = 0; i < pOn.Effects.Count; i++)
                {
                    if (pOn.Effects[i].EffectName == EffectName.Shield
                        || pOn.Effects[i].EffectName == EffectName.HardenedShield
                        || pOn.Effects[i].EffectName == EffectName.Carapace)
                    {
                        canBeStunned = false;
                        break;
                    }
                }
                
                if (canBeStunned) ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pOn)));
            }
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }
    }
}