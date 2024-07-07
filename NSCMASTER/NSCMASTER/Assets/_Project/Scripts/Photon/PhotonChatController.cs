//using Photon.Pun;
//using Photon.Chat;
//using UnityEngine;
//using ExitGames.Client.Photon;
//using System;

//public class PhotonChatController : MonoBehaviour, IChatClientListener
//{
//    [SerializeField] private string nickName;
//    private ChatClient chatClient;

//    public static Action<string, string> OnRoomInvite = delegate { };
//    #region Unity Methods
//    private void Awake()
//    {
//        nickName = PlayerPrefs.GetString("Username");
//        UIFriend.OnInviteFriend += HandleFriendInvite;
//    }
//    private void OnDestroy()
//    {
//        UIFriend.OnInviteFriend -= HandleFriendInvite;
//    }
//    private void Start()
//    {
//        chatClient = new ChatClient(this);
//        ConnectToPhotonChat();
//    }
//    private void Update()
//    {
//        chatClient.Service();
//    }
//    #endregion
//    #region Private Methods
//    private void ConnectToPhotonChat()
//    {
//        Debug.Log("Connecting to Photon Chat");
//        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nickName);
//        //ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
//        //chatClient.ConnectUsingSettings(chatSettings);
//    }
//    #endregion
//    #region Public Methods
//    public void HandleFriendInvite(string recipient)
//    {
//        chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
//    }

//    #endregion
//    #region Photon Chat Callbacks


//    public void DebugReturn(DebugLevel level, string message)
//    {
//        Debug.Log($"Debug Return : { message}");
//    }

//    public void OnDisconnected()
//    {
//        Debug.Log("You have disconnected to the Photon Chat");
//    }

//    public void OnConnected()
//    {
//        Debug.Log("You have connected to the Photon Chat");
//    }

//    public void OnChatStateChange(ChatState state)
//    {
//        Debug.Log($"Chat State Changed : {state}");
//    }

//    public void OnGetMessages(string channelName, string[] senders, object[] messages)
//    {
//        Debug.Log($"Photon Chat OngerMessages {channelName}");
//        for (int i = 0; i < senders.Length; i++)
//        {
//            Debug.Log($"Sender : {senders[i]} Message : {messages[i]}");
//        }
//    }

//    public void OnPrivateMessage(string sender, object message, string channelName)
//    {
//        if (!string.IsNullOrEmpty(message.ToString()))
//        {
//            //Channel Name format [Sender : Receiver]

//            string[] splitNames = channelName.Split(new char[] { ':' });
//            string senderName = splitNames[0];
//            if (!sender.Equals(senderName,StringComparison.OrdinalIgnoreCase))
//            {
//                Debug.Log($"Private Message from {sender} : {message}");
//                OnRoomInvite?.Invoke(sender, message.ToString());
//            }
//        }
//    }

//    public void OnSubscribed(string[] channels, bool[] results)
//    {
//        Debug.Log("Photon Chat OnSubscribed");
//        for (int i = 0; i < channels.Length; i++)
//        {
//            Debug.Log($"Channel : {channels[i]} Result : {results[i]}");
//        }
//    }

//    public void OnUnsubscribed(string[] channels)
//    {

//    }

//    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void OnUserSubscribed(string channel, string user)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void OnUserUnsubscribed(string channel, string user)
//    {
//        throw new System.NotImplementedException();
//    }
//    #endregion
//}
//using UnityEngine;
//using System;
//using Photon.Chat;
//using Photon.Pun;
//using ExitGames.Client.Photon;
//using System.Collections.Generic;
//using PlayFab.ClientModels;
//using Photon.Chat.Demo;
//using Photon.Realtime;
//using System.Collections;
//public class PhotonChatController : MonoBehaviour, IChatClientListener
//{
//    [SerializeField] private string nickName;
//    private ChatClient chatClient;

//    public static Action<string, string> OnRoomInvite = delegate { };
//    public static Action<ChatClient> OnChatConnected = delegate { };
//    public static Action<PhotonStatus> OnStatusUpdated = delegate { };

//    #region Unity Methods

//    private void Awake()
//    {
//        nickName = PlayerPrefs.GetString("Username");
//        UIFriend.OnInviteFriend += HandleFriendInvite;
//    }
//    private void OnDestroy()
//    {
//        UIFriend.OnInviteFriend -= HandleFriendInvite;
//    }

//    private void Start()
//    {
//        chatClient = new ChatClient(this);
//        ConnectoToPhotonChat();
//    }

//    private void Update()
//    {
//        chatClient.Service();
//    }

//    #endregion

//    #region  Private Methods

//    private void ConnectoToPhotonChat()
//    {
//        Debug.Log("Connecting to Photon Chat");
//        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nickName);
//        //ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
//        //chatClient.ConnectUsingSettings(chatSettings);
//    }
//    private IEnumerator WaitForRoomCreationAndInvite(string recipient)
//    {
//        // รอจนกว่าจะสร้างห้องเสร็จ
//        while (!PhotonNetwork.InRoom)
//        {
//            yield return null;
//        }
//        SendInvite(recipient);
//    }

//    private void SendInvite(string recipient)
//    {
//        Debug.Log($"Sending invite to {recipient} for room {PhotonNetwork.CurrentRoom.Name}");
//        chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
//    }

//    #endregion

//    #region  Public Methods

//    public void HandleFriendInvite(string recipient)
//    {
//        if (!PhotonNetwork.InRoom)
//        {
//            Debug.Log("Not in room, creating a new room.");
//            string roomName = $"Room_{nickName}";
//            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
//            PhotonNetwork.CreateRoom(roomName, roomOptions);
//            StartCoroutine(WaitForRoomCreationAndInvite(recipient));
//            return;
//        }
//        SendInvite(recipient);
//    }


