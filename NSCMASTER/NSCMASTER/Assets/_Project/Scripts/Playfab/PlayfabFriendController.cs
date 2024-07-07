using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlayfabFriendController : MonoBehaviour
{
    public static PlayfabFriendController Instance;
    public static Action<List<FriendInfo>> OnFriendListUpdated = delegate { };
    private List<FriendInfo> friends;

    private void Awake()
    {
        friends = new List<FriendInfo>();
        ProtonConnector.GetPhotonFriends += HandleGetFriends;
        UIAddFriend.OnAddFriend += HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend += HandleRemovePlayfabFriend;
    }
    private void OnDestroy()
    {
        ProtonConnector.GetPhotonFriends -= HandleGetFriends;
        UIAddFriend.OnAddFriend -= HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend -= HandleRemovePlayfabFriend;
    }
    private void HandleAddPlayfabFriend(string name)
    {
        var request = new AddFriendRequest { FriendTitleDisplayName = name };
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFailure);
    }
    private void HandleRemovePlayfabFriend(string name)
    {
        string id = friends.FirstOrDefault(f => f.TitleDisplayName == name).FriendPlayFabId;
        Debug.Log($"Remove friend {name} with id {id}");
        var request = new RemoveFriendRequest { FriendPlayFabId = id };
        PlayFabClientAPI.RemoveFriend(request, OnFriendRemoveSuccess, OnFailure);
    }

    private void HandleGetFriends()
    {
        GetPlayfabFriends();
    }
    private void GetPlayfabFriends()
    {
        var request = new GetFriendsListRequest();
        PlayFabClientAPI.GetFriendsList(request, OnFriendListSuccess, OnFailure);
    }
    private void OnFriendAddedSuccess(AddFriendResult result)
    {
        GetPlayfabFriends();
    }

    //private void OnFriendListSuccess(GetFriendsListResult result)
    //{
    //    friends = result.Friends;
    //    OnFriendListUpdated?.Invoke(result.Friends);
    //}
    public void OnFriendListSuccess(GetFriendsListResult result)
    {
        friends = result.Friends;
        Debug.Log($"Found PlayFab friends: {friends.Count}");
        foreach (var friend in friends)
        {
            Debug.Log($"PlayFab Friend: {friend.TitleDisplayName}");
        }
        OnFriendListUpdated?.Invoke(friends);
    }
    private void OnFriendRemoveSuccess(RemoveFriendResult result)
    {
        Debug.Log("Friend removed and refresh");
        GetPlayfabFriends();
    }
    private void OnFailure(PlayFabError error)
    {
        Debug.LogError($"Error Playfab friend: {error.GenerateErrorReport()}");
    }
}