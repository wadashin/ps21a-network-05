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

    PhotonView _view;
    Rigidbody _rb;
    /// <summary>�ړI�n�̍��W</summary>
    Vector3 _destination;
    /// <summary>�v���C���[�̌��݂̈ړ����</summary>
    PlayerMoveState _moveState;
    /// <summary>�v���C���[�̌��݂̃G�C�����</summary>
    PlayerAimState _aimState;
    /// <summary>�U���̃X�g�b�N��</summary>
    int _attackStock = 0;
    /// <summary>�}�E�X�̍��W</summary>
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
            _rb.transform.position = _destination;
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
    /// �v���C���[���ړ�������
    /// </summary>
    void Move()
    {
        if (_moveState == PlayerMoveState.Move || _moveState == PlayerMoveState.Attack)
        {
            if (Vector3.Distance(transform.position, _destination) < _stoppingDistance)
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
                _moveState = PlayerMoveState.Idol;
            }
            else
            {
                float speed = 0;
                if (_moveState == PlayerMoveState.Attack) speed = _attackSpeed;
                else if (_moveState == PlayerMoveState.Move) speed = _moveSpeed;
                Vector3 dir = _destination - transform.position;
                dir.y = 0;
                dir = dir.normalized * speed;
                _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
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
        yield return _attackCoolTime;
        if (_attackStock < _maxAttackStock)
        {
            _attackStock++;
        }
    }

    /// <summary>
    /// �A�^�b�N���̈ړ�����w�肷��
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackAim()
    {
        while (true)
        {
            if (_moveState == PlayerMoveState.Attack)
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
    /// �ʏ�ړ����̈ړ�����w�肷��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAim()
    {
        while (true)
        {
            if (_aimState != PlayerAimState.Move)
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

    #region ���͎�t��

    /// <summary>
    /// �U���L�[����
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
            _moveState = PlayerMoveState.Attack;
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
            Debug.Log(1);
            _aimState = PlayerAimState.Move;
            _moveState = PlayerMoveState.Move;
            StartCoroutine(MoveAim());
        }
        if (context.canceled)
        {
            _aimState = PlayerAimState.Non;
        }
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
public enum PlayerAimState
{
    /// <summary>�ҋ@</summary>
    Non,
    /// <summary>�U��</summary>
    Attack,
    /// <summary>�ړ�</summary>
    Move,
}