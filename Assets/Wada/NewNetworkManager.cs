using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Unity 2019.1.11f1
/// 
/// Pun: 2.4
/// 
/// Photon lib: 4.1.2.4
/// 
/// </summary>

public class NewNetworkManager : MonoBehaviourPunCallbacks
{
    /////////////////////////////////////////////////////////////////////////////////////
    // Field ////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    [Header("DefaultRoomSettings")]

    // �ő�l��
    [SerializeField] private int maxPlayers = 4;

    // ���J�E����J
    [SerializeField] private bool isVisible = true;

    // �����̉�
    [SerializeField] private bool isOpen = true;

    // ������
    [SerializeField] private string roomName = "Knohhoso's Room";

    // �X�e�[�W
    [SerializeField] private string stageName = "Stage1";

    // ��Փx
    [SerializeField] private string stageDifficulty = "Easy";

    // ���[�����X�g
    private List<RoomInfo> roomInfoList = new List<RoomInfo>();


    /////////////////////////////////////////////////////////////////////////////////////
    // Awake & Start ////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // Awake
    private void Awake()
    {
        // �V�[���̎�������: ����
        PhotonNetwork.AutomaticallySyncScene = false;
    }


    // Start is called before the first frame update
    private void Start()
    {
        // Photon�ɐڑ�
        Connect("1.0");
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Connect //////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // Photon�ɐڑ�����
    private void Connect(string gameVersion)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // �j�b�N�l�[����t����
    private void SetMyNickName(string nickName)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Join Lobby ///////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // ���r�[�ɓ���
    private void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Join Room ////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // 1. �������쐬���ē�������
    public void CreateAndJoinRoom()
    {
        // ���[���I�v�V�����̊�{�ݒ�
        RoomOptions roomOptions = new RoomOptions
        {
            // �����̍ő�l��
            MaxPlayers = (byte)maxPlayers,

            // ���J
            IsVisible = isVisible,

            // ������
            IsOpen = isOpen
        };

        // ���[���I�v�V�����ɃJ�X�^���v���p�e�B��ݒ�
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Stage", stageName },
            { "Difficulty", stageDifficulty }
        };
        roomOptions.CustomRoomProperties = customRoomProperties;

        // ���r�[�Ɍ��J����J�X�^���v���p�e�B���w��
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Stage", "Difficulty" };

        // �������쐬���ē�������
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }


    // 2. �����ɓ������� �i���݂��Ȃ���΍쐬���ē�������j
    public void JoinOrCreateRoom()
    {
        // ���[���I�v�V�����̊�{�ݒ�
        RoomOptions roomOptions = new RoomOptions
        {
            // �����̍ő�l��
            MaxPlayers = (byte)maxPlayers,

            // ���J
            IsVisible = isVisible,

            // ������
            IsOpen = isOpen
        };

        // ���[���I�v�V�����ɃJ�X�^���v���p�e�B��ݒ�
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Stage", stageName },
            { "Difficulty", stageDifficulty }
        };
        roomOptions.CustomRoomProperties = customRoomProperties;

        // ���r�[�Ɍ��J����J�X�^���v���p�e�B���w��
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Stage", "Difficulty" };

