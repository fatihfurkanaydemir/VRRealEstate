using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ServerManagement : MonoBehaviourPunCallbacks
{
  // Start is called before the first frame update
  void Start()
  {
    //1. connect to server
    //2. connect to lobby
    //3. connect to room 
    PhotonNetwork.ConnectUsingSettings(); // connect to server 
    /*
    PhotonNetwork.JoinRoom("room name"); // connect to room 
    PhotonNetwork.CreateRoom("room name", room_settings); // create room
    PhotonNetwork.JoinOrCreateRoom("room name", room_settings, TypedLobby.Default);
    PhotonNetwork.LeaveRoom();
    PhotonNetwork.LeaveLobby();
     */
  }



  public override void OnConnectedToMaster()
  {
    Debug.Log("Connected to the server");
    PhotonNetwork.JoinLobby(); // connect to lobby
                               //It checks/controls the connection of the server

  }

  public override void OnJoinedLobby()
  {
    Debug.Log("Connected to the lobby");
    PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 5, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    //connect to random room or create 
    //it checks the connection of the Lobby
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("Connected to the Room");
    GameObject player = PhotonNetwork.Instantiate("Character", new Vector3(0, 0.2f, 0), Quaternion.identity);
  }

  public override void OnLeftRoom()
  {
    Debug.Log("Left the Room");
  }

  public override void OnLeftLobby()
  {
    Debug.Log("Left the Lobby");
  }

  public override void OnJoinRoomFailed(short returnCode, string message)
  {
    Debug.Log("Could not join any room");
  }

  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    Debug.Log("Could not join any random room");
  }

  public override void OnCreateRoomFailed(short returnCode, string message)
  {
    Debug.Log("Could not create room");
  }


}
