using UnityEngine;

public static class Bootstrapper {
    /// <summary>
    /// Player needs to have a prefab named "Systems" in the "Resources" folder. This will instantiate the Systems prefab
    /// into the scene.
    ///
    /// Systems will be those that have "Don't Destroy On Load" attribute on them, so they persist throughout all scenes.
    ///
    /// The purpose of this is to have the Systems load regardless of whatever scene the player starts on.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/Systems")));
    }
}

