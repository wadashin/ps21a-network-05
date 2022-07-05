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
    [SerializeField] float _speed = 6f;
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

        if (Input.GetButton("Fire2"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _layerMask))
            {
                _destination = hit.point;
            }
        }

        if (Vector3.Distance(transform.position, _destination) < _stoppingDistance)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        else
        {
            Vector3 dir = _destination - transform.position;
            dir.y = 0;
            dir = dir.normalized * _speed;
            _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
        }
    }
}
