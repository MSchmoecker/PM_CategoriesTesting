using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using LocalizationManager;
using PieceManager;
using ServerSync;

namespace PM_CategoriesTesting
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class PM_CategoriesTestingPlugin : BaseUnityPlugin
    {
        internal const string ModName = "PM_CategoriesTesting";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource PM_CategoriesTestingLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            BuildPiece repairStation = new("repairstation", "RepairStation");
            repairStation.Name.English("Repair Station");
            repairStation.Description.English("Simple station to repair your tools. All at once. Just interact with this shit.");
            repairStation.RequiredItems.Add("Iron", 30, true);
            repairStation.RequiredItems.Add("Wood", 10, true);
            repairStation.RequiredItems.Add("SurtlingCore", 3, true);
            repairStation.Category.Set("Custom Category");
            repairStation.Tool.Add("Cultivator");
            repairStation.Crafting.Set(CraftingTable.Forge);

            BuildPiece OU_MetalGrate = new("odins_undercroft", "OU_MetalGrate");
            OU_MetalGrate.Category.Set("New_Building");
            OU_MetalGrate.Name.English("Odins MetalGrate");
            OU_MetalGrate.Description.English("A large metal grate");
            OU_MetalGrate.RequiredItems.Add("Iron", 1, true);

            BuildPiece OU_Urn = new("odins_undercroft", "OU_Urn");
            OU_Urn.Category.Set("New_Furniture");
            OU_Urn.Name.English("Odins Urn");
            OU_Urn.Description.English("A place to keep your remains");
            OU_Urn.RequiredItems.Add("Stone", 6, true);

            BuildPiece OU_Sarcophagus = new("odins_undercroft", "OU_Sarcophagus");
            OU_Sarcophagus.Category.Set("New_Furniture");
            OU_Sarcophagus.Name.English("Odins Sarcophagus");
            OU_Sarcophagus.Description.English("A large stone Sarcophagus");
            OU_Sarcophagus.RequiredItems.Add("Stone", 10, true);

            BuildPiece OU_Sarcophagus_Lid = new("odins_undercroft", "OU_Sarcophagus_Lid");
            OU_Sarcophagus_Lid.Category.Set("New_Furniture");
            OU_Sarcophagus_Lid.Name.English("Odins Sarcophagus Lid");
            OU_Sarcophagus_Lid.Description.English("A large stone Sarcophagus Lid");
            OU_Sarcophagus_Lid.RequiredItems.Add("Stone", 3, true);

            BuildPiece OU_Skeleton_Full = new("odins_undercroft", "OU_Skeleton_Full");
            OU_Skeleton_Full.Category.Set("New_Furniture");
            OU_Skeleton_Full.Name.English("Odins Skeleton Full");
            OU_Skeleton_Full.Description.English("A Skeleton Full");
            OU_Skeleton_Full.RequiredItems.Add("BoneFragments", 4, true);

            BuildPiece OU_Skeleton_Ribs = new("odins_undercroft", "OU_Skeleton_Ribs");
            OU_Skeleton_Ribs.Category.Set("New_Furniture");
            OU_Skeleton_Ribs.Name.English("Odins Skeleton Ribs");
            OU_Skeleton_Ribs.Description.English("A ribcage from a skeleton");
            OU_Skeleton_Ribs.RequiredItems.Add("BoneFragments", 2, true);

            BuildPiece OU_Skeleton_Hanging = new("odins_undercroft", "OU_Skeleton_Hanging");
            OU_Skeleton_Hanging.Category.Set("New_Furniture");
            OU_Skeleton_Hanging.Name.English("Odins Skeleton Hanging");
            OU_Skeleton_Hanging.Description.English("A Skeleton Hung");
            OU_Skeleton_Hanging.RequiredItems.Add("BoneFragments", 2, true);

            BuildPiece OU_Skeleton_Pile = new("odins_undercroft", "OU_Skeleton_Pile");
            OU_Skeleton_Pile.Category.Set("New_Furniture");
            OU_Skeleton_Pile.Name.English("Odins Skeleton Pile");
            OU_Skeleton_Pile.Description.English("A Skeleton Pile");
            OU_Skeleton_Pile.RequiredItems.Add("BoneFragments", 2, true);

            BuildPiece OU_StoneArchway = new("odins_undercroft", "OU_StoneArchway");
            OU_StoneArchway.Category.Set("New_Building");
            OU_StoneArchway.Name.English("Odins Stone Archway");
            OU_StoneArchway.Description.English("A stone Stone Archway");
            OU_StoneArchway.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneWall = new("odins_undercroft", "OU_StoneWall");
            OU_StoneWall.Category.Set("New_Building");
            OU_StoneWall.Name.English("Odins StoneWall");
            OU_StoneWall.Description.English("A stone wall");
            OU_StoneWall.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneHalfWall = new("odins_undercroft", "OU_StoneHalfWall");
            OU_StoneHalfWall.Category.Set("New_Building");
            OU_StoneHalfWall.Name.English("Odins StoneHalfWall");
            OU_StoneHalfWall.Description.English("A stone half wall");
            OU_StoneHalfWall.RequiredItems.Add("Stone", 1, true);

            BuildPiece OU_Stone_Floor_1x1 = new("odins_undercroft", "OU_Stone_Floor_1x1");
            OU_Stone_Floor_1x1.Category.Set("New_Building");
            OU_Stone_Floor_1x1.Name.English("Odins Stone Floor 1x1");
            OU_Stone_Floor_1x1.Description.English("A stone 1x1 floor piece");
            OU_Stone_Floor_1x1.RequiredItems.Add("Stone", 1, true);

            BuildPiece OU_Stone_Floor_2x1 = new("odins_undercroft", "OU_Stone_Floor_2x1");
            OU_Stone_Floor_2x1.Category.Set("New_Building");
            OU_Stone_Floor_2x1.Name.English("Odins Stone Floor 2x1");
            OU_Stone_Floor_2x1.Description.English("A stone 2x1 floor piece");
            OU_Stone_Floor_2x1.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_Stone_Floor_2x2 = new("odins_undercroft", "OU_Stone_Floor_2x2");
            OU_Stone_Floor_2x2.Category.Set("New_Building");
            OU_Stone_Floor_2x2.Name.English("Odins Stone Floor 2x2");
            OU_Stone_Floor_2x2.Description.English("A stone 2x2 floor piece");
            OU_Stone_Floor_2x2.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_Stone_Roof_45 = new("odins_undercroft", "OU_Stone_Roof_45");
            OU_Stone_Roof_45.Category.Set("New_Building");
            OU_Stone_Roof_45.Name.English("Odins Stone Roof 45");
            OU_Stone_Roof_45.Description.English("A stone 45 roof");
            OU_Stone_Roof_45.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_Stone_Outter_Corner = new("odins_undercroft", "OU_Stone_Outter_Corner");
            OU_Stone_Outter_Corner.Category.Set("New_Building");
            OU_Stone_Outter_Corner.Name.English("Odins Stone Outside Corner");
            OU_Stone_Outter_Corner.Description.English("A stone roof outter corner");
            OU_Stone_Outter_Corner.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_Stone_Roof_Corner = new("odins_undercroft", "OU_Stone_Roof_Corner");
            OU_Stone_Roof_Corner.Category.Set("New_Building");
            OU_Stone_Roof_Corner.Name.English("Odins Stone Roof Corner");
            OU_Stone_Roof_Corner.Description.English("A stone roof corner");
            OU_Stone_Roof_Corner.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_DrainPipe = new("odins_undercroft", "OU_DrainPipe");
            OU_DrainPipe.Category.Set("New_Furniture");
            OU_DrainPipe.Name.English("Odins Drinpipe");
            OU_DrainPipe.Description.English("A stone drain deco");
            OU_DrainPipe.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_CornerCap = new("odins_undercroft", "OU_CornerCap");
            OU_CornerCap.Category.Set("New_Building");
            OU_CornerCap.Name.English("Odins CornerCap");
            OU_CornerCap.Description.English("A stone corner cap for OU walls");
            OU_CornerCap.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_CornerCap_Small = new("odins_undercroft", "OU_CornerCap_Small");
            OU_CornerCap_Small.Category.Set("New_Building");
            OU_CornerCap_Small.Name.English("Odins CornerCap Small");
            OU_CornerCap_Small.Description.English("A small stone corner cap for OU walls");
            OU_CornerCap_Small.RequiredItems.Add("Stone", 1, true);

            BuildPiece OU_StoneBeam = new("odins_undercroft", "OU_StoneBeam");
            OU_StoneBeam.Category.Set("New_Building");
            OU_StoneBeam.Name.English("Odins Stone Beam");
            OU_StoneBeam.Description.English("A stone beam cap for OU walls");
            OU_StoneBeam.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneBeam_Small = new("odins_undercroft", "OU_StoneBeam_Small");
            OU_StoneBeam_Small.Category.Set("New_Building");
            OU_StoneBeam_Small.Name.English("Odins Stone Beam Small");
            OU_StoneBeam_Small.Description.English("A small stone beam cap for OU walls");
            OU_StoneBeam_Small.RequiredItems.Add("Stone", 1, true);

            BuildPiece OU_Iron_Cage = new("odins_undercroft", "OU_Iron_Cage");
            OU_Iron_Cage.Category.Set("New_Misc");
            OU_Iron_Cage.Name.English("Odins Iron Cage");
            OU_Iron_Cage.Description.English("An iron cage");
            OU_Iron_Cage.RequiredItems.Add("Iron", 4, true);

            BuildPiece OU_Metal_Cage = new("odins_undercroft", "OU_Metal_Cage");
            OU_Metal_Cage.Category.Set("New_Misc");
            OU_Metal_Cage.Name.English("Odins BlackMetal Cage");
            OU_Metal_Cage.Description.English("An blackmetal cage");
            OU_Metal_Cage.RequiredItems.Add("BlackMetal", 4, true);

            BuildPiece OU_Swords_Crossed = new("odins_undercroft", "OU_Swords_Crossed");
            OU_Swords_Crossed.Category.Set("New_Furniture");
            OU_Swords_Crossed.Name.English("Odins Crossed Swords");
            OU_Swords_Crossed.Description.English("A stone pare of swords");
            OU_Swords_Crossed.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_Wall_Shield = new("odins_undercroft", "OU_Wall_Shield");
            OU_Wall_Shield.Category.Set("New_Furniture");
            OU_Wall_Shield.Name.English("Odins Wall Shield");
            OU_Wall_Shield.Description.English("A shield deco for walls");
            OU_Wall_Shield.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneRoof_Tile = new("odins_undercroft", "OU_StoneRoof_Tile");
            OU_StoneRoof_Tile.Category.Set("New_Building");
            OU_StoneRoof_Tile.Name.English("Odins StoneRoof Tile");
            OU_StoneRoof_Tile.Description.English("A stone rooftile piece");
            OU_StoneRoof_Tile.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneFloor = new("odins_undercroft", "OU_StoneFloor");
            OU_StoneFloor.Category.Set("New_Building");
            OU_StoneFloor.Name.English("Odins Stone Floor");
            OU_StoneFloor.Description.English("A stone floor piece");
            OU_StoneFloor.RequiredItems.Add("Stone", 2, true);

            BuildPiece OU_StoneStair = new("odins_undercroft", "OU_StoneStair");
            OU_StoneStair.Category.Set("New_Building");
            OU_StoneStair.Name.English("Odins StoneStairs");
            OU_StoneStair.Description.English("A stone stair piece");
            OU_StoneStair.RequiredItems.Add("Stone", 6, true);

            BuildPiece OU_Large_Stone_Pillar = new("odins_undercroft", "OU_Large_Stone_Pillar");
            OU_Large_Stone_Pillar.Category.Set("New_Building");
            OU_Large_Stone_Pillar.Name.English("Odins Large Stone Pillar");
            OU_Large_Stone_Pillar.Description.English("A large stone pillar");
            OU_Large_Stone_Pillar.RequiredItems.Add("Stone", 10, true);

            BuildPiece OU_Medium_Stone_Pillar = new("odins_undercroft", "OU_Medium_Stone_Pillar");
            OU_Medium_Stone_Pillar.Category.Set("New_Building");
            OU_Medium_Stone_Pillar.Name.English("Odins Medium Stone Pillar");
            OU_Medium_Stone_Pillar.Description.English("A medium stone pillar");
            OU_Medium_Stone_Pillar.RequiredItems.Add("Stone", 6, true);

            BuildPiece OH_Undercroft_BuildSkull = new("odins_undercroft", "OH_Undercroft_BuildSkull");
            OH_Undercroft_BuildSkull.Category.Set("New_Crafting");
            OH_Undercroft_BuildSkull.Name.English("Odins Crafting Skull");
            OH_Undercroft_BuildSkull.Description.English("Sets Build Area for undercroft pieces.");
            OH_Undercroft_BuildSkull.RequiredItems.Add("BoneFragments", 1, true);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                PM_CategoriesTestingLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                PM_CategoriesTestingLogger.LogError($"There was an issue loading your {ConfigFileName}");
                PM_CategoriesTestingLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            [UsedImplicitly] public int? Order;
            [UsedImplicitly] public bool? Browsable;
            [UsedImplicitly] public string? Category;
            [UsedImplicitly] public Action<ConfigEntryBase>? CustomDrawer;
        }

        #endregion
    }
}