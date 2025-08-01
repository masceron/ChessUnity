using Game.Board.General;
using UnityEngine;

namespace Game.Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        private static void MatchMaker()
        {
            MatchManager.Init(new Config());
        }
    
        private void Awake()
        {
            MatchMaker();
        }
    }
}
