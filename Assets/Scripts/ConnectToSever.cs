using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ConnectToSever : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Loading;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Loading.SetActive(false);
        Menu.SetActive(true);
    }
}
