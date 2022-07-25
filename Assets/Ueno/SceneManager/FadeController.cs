using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �V�[���J�ڎ���Fade����
/// </summary>
public class FadeController : MonoBehaviour
{
    private static FadeController instance = default;

    [Tooltip("�t�F�[�h�X�s�[�h"),SerializeField] private float _fadeSpeed = 1f;
    [Tooltip("�t�F�[�h����摜"),SerializeField] private Image _fadeImage = default;
    [Tooltip("�J�n���̐F"),SerializeField] private Color _startColor = Color.black;
    
    private void Awake()
    {
        instance = this;
        _fadeImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// �t�F�[�h�C����ɃA�N�V��������
    /// </summary>
    /// <param name="action"></param>
    public static void StartFadeIn(Action action = null)
    {
        if (instance == null)
        {
            action?.Invoke();
            return;
        }
        instance.StartCoroutine(instance.FadeIn(action));
    }
    /// <summary>
    /// �t�F�[�h�A�E�g��ɃA�N�V��������
    /// </summary>
    /// <param name="action"></param>
    public static void StartFadeOut(Action action = null)
    {
        if (instance == null)
        {
            action?.Invoke();
            return;
        }
        instance.StartCoroutine(instance.FadeOut(action));
    }
    public static void StartFadeOutIn(Action outAction = null, Action inAction = null)
    {
        if (instance == null)
        {
            outAction?.Invoke();
            inAction?.Invoke();
            return;
        }
        instance.StartCoroutine(instance.FadeOutIn(outAction, inAction));
    }
    IEnumerator FadeOutIn(Action outAction, Action inAction)
    {
        yield return FadeOut(outAction);
        yield return FadeIn(inAction);
    }
    IEnumerator FadeIn(Action action)
    {
        _fadeImage.gameObject.SetActive(true);
        yield return FadeIn();
        action?.Invoke();
        _fadeImage.gameObject.SetActive(false);
    }
    IEnumerator FadeOut(Action action)
    {
        _fadeImage.gameObject.SetActive(true);
        yield return FadeOut();
        action?.Invoke();
    }
    IEnumerator FadeIn()
    {
        float clearScale = 1f;
        Color currentColor = _startColor;
        while (clearScale > 0f)
        {
            clearScale -= _fadeSpeed * Time.deltaTime;
            if (clearScale <= 0f)
            {
                clearScale = 0f;
            }
            currentColor.a = clearScale;
            _fadeImage.color = currentColor;
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        float clearScale = 0f;
        Color currentColor = _startColor;
        while (clearScale < 1f)
        {
            clearScale += _fadeSpeed * Time.deltaTime;
            if (clearScale >= 1f)
            {
                clearScale = 1f;
            }
            currentColor.a = clearScale;
            _fadeImage.color = currentColor;
            yield return null;
        }
    }
}
