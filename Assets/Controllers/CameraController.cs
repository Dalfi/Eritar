using Eritar.Framework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Eritar
{
  public class CameraController : MonoBehaviour
  {
    // Update is called once per frame
    public void Update()
    {
      MoveCamera();
      RotateCamera();
    }

    private void RotateCamera()
    {
      Vector3 origin = Camera.main.transform.eulerAngles;
      Vector3 destination = origin;

      //detect rotation amount if ALT is being held and the Right mouse button is down
      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
      {
        destination.x -= Input.GetAxis("Mouse Y") * GlobalResourceManager.RotateAmount;
        destination.y += Input.GetAxis("Mouse X") * GlobalResourceManager.RotateAmount;
      }
      else
      {
        if (Input.GetKey(KeyCode.Q))
        {
          destination.y -= GlobalResourceManager.RotateAmount;
        }
        else if (Input.GetKey(KeyCode.E))
        {
          destination.y += GlobalResourceManager.RotateAmount;
        }

        if (Input.GetKey(KeyCode.PageUp))
        {
          destination.x -= GlobalResourceManager.RotateAmount;
        }
        else if (Input.GetKey(KeyCode.PageDown))
        {
          destination.x += GlobalResourceManager.RotateAmount;
        }
      }

      destination.x = Mathf.Clamp(destination.x, 0f, 180f);

      //if a change in position is detected perform the necessary update
      if (destination != origin)
      {
        Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * GlobalResourceManager.RotateSpeed);
      }
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

      //make sure movement is in the direction the camera is pointing
      //but ignore the vertical tilt of the camera to get sensible scrolling
      movement = Camera.main.transform.TransformDirection(movement);
      movement.y = 0;

      // handle camera scrolling
      movement.y -= GlobalResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

      //calculate desired camera position based on received input
      Vector3 origin = Camera.main.transform.position;
      Vector3 destination = origin;
      destination.x += movement.x;
      destination.y += movement.y;
      destination.z += movement.z;

      //limit away from ground movement to be between a minimum and maximum distance
      destination.y = Mathf.Clamp(destination.y, GlobalResourceManager.MinCameraHeight, GlobalResourceManager.MaxCameraHeight);

      //if a change in position is detected perform the necessary update
      if (destination != origin)
      {
        Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * GlobalResourceManager.ScrollSpeed);
      }
    }
  }
}