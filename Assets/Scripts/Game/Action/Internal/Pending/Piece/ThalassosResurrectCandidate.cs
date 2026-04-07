using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    //Làm lại
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // public class ThalassosResurrectCandidate : PendingAction, ISkills
    // {
    //     public ThalassosResurrectCandidate(PieceLogic maker, int pos) : base(maker, pos)
    //     {
    //     }
    //
    //     public int AIPenaltyValue(PieceLogic p)
    //     {
    //         return 0;
    //     }
    //
    //     protected override void CompleteAction()
    //     {
    //         var selector =
    //             BoardViewer.Ins.GetOrInstantiateUI<ThalassosResurrector>(IngameSubmenus.ThalassosResurrector);
    //         selector.Load(GetFrom(), GetTargetPos(), this);
    //     }
    // }
}