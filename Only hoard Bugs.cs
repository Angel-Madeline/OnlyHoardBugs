using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;


namespace OnlyHoardBugs
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class OnlyHoardBugs : BaseUnityPlugin
    {
        public static bool loaded;

        private const string modGUID = "AngelMadeline.OnlyHoardBugs";

        private const string modName = "Only Hoard Bugs";

        private const string modVersion = "1.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static OnlyHoardBugs Instance;

        public static ManualLogSource mls;

        private void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modName);
            mls.LogInfo("Loaded Only Hoard Bugs and applying patches.");
            harmony.PatchAll(typeof(OnlyHoardBugs));
        }

        [HarmonyPatch(typeof(RoundManager), "LoadNewLevel")]
        [HarmonyPrefix]
        private static bool OnlyHoardBugsSpawn(ref SelectableLevel newLevel)
        {
            foreach (SpawnableEnemyWithRarity Enemy in newLevel.Enemies)
            {
                Enemy.rarity = 0;
                if ((Object)(object)Enemy.enemyType.enemyPrefab.GetComponent<HoarderBugAI>() != (Object)null)
                {
                    Enemy.rarity = 999;
                }
            }
            mls.LogDebug($"{modName}: Removed Other Enemies");
            return true;
        }
    }
}