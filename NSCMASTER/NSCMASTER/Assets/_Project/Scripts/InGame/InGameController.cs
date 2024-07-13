using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameController : MonoBehaviourPunCallbacks
{
    //[SerializeField] private string username;

    //// UI Elements
    //[Header("UI Elements")]
    //[SerializeField] public GameObject Lobby;
    //[SerializeField] public GameObject Join;
    //[SerializeField] public TMP_InputField JoinRoomCode;
    //[SerializeField] TextMeshProUGUI Roomcode;

    #region Unity Method

    //private void Awake()
    //{
    //    username = PlayerPrefs.GetString("Username");
    //    UIInvite.OnRoomInviteAccept += HandleRoomInviteAccept;
    //}
    //private void OnDestroy()
    //{
    //    UIInvite.OnRoomInviteAccept -= HandleRoomInviteAccept;
    //}
    //private void Start()
    //{

    //    ConnectToPhoton();
    //}
    //#endregion
    //#region Privete Methods
    //private void ConnectToPhoton()
    //{
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        Debug.LogWarning("Already connected to Photon.");
    //        if (!PhotonNetwork.InLobby)
    //        {
    //            PhotonNetwork.JoinLobby();
    //        }
    //        else
    //        {
    //            GetPhotonFriends?.Invoke();
    //        }
    //        return;
    //    }


    //    Debug.Log($"Connect to Photon as {username}");
    //    PhotonNetwork.AuthValues = new AuthenticationValues(username);
    //    PhotonNetwork.AutomaticallySyncScene = true;
    //    PhotonNetwork.NickName = username;
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    //private void HandleRoomInviteAccept(string roomName)
    //{
    //    PlayerPrefs.SetString("RoomName", roomName);
    //    if (PhotonNetwork.InRoom)
    //    {
    //        PhotonNetwork.LeaveRoom();
    //    }
    //    else
    //    {
    //        if (PhotonNetwork.InLobby)
    //        {
    //            JoinPlayerRoom();
    //        }
    //    }
    //}
    //private void JoinPlayerRoom()
    //{
    //    string roomName = PlayerPrefs.GetString("RoomName");
    //    PlayerPrefs.SetString("RoomName", "");
    //    PhotonNetwork.JoinRoom(roomName);
    //}
    #endregion
    #region Public Methods


    #endregion
    #region Photon Callbacks
    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected to Photon Master Server");
    //    if (!PhotonNetwork.InLobby)
    //    {
    //        PhotonNetwork.JoinLobby();
    //    }
    //}
    //public override void OnJoinedLobby()
    //{
    //    Debug.Log("Joined to Photon Lobby");
    //    GetPhotonFriends?.Invoke();
    //    string roomName = PlayerPrefs.GetString("RoomName");
    //    if (!string.IsNullOrEmpty(roomName))
    //    {
    //        JoinPlayerRoom();
    //    }
    //}
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"Joined to Room {PhotonNetwork.CurrentRoom.Name}");
    //    Debug.Log($"Number of players in the room: {PhotonNetwork.CurrentRoom.PlayerCount}");
    //    GetPhotonFriends?.Invoke();
    //    Debug.Log("GetPhotonFriends Invoked");

    //}
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room");
    }
    #endregion
}
