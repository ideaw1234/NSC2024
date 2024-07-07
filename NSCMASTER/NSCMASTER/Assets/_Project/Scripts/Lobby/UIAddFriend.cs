using System;
using UnityEngine;
using TMPro;

public class UIAddFriend : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] public GameObject Invite;
    [SerializeField] public GameObject Friend;
    [SerializeField] public GameObject Friend_Scroll;
    [SerializeField] public GameObject Invite_Scroll;

    public static Action<string> OnAddFriend = delegate { };

    public void SetAddFrinedName(string name)
    {
        displayName = name;
    }

    public void AddFriend()
    {
        if (string.IsNullOrEmpty(displayName))
        {
            Debug.LogError("Display name is empty");
            return;
        }
        OnAddFriend?.Invoke(displayName);
    }

    public void Invite_Button()
    {
        Invite.SetActive(false);
        Friend_Scroll.SetActive(false);
        Invite_Scroll.SetActive(true);
        Friend.SetActive(true);
    }
    public void Friend_Button()
    {
        Invite.SetActive(true);
        Invite_Scroll.SetActive(false);
        Friend_Scroll.SetActive(true);
        Friend.SetActive(false);
    }
}
