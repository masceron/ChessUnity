using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLeveler : RelicLogic
    {
        private readonly Charge charge;
        public SeabedLeveler(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddEffectObserver(charge);
        }

        public override void Activate()
        {
            if (charge.Strength >= 3)
            {

                for (var i = 0; i < BoardUtils.BoardSize; ++i)
                {
                    var piece = BoardUtils.PieceOn(i);
                    if (piece == null || piece.Color == Color) continue;
                    if (!BoardUtils.HasFormation(i)) continue;

                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new SeabedLevelerPending(charge, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }

            if (CurrentCooldown > 0)
            {
                charge.Strength = 0;
            }
        }
        

    }
}