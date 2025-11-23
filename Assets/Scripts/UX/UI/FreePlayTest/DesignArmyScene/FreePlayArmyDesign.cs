using System;
using Game.Common;
using Game.Relics;
using Game.Save.Army;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using UX.UI.FreePlayTest.RegionalRealmScene;
using UX.UI.Army.DesignArmy;
using UX.UI.Followers;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FreePlayArmyDesign : Singleton<FreePlayArmyDesign>
    {
        public bool choosenSide { get; private set; }
        [SerializeField] private FreePlayArmyInfo info;
        public FreePlayArmyBoard board;
        public FreePlayArmyTroop troopDisplay;
        public FreePlayNotification notification;
        [SerializeField] private FPArmySearcher searcher;
        [SerializeField] private ArmyRelicSearcher relicSearcher;
        [SerializeField] private Transform nextButton;
        [NonSerialized] public int size;
        public Game.Save.Army.Army army;
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

        public void Load(int s, Game.Save.Army.Army? armyToLoad)
        {
            size = s;
            info.Load(size);
            board.Load(size);
            searcher.Load();
            if (armyToLoad != null)
            {
                LoadSave(armyToLoad.Value);
            }
            else
            {
                relicSearcher.Load(null);
            }
            nextButton?.gameObject.SetActive(true);
            notification.Close();
        }
        public void Load(int s)
        {
            Load(s, null);
        }
        private void LoadSave(Game.Save.Army.Army armyToLoad)
        {
            army = armyToLoad;
            board.LoadSave(army.Troops);
            relicSearcher.Load(armyToLoad.Relic);
        }

        public void Save()
        {
            army.BoardSize = (ushort) size;
            SetTroops();
            ArmySaveLoader.Save(army);
            UIManager.Ins.Load(CanvasID.RegionalEffect);
            RegionalManagerUI.Ins.Load();
        }
        private void SetTroops()
        {
            board.Troops.Sort();
            army.Troops = board.Troops.ToArray();
        }

        public void SelectRelic(string type)
        {
            if (choosenSide == false)
            {
                army.Relic = new Relic(type);
            }
            else
            {
                army.EnemyRelic = new Relic(type);
            }
        }

        public void ToggleChosenSide()
        {
            choosenSide = !choosenSide;
        }
    }
}