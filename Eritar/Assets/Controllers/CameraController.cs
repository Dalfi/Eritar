using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
  public static CameraController Instance { get; protected set; }

  private Camera cameraInstance;

  public Camera CameraInstance
  {
    get { return cameraInstance; }
    protected set { cameraInstance = value; }
  }

  // Use this for initialization
  void OnEnable()
  {
    if (Instance != null)
    {
      Debug.LogError("There should never be two camera controllers.");
    }

    Instance = this;
  }

  // Use this for initialization
  void Start()
  {
    CameraInstance = Camera.main;
  }

  // Update is called once per frame
  void Update()
  {
    CameraInstance = Camera.main;

    MoveCamera();
  }

  private void MoveCamera()
  {
    float xpos = Input.mousePosition.x;
    float ypos = Input.mousePosition.y;
    Vector3 movement = Vector3.zero;
    bool isMouseOverGUI = EventSystem.current.IsPointerOverGameObject();

    //horizontal camera movement
    if (Input.GetKey(KeyCode.A) || (GlobalResourceManager.BorderScrollEnabled && !isMouseOverGUI && xpos >= 0 && xpos < GlobalResourceManager.BorderScrollOffset))
    {
      movement.x -= GlobalResourceManager.ScrollSpeed;
    }
    else if (Input.GetKey(KeyCode.D) || (GlobalResourceManager.BorderScrollEnabled && !isMouseOverGUI && xpos <= Screen.width && xpos > Screen.width - GlobalResourceManager.BorderScrollOffset))
    {
      movement.x += GlobalResourceManager.ScrollSpeed;
    }

    //vertical camera movement
    if (Input.GetKey(KeyCode.S) || (GlobalResourceManager.BorderScrollEnabled && ypos >= 0 && !isMouseOverGUI && ypos < GlobalResourceManager.BorderScrollOffset))
    {
      movement.z -= GlobalResourceManager.ScrollSpeed;
    }
    else if (Input.GetKey(KeyCode.W) || (GlobalResourceManager.BorderScrollEnabled && !isMouseOverGUI && ypos <= Screen.height && ypos > Screen.height - GlobalResourceManager.BorderScrollOffset))
    {
      movement.z += GlobalResourceManager.ScrollSpeed;
    }
  }
}
