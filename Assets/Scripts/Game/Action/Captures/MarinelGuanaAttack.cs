// using Game.Action.Internal;

// namespace Game.Action.Captures
// {
//     [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
//     public class MarinelGuanaAttack: Action, ICaptures
//     {
//         public MarinelGuanaAttack(int maker, int to) : base(maker, true)
//         {
//             Maker = (ushort)maker;
//             Target = (ushort)to;
//         }
//         protected override void Animate()
//         {
            
//         }

//         protected override void ModifyGameState()
//         {
//             PieceManager.Ins.Destroy(Target);
//             PieceManager.Ins.Move(Maker, Target);
//             MatchManager.Ins.GameState.Kill(Target);
//             MatchManager.Ins.GameState.Move(Maker, Target);
//             Maker = Target;
//         }
//     }
// }