using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.SeabedLevelerUI;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLeveler : RelicLogic
    {
        private Charge charge;
        public SeabedLeveler(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddEffectObserver(charge);
        }

        public override void Activate()
        {
            if (charge.Strength >= 0)
            {

                for (int i = 0; i < BoardUtils.BoardSize; ++i)
                {
                    var piece = BoardUtils.PieceOn(i);
                    if (piece == null || piece.Color == Color) continue;
                    if (FormationManager.Ins.HasFormation(i) == false) continue;

                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new SeabedLevelerPending(this, charge, piece.Pos);
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