using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FPSLook : MonoBehaviour
{
  [Range(0.1f, 9f)][SerializeField] float sensitivityX = 2f;
  [Range(0.1f, 9f)][SerializeField] float sensitivityY = 2f;
  [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;


  public enum RotationAxes
  {
    MouseX = 1,
    MouseY = 2
  };

  [SerializeField] RotationAxes axes = RotationAxes.MouseX;
  private PhotonView pv;
  float verticalRot = 0;

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    pv = GetComponentInParent<PhotonView>();
  }

  void Update()
  {
    if (!pv.IsMine) return;

    if (axes == RotationAxes.MouseX)
    {
      transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
    }
    else if (axes == RotationAxes.MouseY)
    {
      verticalRot -= Input.GetAxis("Mouse Y") * sensitivityY;
      verticalRot = Mathf.Clamp(verticalRot, -yRotationLimit, yRotationLimit);

      float horizontalRot = transform.localEulerAngles.y;

      transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
    }


  }
}
