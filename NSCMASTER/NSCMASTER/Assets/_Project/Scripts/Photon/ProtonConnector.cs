using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ProtonConnector : MonoBehaviourPunCallbacks
{
    [SerializeField] private string username;

    // UI Elements
    [Header("UI Elements")]
    [SerializeField] public GameObject Lobby;
    [SerializeField] public GameObject Join;
    [SerializeField] public TMP_InputField JoinRoomCode;
    [SerializeField] TextMeshProUGUI Roomcode;
    public static Action GetPhotonFriends = delegate { };

    #region Unity Method

    private void Awake()
    {
        username = PlayerPrefs.GetString("Username");
        UIInvite.OnRoomInviteAccept += HandleRoomInviteAccept;
    }
    private void OnDestroy()
    {
        UIInvite.OnRoomInviteAccept -= HandleRoomInviteAccept;
    }
    private void Start()
    {
        
        ConnectToPhoton();
    }
    #endregion
    #region Privete Methods
    private void ConnectToPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Already connected to Photon.");
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                GetPhotonFriends?.Invoke();
            }
            return;
        }


        Debug.Log($"Connect to Photon as {username}");
        PhotonNetwork.AuthValues = new AuthenticationValues(username);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = username;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void CreatePhotonRoom()
    {
        string roomName = $"{UnityEngine.Random.Range(100000, 999999)}";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        Debug.Log($"Create Room {roomName}");
        //ScreenController.LoadScreen("Ingame");
        ScreenController.LoadScreen("ConfigRoom");
        Debug.Log("=====> ConfigRoom Loaded <=====");
    }
    private void HandleRoomInviteAccept(string roomName)
    {
        PlayerPrefs.SetString("RoomName", roomName);
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(PhotonNetwork.InLobby)
            {
                JoinPlayerRoom();
            }
        }
    }
    private void JoinPlayerRoom()
    {
        string roomName = PlayerPrefs.GetString("RoomName");
        PlayerPrefs.SetString("RoomName", "");
        PhotonNetwork.JoinRoom(roomName);
    }
    #endregion
    #region Public Methods
    public void OnCreateRoomClicked()
    {
        CreatePhotonRoom();
    }
    public void OnJoinRoomClicked()
    {
        Lobby.SetActive(false);
        Join.SetActive(true);
        if (string.IsNullOrEmpty(JoinRoomCode.text))
        {
            Debug.LogWarning("Room code is empty");
            return;
        }
        PlayerPrefs.SetString("RoomName", JoinRoomCode.text);
        JoinPlayerRoom();
    }


    #endregion
    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to Photon Lobby");
        GetPhotonFriends?.Invoke();
        string roomName = PlayerPrefs.GetString("RoomName");
        if (!string.IsNullOrEmpty(roomName))
        {
            JoinPlayerRoom();
        }
    }
    public override void OnCreatedRoom()
    {
        Debug.Log($"Room Created {PhotonNetwork.CurrentRoom.Name}");
        //Roomcode.text = PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined to Room {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Number of players in the room: {PhotonNetwork.CurrentRoom.PlayerCount}");
        GetPhotonFriends?.Invoke();
        Debug.Log("GetPhotonFriends Invoked");

    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Create Room Failed {message}");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Failed {message}");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered to room");
        Debug.Log($"Number of players in the room: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"Master Client Switched to {newMasterClient.NickName}");
    }
    #endregion
}

