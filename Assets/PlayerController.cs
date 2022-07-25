using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーを操作するコンポーネント
/// クリックした場所に移動する
/// </summary>
[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("移動速度")]
    [SerializeField] float _moveSpeed = 6f;
    [Tooltip("攻撃速度")]
    [SerializeField] float _attackSpeed = 20f;
    [Tooltip("攻撃可能距離")]
    [SerializeField] float _attackLange = 10f;
    [Tooltip("クリック可能なレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("移動のためのレイキャストの距離")]
    [SerializeField] float _raycastDistance = 20f;
    [Tooltip("ストップする目的地からの距離")]
    [SerializeField] float _stoppingDistance = 1f;
    [Tooltip("攻撃ターゲットを示すオブジェクト")]
    [SerializeField] GameObject _attackTargetObject;
    [Tooltip("通常移動先を示すオブジェクト")]
    [SerializeField] GameObject _moveTargetObject;
    [Tooltip("攻撃の最大ストック数")]
    [SerializeField] int _maxAttackStock = 3;
    [Tooltip("攻撃のクールタイム")]
    [SerializeField] float _attackCoolTime = 1f;

    PhotonView _view;
    Rigidbody _rb;
    /// <summary>目的地の座標</summary>
    Vector3 _destination;
    /// <summary>プレイヤーの現在の状態</summary>
    PlayerState _state;
    /// <summary>攻撃のストック数</summary>
    int _attackStock = 0;
    /// <summary>マウスの座標</summary>
    Vector2 _mousePosition;

    void Start()
    {
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            _rb = GetComponent<Rigidbody>();
            _destination = transform.position;
            if (!_attackTargetObject)
            {
                _attackTargetObject = new GameObject();
                _attackTargetObject.name = nameof(_attackTargetObject);
                Debug.LogWarning($"{nameof(_attackTargetObject)}がアサインされていないため、仮のオブジェクトを私用します");
            }
            _attackTargetObject = Instantiate(_attackTargetObject);
            _attackTargetObject.SetActive(false);
            if (!_moveTargetObject)
            {
                _moveTargetObject = new GameObject();
                _moveTargetObject.name = nameof(_moveTargetObject);
                Debug.LogWarning($"{nameof(_moveTargetObject)}がアサインされていないため、仮のオブジェクトを私用します");
            }
            _moveTargetObject = Instantiate(_moveTargetObject);
            _moveTargetObject.SetActive(false);
            _rb.transform.position = _destination;
            // カメラをセットアップする
            var vcam = FindObjectOfType<CinemachineVirtualCameraBase>();
            vcam.LookAt = transform;
            vcam.Follow = transform;

            // マスタークライアントからゲーム開始イベントを fire する
            if (PhotonNetwork.IsMasterClient)
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                raiseEventOptions.Receivers = ReceiverGroup.All;
                SendOptions sendOptions = new SendOptions();
                PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, sendOptions);
            }
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;
        //if (Input.GetButtonUp("Fire2"))
        //{
        //    _state = PlayerState.Move;
        //    PointSet();
        //}
        //if (Input.GetButtonUp("Fire1"))
        //{
        //}
        //if (Input.GetButton("Fire2"))
        //{

        //}
        Move();

    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    void Move()
    {
        if (_state == PlayerState.Move || _state == PlayerState.Attack)
        {
            if (Vector3.Distance(transform.position, _destination) < _stoppingDistance)
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
                _state = PlayerState.Idol;
            }
            else
            {
                float speed = 0;
                if (_state == PlayerState.Attack) speed = _attackSpeed;
                else if (_state == PlayerState.Move) speed = _moveSpeed;
                Vector3 dir = _destination - transform.position;
                dir.y = 0;
                dir = dir.normalized * speed;
                _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
            }
        }
    }

    /// <summary>
    /// 移動先の座標を設定する
    /// </summary>
    void PointSet()
    {
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _layerMask))
        //{
        //    _destination = hit.point;
        //}
    }

    /// <summary>
    /// ポインターの位置を取得 
    /// </summary>
    /// <returns></returns>
    Vector3 PointGet()
    {
        var ray = Camera.main.ScreenPointToRay((Vector3)_mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _layerMask))
        {
            return hit.point;
        }
        return transform.position;
    }

    /// <summary>
    /// 攻撃のストックをチャージする
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackRecharge()
    {
        yield return _attackCoolTime;
        if (_attackStock < _maxAttackStock)
        {
            _attackStock++;
        }
    }

    /// <summary>
    /// アタック時の移動先を指定する
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackAim()
    {
        while (true)
        {
            if (_state == PlayerState.Attack)
            {
                _attackTargetObject.SetActive(false);
                yield break;
            }
            if (!_attackTargetObject.activeSelf)
            {
                _attackTargetObject.SetActive(true);
            }
            Vector3 point = PointGet();
            float dis = Vector3.Distance(transform.position, point);
            _destination = dis <= _attackLange ? point : transform.position + (point - transform.position) * (_attackLange / dis);
            _attackTargetObject.transform.position = _destination;
            yield return null;
        }
    }


    /// <summary>
    /// 通常移動時の移動先を指定する
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAim()
    {
        while (true)
        {
            if (_state == PlayerState.Move)
            {
                _moveTargetObject.SetActive(false);
                yield break;
            }
            if (!_moveTargetObject.activeSelf)
            {
                _moveTargetObject.SetActive(true);
                Debug.Log(2);
            }
            _destination = PointGet();
            _moveTargetObject.transform.position = _destination;
            yield return null;
        }
    }

    #region 入力受付部

    /// <summary>
    /// 攻撃キー入力
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine(AttackAim());
        }
        if (context.canceled)
        {
            _state = PlayerState.Attack;
        }
    }

    /// <summary>
    /// 移動キー入力
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(1);
            StartCoroutine(MoveAim());
        }
        if (context.canceled)
        {
            _state = PlayerState.Move;
        }
    }

    /// <summary>
    /// ポインター座標入力
    /// </summary>
    /// <param name="context"></param>
    public void OnPoint(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }

    #endregion

}


/// <summary>
/// プレイヤーの状態
/// </summary>
public enum PlayerState
{
    /// <summary>待機</summary>
    Idol,
    /// <summary>移動</summary>
    Move,
    /// <summary>攻撃</summary>
    Attack,
}