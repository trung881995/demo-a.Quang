using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField createInput;
    [SerializeField] InputField joinInput;
    private float Player1ispicked;
    public float Player1IsPicked => Player1ispicked;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void CreateRoom()
    {
        Player1ispicked = 1;
        PhotonNetwork.CreateRoom(createInput.text);
        

    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);    
    }
    public override void OnJoinedRoom()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        PhotonNetwork.LoadLevel("Game");
       
    }

}
