using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Management : MonoBehaviour
{
    [SerializeField] public GameObject Join;
    [SerializeField] public GameObject Lobby;
    public void OnBGClicked()
    {
        Debug.Log("Background clicked");
        Join.SetActive(false);
        Lobby.SetActive(true);
    }
}
