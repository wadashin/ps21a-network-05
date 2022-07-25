using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̍s��State
/// </summary>
public enum ActionStateType
{
    defense = 0,//���_�߂��ɂ��鎞
    fuel = 1,//�R���������ċ��_�߂��ɂ��鎞
    carry = 2,//�R���������ĔR����U�ꏊ�߂��ɂ��鎞
    why = 3,//�R���������Ă��Ȃ��̂ɔR����U�ꏊ�߂��ɂ��鎞
    other = 4,//���_������R����U�ꏊ���������Ă��鎞
}

/// <summary>
///�v���C���[�J���[ 
/// </summary>
public enum PlayerType
{
    red = 0,
    blue = 1,
    green = 2,
    yellow = 3,
}

public class State : MonoBehaviour
{
    [SerializeField]
    private Text _view = null;

    [SerializeField]
    private Image _image = null;

    [SerializeField]
    public ActionStateType _actionState = ActionStateType.defense;
    public ActionStateType ActionStateType
    {
        get => _actionState;
        set
        {
            _actionState = value;
            OnStateChanged();
        }
    }

    [SerializeField]
    public PlayerType _playerState = PlayerType.red;

    private void OnValidate()
    {
        OnStateChanged();

        _image = GetComponent<Image>();
        ChangePlayerColor();
    }
    /// <summary>
    /// Player�̃J���[��ς���
    /// </summary>
    private void ChangePlayerColor()
    {
        if (_image == null) { return; }

        switch (_playerState)
        {
            case PlayerType.red:
                _image.color = new Color(255, 0, 0, 50);
                break;
            case PlayerType.blue:
                _image.color = new Color(0, 255, 255, 50);
                break;
            case PlayerType.green:
                _image.color = new Color(0, 255, 0, 50);
                break;
            case PlayerType.yellow:
                _image.color = new Color(255, 255, 0, 50);
                break;
        }
    }
    /// <summary>
    /// Player��State�ɉ����ĕ\����ς���
    /// </summary>
    private void OnStateChanged()
    {
        if (_view == null) { return; }

        switch (ActionStateType)
        {
            case ActionStateType.defense:
                _view.text = "���_";
                break;
            case ActionStateType.fuel:
                _view.text = "���_�F�R���ێ�";
                break;
            case ActionStateType.carry:
                _view.text = "��U�ꏊ�F�R���^����";
                break;
            case ActionStateType.why:
                _view.text = "��U�ꏊ�F�R�����ێ�";
                break;
            case ActionStateType.other:
                _view.text = "���ꂽ�ꏊ�ɂ��܂�";
                break;
        }

    }

    
}
