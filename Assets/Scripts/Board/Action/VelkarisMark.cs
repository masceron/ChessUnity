using Core;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisMark: Action
    {
        public VelkarisMark(int f, int t)
        {
            From = f;
            To = t;

            Move = new Move
            {
                from = (byte)f,
                to = (byte)t,
                flag = MoveFlag.VelkarisMark
            };
        }
        public override void ApplyAction()
        {
            
        }
    }
}