//    #endregion

//    #region Photon Chat Callbacks

//    public void DebugReturn(DebugLevel level, string message)
//    {
//        Debug.Log($"Photon Chat DebugReturn: {message}");
//    }

//    public void OnDisconnected()
//    {
//        Debug.Log("You have disconnected from the Photon Chat");
//        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
//    }

//    public void OnConnected()
//    {
//        Debug.Log("You have connected to the Photon Chat");
//        OnChatConnected?.Invoke(chatClient);
//        chatClient.SetOnlineStatus(ChatUserStatus.Online);
//    }

//    public void OnChatStateChange(ChatState state)
//    {
//        Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
//    }

//    public void OnGetMessages(string channelName, string[] senders, object[] messages)
//    {
//        Debug.Log($"Photon Chat OnGetMessages {channelName}");
//        for (int i = 0; i < senders.Length; i++)
//        {
//            Debug.Log($"{senders[i]} messaged: {messages[i]}");
//        }
//    }

//    public void OnPrivateMessage(string sender, object message, string channelName)
//    {
//        if (!string.IsNullOrEmpty(message.ToString()))
//        {
//            // Channel Name format [Sender : Recipient]
//            string[] splitNames = channelName.Split(new char[] { ':' });
//            string senderName = splitNames[0];

//            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
//            {
//                Debug.Log($"{sender}: {message}");
//                OnRoomInvite?.Invoke(sender, message.ToString());
//            }
//        }
//    }

//    public void OnSubscribed(string[] channels, bool[] results)
//    {
//        Debug.Log($"Photon Chat OnSubscribed");
//        for (int i = 0; i < channels.Length; i++)
//        {
//            Debug.Log($"{channels[i]}");
//        }
//    }

//    public void OnUnsubscribed(string[] channels)
//    {
//        Debug.Log($"Photon Chat OnUnsubscribed");
//        for (int i = 0; i < channels.Length; i++)
//        {
//            Debug.Log($"{channels[i]}");
//        }
//    }

//    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
//    {
//        Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
//        PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
//        Debug.Log($"Status Update for {user} and its now {status}.");
//        OnStatusUpdated?.Invoke(newStatus);
//    }

//    public void OnUserSubscribed(string channel, string user)
//    {
//        Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
//    }

//    public void OnUserUnsubscribed(string channel, string user)
//    {
//        Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
//    }
//    #endregion
//}
using UnityEngine;
using System;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;

using Photon.Chat.Demo;
using Photon.Realtime;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    [SerializeField] private string nickName;
    private ChatClient chatClient;
    private bool isConnected = false; // สถานะการเชื่อมต่อ

    public static Action<string, string> OnRoomInvite = delegate { };
    public static Action<ChatClient> OnChatConnected = delegate { };
    public static Action<PhotonStatus> OnStatusUpdated = delegate { };

    #region Unity Methods

    private void Awake()
    {
        nickName = PlayerPrefs.GetString("Username");
        UIFriend.OnInviteFriend += HandleFriendInvite;
        
    }

    private void OnDestroy()
    {
        UIFriend.OnInviteFriend -= HandleFriendInvite;
    }

    private void Start()
    {
        chatClient = new ChatClient(this);
        ConnectToPhotonChat();
    }

    private void Update()
    {
        chatClient.Service();
    }

    #endregion

    #region Private Methods

    private void ConnectToPhotonChat()
    {
        Debug.Log("Connecting to Photon Chat");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nickName);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(nickName));
    }

    private IEnumerator WaitForRoomCreationAndInvite(string recipient)
    {
        while (!isConnected)
        {
            yield return null;
        }

        if (!PhotonNetwork.InRoom)
        {
            
            string RoomName = $"{UnityEngine.Random.Range(100000,999999)}";
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            // ไม่อยู่ในห้อง ให้สร้างห้องใหม่
            PhotonNetwork.CreateRoom(RoomName, roomOptions, TypedLobby.Default);
            yield return new WaitUntil(() => PhotonNetwork.InRoom);
        }

        // ส่งคำเชิญเมื่อสร้างห้องหรืออยู่ในห้องแล้ว
        Debug.Log($"Sending invite to {recipient} for room {PhotonNetwork.CurrentRoom.Name}");
        chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
        yield return new WaitForSeconds(3f);
        ScreenController.LoadScreen("InGame");
    }



    #endregion

    #region Public Methods

    public void HandleFriendInvite(string recipient)
    {
        StartCoroutine(WaitForRoomCreationAndInvite(recipient));
    }

    #endregion

    #region Photon Chat Callbacks

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"Photon Chat DebugReturn: {message}");
    }

    public void OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat");
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        isConnected = false;
    }

    public void OnConnected()
    {
        Debug.Log("You have connected to the Photon Chat");
        OnChatConnected?.Invoke(chatClient);
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        isConnected = true; // อัพเดตสถานะการเชื่อมต่อ
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log($"Photon Chat OnGetMessages {channelName}");
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log($"{senders[i]} messaged: {messages[i]}");
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            // Channel Name format [Sender : Recipient]
            string[] splitNames = channelName.Split(new char[] { ':' });
            string senderName = splitNames[0];

            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{sender}: {message}");
                OnRoomInvite?.Invoke(sender, message.ToString());
            }
        }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Photon Chat OnSubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log($"Photon Chat OnUnsubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}");
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
        PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
        Debug.Log($"Status Update for {user} and its now {status}.");
        OnStatusUpdated?.Invoke(newStatus);
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
    }
    #endregion
}
