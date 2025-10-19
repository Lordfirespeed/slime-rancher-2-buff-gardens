using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BuffGardens;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal new static ManualLogSource Log = null!;
    internal static bool BuffedTheGardens = false;

    public override void Load()
    {
        Log = base.Log;
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (BuffedTheGardens) return;
        if (scene.path != "Assets/Scene/Core/GameCore.unity") return;
        var gameContext = UnityEngine.Object.FindObjectOfType<GameContext>();
        Log.LogInfo($"do we have a game context? {gameContext}");
        if  (gameContext == null) return;
        var resourceGrowers = gameContext.AutoSaveDirector?._configuration?.ResourceGrowers;
        if (resourceGrowers == null) return;
        foreach (var resourceGrower in resourceGrowers.items)
        {
            if (resourceGrower == null) continue;
            if (resourceGrower.ReferenceId != "treecuberry01") continue;
            resourceGrower._maxResources = 15;
            resourceGrower._maxSpawnIntervalGameHours /= 2;
            resourceGrower._minSpawnIntervalGameHours /= 2;
            break;
        }
    }
}
