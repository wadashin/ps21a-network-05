using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// �v���C���[�𑀍삷��R���|�[�l���g
/// �N���b�N�����ꏊ�Ɉړ�����
/// </summary>
[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// <summary>�ړ����x</summary>
    [SerializeField] float _moveSpeed = 6f;
    /// <summary>�U�����x</summary>
    [SerializeField] float _attackSpeed = 20f;
    /// <summary>�N���b�N�\�ȃ��C���[</summary>
    [SerializeField] LayerMask _layerMask;
    /// <summary>�ړ��̂��߂̃��C�L���X�g�̋���</summary>
    [SerializeField] float _raycastDistance = 20f;
    /// <summary>�X�g�b�v����ړI�n����̋���</summary>
    [SerializeField] float _stoppingDistance = 1f;
    PhotonView _view;
    Rigidbody _rb;
    /// <summary>�ړI�n�̍��W</summary>
    Vector3 _destination;
    /// <summary>�v���C���[�̌��݂̏��</summary>
    PlayerState _state;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _destination = transform.position;

        if (_view.IsMine)
        {
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
        if (_state == PlayerState.Idol)
        {
            if (Input.GetButton("Fire2"))
            {
                PointSet();
                _state = PlayerState.Move;
            }
            if (Input.GetButton("Fire1"))
            {
                PointSet();
                _state = PlayerState.Attack;
            }
        }

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
                if(_state == PlayerState.Attack) speed = _attackSpeed;
                else if(_state == PlayerState.Move) speed = _moveSpeed;
                Vector3 dir = _destination - transform.position;
                dir.y = 0;
                dir = dir.normalized * speed;
                _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
            }
        }
    }

    void PointSet()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _layerMask))
        {
            _destination = hit.point;
        }
    }

}


/// <summary>
/// �v���C���[�̏��
/// </summary>
public enum PlayerState
{
    /// <summary>�ҋ@</summary>
    Idol,
    /// <summary>�ړ�</summary>
    Move,
    /// <summary>�U��</summary>
    Attack,
}