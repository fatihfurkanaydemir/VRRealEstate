using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Agora.Rtc;
using System.Linq;

public class SpatialAudio : MonoBehaviour
{
  [SerializeField] float radius = 5;
  static Dictionary<Player, SpatialAudio> spatialAudioFromPlayers = new Dictionary<Player, SpatialAudio>();
  PhotonView pv;
  void Awake()
  {
    pv = GetComponent<PhotonView>();
    spatialAudioFromPlayers[pv.Owner] = this;
  }

  // Update is called once per frame
  void Update()
  {
    if (!pv.IsMine) return;

    foreach (Player player in PhotonNetwork.PlayerList)
    {
      if (player.IsLocal) continue;
      if (player.CustomProperties.TryGetValue("agoraID", out object agoraID))
      {
        if (spatialAudioFromPlayers.ContainsKey(player))
        {
          SpatialAudio other = spatialAudioFromPlayers[player];

          float gain = GetGain(other.transform.position);
          float pan = GetPan(other.transform.position);

          VoiceChatManager.Instance.GetRtcEngine().SetRemoteVoicePosition(uint.Parse((string)agoraID), pan, gain);
        }
        else
        {
          VoiceChatManager.Instance.GetRtcEngine().SetRemoteVoicePosition(uint.Parse((string)agoraID), 0, 0);
        }
      }
    }
  }

  void OnDestroy()
  {
    foreach (var item in spatialAudioFromPlayers.Where(x => x.Value == this).ToList())
    {
      spatialAudioFromPlayers.Remove(item.Key);
    }
  }

  float GetGain(Vector3 otherPosition)
  {
    float distance = Vector3.Distance(transform.position, otherPosition);
    float gain = Mathf.Max(1 - (distance / radius), 0) * 100f;
    return gain;
  }

  float GetPan(Vector3 otherPosition)
  {
    Vector3 direction = otherPosition - transform.position;
    direction.Normalize();
    float dotProduct = Vector3.Dot(transform.right, direction);
    return dotProduct;
  }
}
