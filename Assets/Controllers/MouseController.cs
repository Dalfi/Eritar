using UnityEngine;
using System.Collections;

public enum MouseMode
{
  Normal,
  BuildingPlacement
}

public class MouseController : MonoBehaviour
{
  public MouseMode CurrentMode { get; set; }
  private GameObject buildingPlacementTemplate { get; set; }

  //destination point
  private Vector3 endPoint;

  // Update is called once per frame
  void Update()
  {
    if (CurrentMode == MouseMode.BuildingPlacement)
    {
      //declare a variable of RaycastHit struct
      RaycastHit hit;
      Ray ray;

      //Create a Ray on mouse position
      ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit))
      {
        //save the click position
        endPoint = hit.point;

        //as we do not want to change the y axis value based on the input position, reset it to original y axis value
        endPoint.y = buildingPlacementTemplate.transform.position.y;

        buildingPlacementTemplate.transform.position = endPoint;
      }
    }

    if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
      LeftMouseClick();
    else if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
      RightMouseClick();
  }

  private void LeftMouseClick()
  {
    if(CurrentMode == MouseMode.BuildingPlacement)
    {
      //buildingPlacementTemplate.name

      CurrentMode = MouseMode.Normal;
    }
  }

  private void RightMouseClick()
  {

  }


  public void BeginBuildingPlacement(GameObject building)
  {
    if (buildingPlacementTemplate != null)
      Destroy(buildingPlacementTemplate);

    CurrentMode = MouseMode.BuildingPlacement;

    Vector3 m = Input.mousePosition;
    m = new Vector3(m.x, m.y, Camera.main.transform.position.y);
    Vector3 p = Camera.main.ScreenToWorldPoint(m);

    buildingPlacementTemplate = (GameObject)Instantiate(building, new Vector3(p.x, 0, p.z),  building.transform.rotation);
  }

  
}
