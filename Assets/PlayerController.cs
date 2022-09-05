using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�𑀍삷��R���|�[�l���g
/// �N���b�N�����ꏊ�Ɉړ�����
/// </summary>
[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("�ړ����x")]
    [SerializeField] float _moveSpeed = 6f;
    [Tooltip("�U�����x")]
    [SerializeField] float _attackSpeed = 20f;
    [Tooltip("�U���\����")]
    [SerializeField] float _attackLange = 10f;
    [Tooltip("�N���b�N�\�ȃ��C���[")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("�ړ��̂��߂̃��C�L���X�g�̋���")]
    [SerializeField] float _raycastDistance = 20f;
    [Tooltip("�X�g�b�v����ړI�n����̋���")]
    [SerializeField] float _stoppingDistance = 1f;
    [Tooltip("�U���^�[�Q�b�g�������I�u�W�F�N�g")]
    [SerializeField] GameObject _attackTargetObject;
    [Tooltip("�ʏ�ړ���������I�u�W�F�N�g")]
    [SerializeField] GameObject _moveTargetObject;
    [Tooltip("�U���̍ő�X�g�b�N��")]
    [SerializeField] int _maxAttackStock = 3;
    [Tooltip("�U���̃N�[���^�C��")]
    [SerializeField] float _attackCoolTime = 1f;
    [Tooltip("�G�l�~�[���ʂ̃^�O")]
    [SerializeField] string _enemyTag = "";
    [Tooltip("�U����")]
    [SerializeField] int _atk;

    PhotonView _view;
    Rigidbody _rb;
    /// <summary>�ړI�n�̍��W</summary>
    Vector3 _moveDestination;
    /// <summary>�ړI�n�̍��W</summary>
    Vector3 _attackDestination;
    /// <summary>�v���C���[�̌��݂̈ړ����</summary>
    PlayerMoveState _moveState;
    /// <summary>�v���C���[�̌��݂̃G�C�����</summary>
    //PlayerAimState _aimState;
    /// <summary>�U���̃X�g�b�N��</summary>
    int _attackStock = 0;
    /// <summary>�}�E�X�̍��W</summary>
    Vector2 _mousePosition;
    /// <summary>�E�{�^����������Ă邩</summary>
    bool _attackButtonDown = false;
    /// <summary>���{�^����������Ă邩</summary>
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
                Debug.LogWarning($"{nameof(_attackTargetObject)}���A�T�C������Ă��Ȃ����߁A���̃I�u�W�F�N�g���g�p���܂�");
            }
            _attackTargetObject = Instantiate(_attackTargetObject);
            _attackTargetObject.SetActive(false);
            if (!_moveTargetObject)
            {
                _moveTargetObject = new GameObject();
                _moveTargetObject.name = nameof(_moveTargetObject);
                Debug.LogWarning($"{nameof(_moveTargetObject)}���A�T�C������Ă��Ȃ����߁A���̃I�u�W�F�N�g���g�p���܂�");
            }
            _moveTargetObject = Instantiate(_moveTargetObject);
            _moveTargetObject.SetActive(false);
            _rb.transform.position = _moveDestination;
            // �J�������Z�b�g�A�b�v����
            var vcam = FindObjectOfType<CinemachineVirtualCameraBase>();
            vcam.LookAt = transform;
            vcam.Follow = transform;

            // �}�X�^�[�N���C�A���g����Q�[���J�n�C�x���g�� fire ����
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
    /// �v���C���[���ړ�������
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
    /// �ړ���̍��W��ݒ肷��
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
    /// �|�C���^�[�̈ʒu���擾 
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
    /// �U���̃X�g�b�N���`���[�W����
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
                Debug.Log($"�X�g�b�N��{_attackStock}");
            }
            yield return null;
        }
    }

    /// <summary>
    /// �A�^�b�N���̈ړ�����w�肷��
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
    /// �U���̃X�g�b�N������΍U�����J�n����
    /// </summary>
    void Attack()
    {
        _attackDestination = _attackTargetObject.transform.position;
    }


    /// <summary>
    /// �ʏ�ړ����̈ړ�����w�肷��
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


    #region ���͎�t��

    /// <summary>
    /// �U���L�[����
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
                Debug.Log($"�X�g�b�N����{_attackStock}");
            }
        }
    }

    /// <summary>
    /// �ړ��L�[����
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
    /// �|�C���^�[���W����
    /// </summary>
    /// <param name="context"></param>
    public void OnPoint(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }

    #endregion

}


/// <summary>
/// �v���C���[�̍s�����
/// </summary>
public enum PlayerMoveState
{
    /// <summary>�ҋ@</summary>
    Idol,
    /// <summary>�ړ�</summary>
    Move,
    /// <summary>�U��</summary>
    Attack,
}

/// <summary>
/// �v���C���[�̃G�C�����
/// </summary>
//public enum PlayerAimState
//{
//    /// <summary>�ҋ@</summary>
//    Non,
//    /// <summary>�U��</summary>
//    Attack,
//    /// <summary>�ړ�</summary>
//    Move,
//}