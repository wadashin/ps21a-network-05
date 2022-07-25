using UnityEngine;
/// <summary>
/// Sceneの各処理
/// </summary>
public class SceneController : MonoBehaviour
{
    void Start()
    {
        //Sceneが切り替わった時にフェードインする
        FadeController.StartFadeIn();
    }
    /// <summary>Sceneを遷移させるメソッド</summary>
    /// <param name="scene"></param>
    public void ChangeScene(string scene)
    {
        SceneChange.LoadScene(scene);
    }
    /// <summary>ゲームを終了させるメソッド</summary>
    public void Finish()
    {
        Application.Quit();
    }
}