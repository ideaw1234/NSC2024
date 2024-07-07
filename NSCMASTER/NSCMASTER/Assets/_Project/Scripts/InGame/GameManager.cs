using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        [Tooltip("The prefab to use for representing the player 1 (Dog)")]
        [SerializeField]
        private GameObject player1Prefab;

        [Tooltip("The prefab to use for representing the player 2 (Cat)")]
        [SerializeField]
        private GameObject player2Prefab;

        [Tooltip("The prefab to use for representing the player 3 (Deer)")]
        [SerializeField]
        private GameObject player3Prefab;

        [Tooltip("The prefab to use for representing the player 4 (Bear)")]
        [SerializeField]
        private GameObject player4Prefab;

        private GameObject instance;

        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Login");
                return;
            }

            if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                InstantiatePlayer();
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }

        public override void OnJoinedRoom()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                InstantiatePlayer();
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("PunBasics-Launcher");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        private void InstantiatePlayer()
        {
            GameObject prefabToInstantiate = GetPlayerPrefab();
            if (prefabToInstantiate != null)
            {
                PhotonNetwork.Instantiate(prefabToInstantiate.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogError("No player prefab found for this player index.");
            }
        }

        private GameObject GetPlayerPrefab()
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber is 1-based, so subtract 1
            switch (playerIndex)
            {
                case 0: return player1Prefab; // Player 1 - Dog
                case 1: return player2Prefab; // Player 2 - Cat
                case 2: return player3Prefab; // Player 3 - Deer
                case 3: return player4Prefab; // Player 4 - Bear
                default: return null; // No prefab for other players
            }
        }

        private void LoadArena()
        {
            //if (!PhotonNetwork.IsMasterClient)
            //{
            //    Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            //    return;
            //}

            //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("PunBasics-Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("Loading Ingame scene");
        }
    }
}
