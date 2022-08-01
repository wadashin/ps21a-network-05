using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの行動State
/// </summary>
public enum ActionStateType
{
    defense = 0,//拠点近くにいる時
    fuel = 1,//燃料を持って拠点近くにいる時
    carry = 2,//燃料を持って燃料補填場所近くにいる時
    why = 3,//燃料を持っていないのに燃料補填場所近くにいる時
    other = 4,//拠点からも燃料補填場所からも離れている時
}

/// <summary>
///プレイヤーカラー 
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
    /// Playerのカラーを変える
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
    /// PlayerのStateに応じて表示を変える
    /// </summary>
    private void OnStateChanged()
    {
        if (_view == null) { return; }

        switch (ActionStateType)
        {
            case ActionStateType.defense:
                _view.text = "拠点";
                break;
            case ActionStateType.fuel:
                _view.text = "拠点：燃料保持";
                break;
            case ActionStateType.carry:
                _view.text = "補填場所：燃料運搬中";
                break;
            case ActionStateType.why:
                _view.text = "補填場所：燃料未保持";
                break;
            case ActionStateType.other:
                _view.text = "離れた場所にいます";
                break;
        }

    }

    
}
