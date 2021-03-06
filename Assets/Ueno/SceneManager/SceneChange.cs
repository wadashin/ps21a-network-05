using UnityEngine.SceneManagement;

/// <summary>
/// Sceneを切り替える
/// </summary>
public class SceneChange
{
    private static bool roadNow = false;

    /// <summary>
    /// 指定した名称のSceneに遷移する
    /// </summary>
    /// <param name="sceneName">Scene名</param>
    public static void LoadScene(string sceneName)
    {
        if (roadNow)
        {
            return;
        }

        roadNow = true;
        FadeController.StartFadeOut(() => Load(sceneName));
    }
    private static void Load(string sceneName)
    {
        roadNow = false;
        SceneManager.LoadScene(sceneName);
    }
}
