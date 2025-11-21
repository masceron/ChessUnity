using System;
using Game.Common;
using Game.Relics;
using Game.Save.Army;
using Game.Save.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesign: Singleton<ArmyDesign>
    {

        [SerializeField] private DesignArmyInfo info;
        public ArmyDesignBoard board;
        public ArmyDesignTroop troopDisplay;
        [SerializeField] private ArmySearcher searcher;
        [SerializeField] private DesignNotification notification;
        [SerializeField] private ArmyRelicSearcher relicSearcher;
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
            
            if (!notification.gameObject.activeSelf)
            {
                notification.Open(DesignNotifications.Quit);
            }
            else notification.Close();     
        }

        private void OnDisable()
        {
            notification.Close();
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
        }
        private void LoadSave(Game.Save.Army.Army armyToLoad)
        {
            army = armyToLoad;
            board.LoadSave(army.Troops);
            info.LoadSave(army.Name);
            relicSearcher.Load(armyToLoad.Relic);
        }

        public void TrySave()
        {
            army.Name = info.armyName.text;
            if (army.Name == string.Empty)
            {
                notification.Open(DesignNotifications.EmptyName);
            }
            if (ArmySaveLoader.Exists(army.Name) && UIManager.Ins.GetCanvasID() == CanvasID.DesignArmy)
            {
                notification.Open(DesignNotifications.Overwrite);
            }
            Save();
        }
        public bool TrySaveFreeTest()
        {
            army.Name = info.armyName.text;
            if (army.Name == string.Empty)
            {
                notification.Open(DesignNotifications.EmptyName);
                return false;
            }
            Save();
            return true;
        }
        public void Save()
        {
            army.BoardSize = (ushort) size;
            SetTroops();
            ArmySaveLoader.Save(army);
            UIManager.Ins.Load(CanvasID.Followers);
        }
        private void SetTroops()
        {
            board.Troops.Sort();
            army.Troops = board.Troops.ToArray();
        }

        public void SelectRelic(RelicType type)
        {
            army.Relic = new Relic(type);

        }
    }
}