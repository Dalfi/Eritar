using Eritar.Framework.Utilities;
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

  public bool IsMouseOverGUI { get; protected set; }

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
    IsMouseOverGUI = EventSystem.current.IsPointerOverGameObject();
  }

  // Update is called once per frame
  void Update()
  {
    CameraInstance = Camera.main;
    IsMouseOverGUI = EventSystem.current.IsPointerOverGameObject();

    MoveCamera();
  }

  private void MoveCamera()
  {
    float xpos = Input.mousePosition.x;
    float ypos = Input.mousePosition.y;
    Vector3 movement = Vector3.zero;

    //horizontal camera movement
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (GlobalResourceManager.BorderScrollEnabled && !IsMouseOverGUI && xpos >= 0 && xpos < GlobalResourceManager.BorderScrollOffset))
    {
      movement.x -= GlobalResourceManager.ScrollSpeed;
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (GlobalResourceManager.BorderScrollEnabled && !IsMouseOverGUI && xpos <= Screen.width && xpos > Screen.width - GlobalResourceManager.BorderScrollOffset))
    {
      movement.x += GlobalResourceManager.ScrollSpeed;
    }

    //vertical camera movement
    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (GlobalResourceManager.BorderScrollEnabled && ypos >= 0 && !IsMouseOverGUI && ypos < GlobalResourceManager.BorderScrollOffset))
    {
      movement.y -= GlobalResourceManager.ScrollSpeed;
    }
    else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || (GlobalResourceManager.BorderScrollEnabled && !IsMouseOverGUI && ypos <= Screen.height && ypos > Screen.height - GlobalResourceManager.BorderScrollOffset))
    {
      movement.y += GlobalResourceManager.ScrollSpeed;
    }

    //calculate desired camera position based on received input
    Vector3 origin = CameraInstance.transform.position;
    Vector3 destination = origin;
    destination.x += movement.x;
    destination.y += movement.y;
    destination.z += movement.z;

    //if a change in position is detected perform the necessary update
    if (destination != origin)
    {
      CameraInstance.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * GlobalResourceManager.ScrollSpeed);
    }

    CameraInstance.orthographicSize -= GlobalResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
    //limit away from ground movement to be between a minimum and maximum distance
    CameraInstance.orthographicSize = Mathf.Clamp(CameraInstance.orthographicSize, GlobalResourceManager.MinCameraHeight, GlobalResourceManager.MaxCameraHeight);
    
  }
}
