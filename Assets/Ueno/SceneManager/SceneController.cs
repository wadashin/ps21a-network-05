using UnityEngine;
/// <summary>
/// Scene�̊e����
/// </summary>
public class SceneController : MonoBehaviour
{
    void Start()
    {
        //Scene���؂�ւ�������Ƀt�F�[�h�C������
        FadeController.StartFadeIn();
    }
    /// <summary>Scene��J�ڂ����郁�\�b�h</summary>
    /// <param name="scene"></param>
    public void ChangeScene(string scene)
    {
        SceneChange.LoadScene(scene);
    }
    /// <summary>�Q�[�����I�������郁�\�b�h</summary>
    public void Finish()
    {
        Application.Quit();
    }
}