using Eritar.Framework;
using UnityEngine;

namespace Eritar
{
  public class WorldController : MonoBehaviour
  {
    public static WorldController Instance { get; protected set; }

    public World world { get; protected set; }


    // Use this for initialization
    void OnEnable()
    {
      if (Instance != null)
      {
        Debug.LogError("There should never be two world controllers.");
      }

      Instance = this;

      CreateEmptyWorld();
    }

    // Update is called once per frame
    void Update()
    {
      world.Update(Time.deltaTime);
    }

    /// <summary>
    /// ONLY FOR TESTING!
    /// </summary>
    void CreateEmptyWorld()
    {
      Debug.Log("Creating new empty world");
      // Create a world with Empty tiles
      world = new World(256, 256, 6);

      BoxCollider col = this.gameObject.AddComponent<BoxCollider>();
      col.size = new Vector3(world.GetCorrectWidth(), 1, world.GetCorrectHeight());
      col.center = new Vector3(world.GetCorrectWidth() / 2, 0, world.GetCorrectHeight() / 2);
      col.tag = "Ground";

      // Center the Camera
      Camera.main.transform.position = new Vector3(world.Width / 2, Camera.main.transform.position.y, world.Height / 2);
    }
  }
}