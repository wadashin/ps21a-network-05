using UnityEngine.SceneManagement;

/// <summary>
/// Scene‚ğØ‚è‘Ö‚¦‚é
/// </summary>
public class SceneChange
{
    private static bool roadNow = false;

    /// <summary>
    /// w’è‚µ‚½–¼Ì‚ÌScene‚É‘JˆÚ‚·‚é
    /// </summary>
    /// <param name="sceneName">Scene–¼</param>
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
