using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ƒvƒŒƒCƒ„[‚Ìs“®State
/// </summary>
public enum ActionStateType
{
    defense = 0,//‹’“_‹ß‚­‚É‚¢‚é
    fuel = 1,//”R—¿‚ğ‚Á‚Ä‹’“_‹ß‚­‚É‚¢‚é
    carry = 2,//”R—¿‚ğ‚Á‚Ä”R—¿•â“UêŠ‹ß‚­‚É‚¢‚é
    why = 3,//”R—¿‚ğ‚Á‚Ä‚¢‚È‚¢‚Ì‚É”R—¿•â“UêŠ‹ß‚­‚É‚¢‚é
    other = 4,//‹’“_‚©‚ç‚à”R—¿•â“UêŠ‚©‚ç‚à—£‚ê‚Ä‚¢‚é
}

/// <summary>
///ƒvƒŒƒCƒ„[ƒJƒ‰[ 
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
            OnCellStateChanged();
        }
    }

    [SerializeField]
    public PlayerType _playerState = PlayerType.red;

    private void OnValidate()
    {
        OnCellStateChanged();

        _image = GetComponent<Image>();
        ChangePlayerColor();
    }

    private void ChangePlayerColor()
    {
        if (_image == null) { return; }

        switch (_playerState)
        {
            case PlayerType.red:
                _image.color = new Color(255, 0, 0);
                break;
            case PlayerType.blue:
                _image.color = new Color(0, 255, 255);
                break;
            case PlayerType.green:
                _image.color = new Color(0, 255, 0);
                break;
            case PlayerType.yellow:
                _image.color = new Color(255, 255, 0);
                break;
        }
    }

    private void OnCellStateChanged()
    {
        if (_view == null) { return; }

        switch (ActionStateType)
        {
            case ActionStateType.defense:
                _view.text = "‹’“_";
                break;
            case ActionStateType.fuel:
                _view.text = "‹’“_F”R—¿•Û";
                break;
            case ActionStateType.carry:
                _view.text = "•â“UêŠF”R—¿‰^”À’†";
                break;
            case ActionStateType.why:
                _view.text = "•â“UêŠF”R—¿–¢•Û";
                break;
            case ActionStateType.other:
                _view.text = "—£‚ê‚½êŠ‚É‚¢‚Ü‚·";
                break;
        }

    }

    
}
