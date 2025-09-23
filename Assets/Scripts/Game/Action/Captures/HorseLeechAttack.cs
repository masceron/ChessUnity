using System.Text.RegularExpressions;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Others;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class HorseLeechAttack: Action, ICaptures
    {
        public HorseLeechAttack(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(PieceOn(Target))));
            ActionManager.EnqueueAction(new KillPiece(Maker));
        }

    }
}