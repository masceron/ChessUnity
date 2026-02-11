using System;
using Game.Common;
using Game.Save.Army;
using Game.Save.FreePlay;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.FreePlayTest.RegionalRealmScene;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FPArmyDesign : Singleton<FPArmyDesign>
    {
        [SerializeField] private FreePlayArmyInfo info;
        public FreePlayArmyBoard board;
        public FreePlayNotification notification;
        [SerializeField] private FPArmySearcher searcher;
        [SerializeField] private FPRelicSearcher relicSearcher;
        [SerializeField] private Transform nextButton;
        [NonSerialized] public int size;
        public Game.Save.FreePlay.FPPreset army;
        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            if (relicSearcher.container.gameObject.activeSelf)
            {
                relicSearcher.Toggle();
                return;
            }
            else FreePlayNotification.Ins.Close();     
        }

        public void Load(int s, Game.Save.FreePlay.FPPreset? armyToLoad)
        {
            size = s;
            info.Load(size);
            board.Load(size);
            searcher.Load();
            if (armyToLoad != null)
            {
                LoadSave(armyToLoad.Value);
            }
            nextButton?.gameObject.SetActive(true);
            notification.Close();
        }
        public void Load(int s)
        {
            Load(s, null);
        }
        private void LoadSave(Game.Save.FreePlay.FPPreset armyToLoad)
        {
            army = armyToLoad;
            board.LoadSave(army.Troops, army.EnemyTroops);
            relicSearcher.Load(armyToLoad.Relic);
            relicSearcher.LoadEnemyRelic(armyToLoad.EnemyRelic);
        }

        public void Save()
        {
            army.BoardSize = (ushort) size;
            board.Troops.Sort();
            army.Troops = board.Troops.ToArray();
            board.EnemyTroops.Sort();
            army.EnemyTroops = board.EnemyTroops.ToArray();
            FreePlaySaveLoader.Save(army);
            UIManager.Ins.Load(CanvasID.RegionalEffect);
            RegionalManagerUI.Ins.Load();
        }

        public void SelectRelic(string type, bool side)
        {
            if (side)
            {
                army.EnemyRelic = new Relic(type);
            }
            else
            {
                army.Relic = new Relic(type);
            }
        }
    }
}