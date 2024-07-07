using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class UIDisplayFriends : MonoBehaviour
{
    [SerializeField] private Transform friendContainer;
    [SerializeField] private UIFriend uifriendPrefab;
    private void Awake()
    {
        PhotonFriendController.OnDisPlayFriends += HandleDisplayFriends;
    }
    private void OnDestroy()
    {
        PhotonFriendController.OnDisPlayFriends -= HandleDisplayFriends;
    }

    private void HandleDisplayFriends(List<FriendInfo> friends)
    {
        Debug.Log("UI remove prior friends displayed");
        foreach (Transform child in friendContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log($"UI instantiate friends display {friends.Count}");
        foreach (FriendInfo friend in friends)
        {
            UIFriend uIFriend = Instantiate(uifriendPrefab, friendContainer);
            uIFriend.Initialize(friend);
            uIFriend.gameObject.SetActive(true);
        }
    }
}
