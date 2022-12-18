using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Networking;
using System;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
 using UnityEngine.Android;
#endif
using Agora.Rtc;

public class VoiceChatManager : MonoBehaviour
{
  private string _appID = "88ba9239783a4d82853c748fe3b8111e";
  private string _channelName = "vrrealestatee";
  // private string _token = "007eJxTYDh4vDhf2NnB6ZiM2fngwDYbu1dmWpfuH7m67CInj+yv8oUKDBYWSYmWRsaW5hbGiSYpFkYWpsbJ5iYWaanGSRaGhoap/7NnJzcEMjIcrfjKyMgAgSA+D0NZUVFqYk5qcUliSSoDAwAxJiMD";
  private string serverUrl = "https://agora-token-service-production-7f57.up.railway.app"; // The base URL to your token server. For example, https://agora-token-service-production-92ff.up.railway.app"
  private int ExpireTime = 5 * 24 * 60 * 60; //Expire time in Seconds.
  private IRtcEngine RtcEngine;
  public static VoiceChatManager Instance;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Microphone };
#endif

  void Awake()
  {
    if (Instance) Destroy(this);
    else
    {
      Instance = this;
      SetupVoiceSDKEngine();
      DontDestroyOnLoad(Instance);
    }
  }
  void Start()
  {
    InitEventHandler();
  }

  void Update()
  {
    CheckPermissions();
  }

  private void CheckPermissions()
  {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    foreach (string permission in permissionList)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }
#endif
  }

  private void SetupVoiceSDKEngine()
  {
    RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
    RtcEngineContext context = new RtcEngineContext(_appID, 0,
    CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
    AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
    RtcEngine.Initialize(context);
    RtcEngine.EnableSoundPositionIndication(true);
  }

  private void InitEventHandler()
  {
    UserEventHandler handler = new UserEventHandler(this);
    RtcEngine.InitEventHandler(handler);
  }

  internal class UserEventHandler : IRtcEngineEventHandler
  {
    private readonly VoiceChatManager _audioSample;

    internal UserEventHandler(VoiceChatManager audioSample)
    {
      _audioSample = audioSample;
    }

    public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
    {
      Debug.Log("Joined channel:" + connection.channelId);
      Hashtable hash = new Hashtable();
      hash.Add("agoraID", connection.localUid.ToString());
      PhotonNetwork.SetPlayerCustomProperties(hash);
    }

    public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
    {
      Debug.Log("Left channel with duration: " + stats.duration);
    }

    public override void OnError(int err, string msg)
    {
      Debug.LogError("AGORA_ERROR: " + msg + " : " + err);
    }

  }


  IEnumerator FetchToken(string url, string channel, string userId, int TimeToLive, Action<string> callback = null)
  {
    UnityWebRequest request = UnityWebRequest.Get(
    string.Format("{0}/rtc/{1}/publisher/userAccount/{2}/?expiry={3}", url, channel, userId, TimeToLive)
    );
    yield return request.SendWebRequest();
    if (request.isNetworkError || request.isHttpError)
    {
      Debug.Log(request.error);
      callback(null);
      yield break;
    }
    TokenStruct tokenInfo = JsonUtility.FromJson<TokenStruct>(request.downloadHandler.text);
    callback(tokenInfo.rtcToken);
  }

  public void Join(string uid)
  {
    StartCoroutine(FetchToken(serverUrl, _channelName, uid, ExpireTime, this.FetchRenew));
  }

  void FetchRenew(string newToken)
  {
    RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    RtcEngine.EnableAudio();
    RtcEngine.JoinChannelWithUserAccount(newToken, _channelName, PhotonNetwork.LocalPlayer.UserId);
  }

  public void Leave()
  {
    if (RtcEngine != null)
    {
      RtcEngine.LeaveChannel();
      RtcEngine.DisableAudio();
    }
  }

  public IRtcEngine GetRtcEngine()
  {
    return RtcEngine;
  }

  void OnApplicationQuit()
  {
    if (RtcEngine != null)
    {
      Leave();
      RtcEngine.Dispose();
      RtcEngine = null;
    }
  }
}
