using System;
using TMPro;
using Photon.Realtime;
using UnityEngine;

public class UIFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    [SerializeField] private FriendInfo friend;

    public static Action<string> OnRemoveFriend = delegate { };
    public static Action<string> OnInviteFriend = delegate { };

    public void Initialize(FriendInfo friend)
    {
        this.friend = friend;
        Debug.Log($"Initializing UI for friend: {this.friend.UserId}");
        friendNameText.SetText(this.friend.UserId);
    }
    public void RemoveFriend()
    {
        Debug.Log($"Removing friend: {friend.UserId}");
        OnRemoveFriend?.Invoke(friend.UserId);
    }
    public void InviteFriend()
    {
        Debug.Log($"Inviting friend: {friend.UserId}");
        OnInviteFriend?.Invoke(friend.UserId);
    }
}