        // ���� (���݂��Ȃ���Ε������쐬���ē�������)
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }


    // 3. ����̕����ɓ�������
    public void JoinRoom(string targetRoomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRoom(targetRoomName);
        }
    }


    // 4. �����_���ȕ����ɓ�������
    public void JoinRandomRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Leave Room ///////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // ��������ގ�����
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            // �ގ�
            PhotonNetwork.LeaveRoom();
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Update RoomProperties ////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // ���[���I�v�V�������X�V����
    private void UpdateRoomOptions(int newMaxPlayers, bool newIsVisible, bool newIsOpen)
    {
        if (PhotonNetwork.InRoom)
        {
            // �ő�l��
            PhotonNetwork.CurrentRoom.MaxPlayers = (byte)newMaxPlayers;

            // ���J�E����J
            PhotonNetwork.CurrentRoom.IsVisible = newIsVisible;

            // �����̉�
            PhotonNetwork.CurrentRoom.IsOpen = newIsOpen;
        }
    }


    // �J�X�^���v���p�e�B���X�V����
    private void UpdateRoomCustomProperties(string newStageName, string newStageDifficulty)
    {
        if (PhotonNetwork.InRoom)
        {
            // �X�e�[�W�Ɠ�Փx���X�V
            ExitGames.Client.Photon.Hashtable customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customRoomProperties["Stage"] = newStageName;
            customRoomProperties["Difficulty"] = newStageDifficulty;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Find Room ////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // ������T��
    public void FindRoom()
    {
        if (roomInfoList != null)
        {
            // ���[�����X�g��W�J���ĖړI�̕�����T��
            foreach (RoomInfo roomInfo in roomInfoList)
            {
                // �X�e�[�W�ōi�荞��
                if (roomInfo.CustomProperties["Stage"] as string == stageName)
                {
                    // ��Փx�ōi�荞��
                    if (roomInfo.CustomProperties["Difficulty"] as string == stageDifficulty)
                    {
                        // �����ɍ��v���������̏���\��
                        Debug.Log("RoomName: " + roomInfo.Name);
                        Debug.Log("Slots: " + roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers);
                    }
                }
            }
        }
    }


    // ������T���ē�������
    public void FindAndJoinRoom()
    {
        if (roomInfoList != null)
        {
            // ���[�����X�g��W�J���ĖړI�̕�����T��
            foreach (RoomInfo roomInfo in roomInfoList)
            {
                // �X�e�[�W�ōi�荞��
                if (roomInfo.CustomProperties["Stage"] as string == stageName)
                {
                    // ��Փx�ōi�荞��
                    if (roomInfo.CustomProperties["Difficulty"] as string == stageDifficulty)
                    {
                        // �����łȂ���Ε����ɓ�������
                        if (roomInfo.PlayerCount < roomInfo.MaxPlayers)
                        {
                            JoinRoom(roomInfo.Name);
                            return;
                        }
                    }
                }
            }
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    // Pun Callbacks ////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // Photon�ɐڑ�������
    public override void OnConnected()
    {
        Debug.Log("OnConnected");

        // �j�b�N�l�[����t����
        SetMyNickName("Knohhoso");
    }


    // Photon����ؒf���ꂽ��
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }


    // �}�X�^�[�T�[�o�[�ɐڑ�������
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        // ���r�[�ɓ���
        JoinLobby();
    }


    // ���r�[�ɓ�������
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }


    // ���r�[����o����
    public override void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }


    // �������쐬������
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }


    // �����̍쐬�Ɏ��s������
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }


    // �����ɓ���������
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");

        // �����̏���\��
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("RoomName: " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("HostName: " + PhotonNetwork.MasterClient.NickName);
            Debug.Log("Stage: " + PhotonNetwork.CurrentRoom.CustomProperties["Stage"] as string);
            Debug.Log("Difficulty: " + PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"] as string);
            Debug.Log("Slots: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
        }
    }


    // ����̕����ւ̓����Ɏ��s������
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }


    // �����_���ȕ����ւ̓����Ɏ��s������
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }


    // ��������ގ�������
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }


    // ���̃v���C���[���������Ă�����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
    }


    // ���̃v���C���[���ގ�������
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
    }


    // �}�X�^�[�N���C�A���g���ς������
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched");
    }


    // ���r�[�ɍX�V����������
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }


    // ���[�����X�g�ɍX�V����������
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");

        // �����̕������X�g���N���A
        if (roomInfoList != null)
        {
            roomInfoList.Clear();
        }

        // �V�������[�����X�g�ɍX�V
        roomInfoList = roomList;
    }


    // ���[���v���p�e�B���X�V���ꂽ��
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");

        // �����̏���\��
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("RoomName: " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("HostName: " + PhotonNetwork.MasterClient.NickName);
            Debug.Log("Stage: " + PhotonNetwork.CurrentRoom.CustomProperties["Stage"] as string);
            Debug.Log("Difficulty: " + PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"] as string);
            Debug.Log("Slots: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
        }
    }


    // �v���C���[�v���p�e�B���X�V���ꂽ��
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
    }


    // �t�����h���X�g�ɍX�V����������
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("OnFriendListUpdate");
    }


    // �n�惊�X�g���󂯎������
    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }


    // WebRpc�̃��X�|���X����������
    public override void OnWebRpcResponse(OperationResponse response)
    {
        Debug.Log("OnWebRpcResponse");
    }


    // �J�X�^���F�؂̃��X�|���X����������
    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }


    // �J�X�^���F�؂����s������
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }
}