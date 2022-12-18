using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.IO;

public class ServerManagement : MonoBehaviourPunCallbacks
{
  // Start is called before the first frame update7
  private string _SelectedAvatar = "real_estate_male";
  private string _RoomNumber = "";
  AssetBundle _bundle;
  [SerializeField] TMPro.TMP_InputField roomInput;
  [SerializeField] TMPro.TMP_Text errorText;
  [SerializeField] Canvas mainCanvas;
  [SerializeField] Canvas gameCanvas;
  void Start()
  {
  }

  public void JoinRoom()
  {
    if (roomInput.text.Trim() == "")
    {
      errorText.text = "Please enter room number!";
      return;
    }

    _RoomNumber = roomInput.text.Trim();
    StartCoroutine(GetRoomByRoomNumber(_RoomNumber, RoomExists));
  }

  public void RoomExists(RoomDTO room)
  {
    if (room == null)
    {
      errorText.text = "Room with specified number does not exist";
      return;
    }

    errorText.text = "Loading property...";
    StartCoroutine(LoadAssetAzureRoutine(room.assetLink, PropertyLoaded));
  }

  public void PropertyLoaded(AssetBundle bundle)
  {
    if (bundle == null)
    {
      Debug.LogError("Bundle not loaded");
      return;
    }

    var prefab = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
    Instantiate(prefab);

    PhotonNetwork.ConnectUsingSettings();
  }

  public override void OnConnectedToMaster()
  {
    Debug.Log("Connected to the server");
    PhotonNetwork.JoinLobby();
  }

  public override void OnJoinedLobby()
  {
    Debug.Log("Connected to the lobby");
    PhotonNetwork.JoinOrCreateRoom(_RoomNumber, new RoomOptions { MaxPlayers = 5, IsOpen = true, IsVisible = true }, TypedLobby.Default);
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("Connected to the Room");
    var spawnPoint = GameObject.FindWithTag("SpawnPoint").transform.position;
    GameObject player = PhotonNetwork.Instantiate(_SelectedAvatar, spawnPoint, Quaternion.identity);

    VoiceChatManager.Instance.Join(PhotonNetwork.LocalPlayer.UserId);

    mainCanvas.gameObject.SetActive(false);
    gameCanvas.gameObject.SetActive(true);
  }

  public void SelectMaleRealEstate() => _SelectedAvatar = "real_estate_male";
  public void SelectFemaleRealEstate() => _SelectedAvatar = "real_estate_female";
  public void SelectMale() => _SelectedAvatar = "male";
  public void SelectFemale() => _SelectedAvatar = "female";

  public override void OnLeftRoom()
  {
    VoiceChatManager.Instance.Leave();
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

  IEnumerator GetRoomByRoomNumber(string roomNumber, Action<RoomDTO> callback = null)
  {
    Debug.Log("Checking for: " + roomNumber);
    UnityWebRequest request = UnityWebRequest.Get($"https://vrrealestateapi.azurewebsites.net/api/Room/GetByRoomNumber/{roomNumber}");
    request.SetRequestHeader("accept", "*/*");
    yield return request.SendWebRequest();
    if (request.isNetworkError || request.isHttpError)
    {
      Debug.Log(request.error);
      callback(null);
      yield break;
    }

    var roomResponse = JsonConvert.DeserializeObject<RoomDTOResponse>(request.downloadHandler.text);

    if (!roomResponse.succeeded)
      callback(null);
    else
      callback(roomResponse.data);
  }

  public IEnumerator LoadAssetAzureRoutine(string url, Action<AssetBundle> callback = null)
  {
    // UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
    // yield return www.SendWebRequest();

    // if (www.result != UnityWebRequest.Result.Success)
    // {
    //   Debug.Log(www.error);
    //   callback(null);
    // }
    // else
    // {
    //   callback(DownloadHandlerAssetBundle.GetContent(www));
    // }
    var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine("Assets/AssetBundles/WindowsAssetBundles/house2"));
    yield return null;

    if (myLoadedAssetBundle == null)
    {
      Debug.Log("Failed to load AssetBundle!");
      callback(null);
    }
    //set the public Asset bundle variable 
    else
    {
      callback(myLoadedAssetBundle);
    }
  }
}
