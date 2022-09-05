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
    [Tooltip("エネミー共通のタグ")]
    [SerializeField] string _enemyTag = "";
    [Tooltip("攻撃力")]
    [SerializeField] int _atk;

    PhotonView _view;
    Rigidbody _rb;
    /// <summary>目的地の座標</summary>
    Vector3 _moveDestination;
    /// <summary>目的地の座標</summary>
    Vector3 _attackDestination;
    /// <summary>プレイヤーの現在の移動状態</summary>
    PlayerMoveState _moveState;
    /// <summary>プレイヤーの現在のエイム状態</summary>
    //PlayerAimState _aimState;
    /// <summary>攻撃のストック数</summary>
    int _attackStock = 0;
    /// <summary>マウスの座標</summary>
    Vector2 _mousePosition;
    /// <summary>右ボタンが押されてるか</summary>
    bool _attackButtonDown = false;
    /// <summary>左ボタンが押されてるか</summary>
    bool _moveButtonDown = false;

    void Start()
    {
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            _rb = GetComponent<Rigidbody>();
            _moveDestination = transform.position;
            if (!_attackTargetObject)
            {
                _attackTargetObject = new GameObject();
                _attackTargetObject.name = nameof(_attackTargetObject);
                Debug.LogWarning($"{nameof(_attackTargetObject)}がアサインされていないため、仮のオブジェクトを使用します");
            }
            _attackTargetObject = Instantiate(_attackTargetObject);
            _attackTargetObject.SetActive(false);
            if (!_moveTargetObject)
            {
                _moveTargetObject = new GameObject();
                _moveTargetObject.name = nameof(_moveTargetObject);
                Debug.LogWarning($"{nameof(_moveTargetObject)}がアサインされていないため、仮のオブジェクトを使用します");
            }
            _moveTargetObject = Instantiate(_moveTargetObject);
            _moveTargetObject.SetActive(false);
            _rb.transform.position = _moveDestination;
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
        _attackStock = _maxAttackStock;
        StartCoroutine(AttackRecharge());
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
        AttackAim();
        MoveAim();
        Move();

    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    void Move()
    {
        if (_moveState == PlayerMoveState.Move || _moveState == PlayerMoveState.Attack)
        {
            Vector3 destination;
            if (_moveState == PlayerMoveState.Attack)
            {
                destination = _attackDestination;
            }
            else if (_moveState == PlayerMoveState.Move)
            {
                destination = _moveDestination;
            }
            else
            {
                destination = transform.position;
            }
            if (Vector3.Distance(transform.position, destination) < _stoppingDistance)
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
                _moveState = PlayerMoveState.Idol;
            }
            else
            {
                float speed = 0;
                if (_moveState == PlayerMoveState.Attack) speed = _attackSpeed;
                else if (_moveState == PlayerMoveState.Move) speed = _moveSpeed;
                Vector3 dir = destination - transform.position;
                dir.y = 0;
                dir = dir.normalized * speed;
                _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
                Vector3 angle = transform.eulerAngles;
                angle = new Vector3(angle.x, Mathf.Atan2(-dir.z, dir.x) / Mathf.PI * 180, angle.z);
                transform.eulerAngles = angle;
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
        while (true)
        {
            if (_attackStock < _maxAttackStock)
            {
                if (_attackCoolTime > 0)
                {
                    yield return new WaitForSeconds(_attackCoolTime);
                }
                _attackStock++;
                Debug.Log($"ストック回復{_attackStock}");
            }
            yield return null;
        }
    }

    /// <summary>
    /// アタック時の移動先を指定する
    /// </summary>
    /// <returns></returns>
    void AttackAim()
    {
        //_aimState = PlayerAimState.Attack;
        //while (true)
        //{

        if (!_attackButtonDown)
        {
            _attackTargetObject.SetActive(false);
            //yield break;
            return;
        }
        if (!_attackTargetObject.activeSelf)
        {
            _attackTargetObject.SetActive(true);
        }
        Vector3 point = PointGet();
        float dis = Vector3.Distance(transform.position, point);
        Vector3 des = dis <= _attackLange ? point : transform.position + (point - transform.position) * (_attackLange / dis);
        _attackTargetObject.transform.position = des;
        //yield return null;
        //}
    }

    /// <summary>
    /// 攻撃のストックがあれば攻撃を開始する
    /// </summary>
    void Attack()
    {
        _attackDestination = _attackTargetObject.transform.position;
    }


    /// <summary>
    /// 通常移動時の移動先を指定する
    /// </summary>
    /// <returns></returns>
    void MoveAim()
    {
        if (!_moveButtonDown)
        {
            _moveDestination = _moveTargetObject.transform.position;
            _moveTargetObject.SetActive(false);
            return;
        }
        if (!_moveTargetObject.activeSelf)
        {
            _moveTargetObject.SetActive(true);
        }
        _moveTargetObject.transform.position = PointGet();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        if (_moveState == PlayerMoveState.Attack)
        {
            Debug.Log(2);
            if (other.CompareTag(_enemyTag))
            {
                Debug.Log(3);
                if (other.gameObject.TryGetComponent<Enemy>(out Enemy e))
                {
                    Debug.Log(4);
                    e.CallGetDamage(_atk);
                }
            }
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
            _attackButtonDown = true;
        }
        if (context.canceled)
        {
            _attackButtonDown = false;
            if (_attackStock > 0)
            {
                _moveState = PlayerMoveState.Attack;
                Attack();
                _attackStock--;
                Debug.Log($"ストック消費{_attackStock}");
            }
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
            _moveButtonDown = true;
            //StartCoroutine(MoveAim());
        }
        if (context.canceled)
        {
            if (_moveState != PlayerMoveState.Attack)
            {
                _moveState = PlayerMoveState.Move;
            }
            _moveButtonDown = false;
        }
        //_aimState = PlayerAimState.Non;
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
/// プレイヤーの行動状態
/// </summary>
public enum PlayerMoveState
{
    /// <summary>待機</summary>
    Idol,
    /// <summary>移動</summary>
    Move,
    /// <summary>攻撃</summary>
    Attack,
}

/// <summary>
/// プレイヤーのエイム状態
/// </summary>
//public enum PlayerAimState
//{
//    /// <summary>待機</summary>
//    Non,
//    /// <summary>攻撃</summary>
//    Attack,
//    /// <summary>移動</summary>
//    Move,
//}