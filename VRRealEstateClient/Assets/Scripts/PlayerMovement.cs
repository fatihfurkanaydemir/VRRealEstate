using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private Animator animator;
  [SerializeField] Camera cam;
  [SerializeField] private PhotonView pv;
  [SerializeField] private GameObject model;
  [Range(0.1f, 5f)][SerializeField] float speed = 3f;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    pv = GetComponent<PhotonView>();

    if (!pv.IsMine)
    {
      Destroy(cam.gameObject);
      SetLayerOnAll(model, 0);
      model.tag = "Untagged";
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Cursor.visible = !Cursor.visible;
      Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    if (!pv.IsMine) return;

    transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

    float xMovement = Input.GetAxisRaw("Horizontal");
    float zMovement = Input.GetAxisRaw("Vertical");
    float zMovementSmooth = Input.GetAxisRaw("Vertical");
    float xMovementSmooth = Input.GetAxisRaw("Horizontal");

    var movementVec = new Vector3(xMovement, 0, zMovement);
    movementVec = movementVec.normalized * speed * Time.deltaTime;
    movementVec = transform.TransformDirection(movementVec);

    transform.position += movementVec;
    animator.SetFloat("speed", zMovementSmooth * speed);
  }

  static void SetLayerOnAll(GameObject obj, int layer)
  {
    foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
    {
      trans.gameObject.layer = layer;
    }
  }
}
