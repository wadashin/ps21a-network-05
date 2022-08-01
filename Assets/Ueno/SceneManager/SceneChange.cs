using UnityEngine.SceneManagement;

/// <summary>
/// Scene��؂�ւ���
/// </summary>
public class SceneChange
{
    private static bool roadNow = false;

    /// <summary>
    /// �w�肵�����̂�Scene�ɑJ�ڂ���
    /// </summary>
    /// <param name="sceneName">Scene��</param>
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
