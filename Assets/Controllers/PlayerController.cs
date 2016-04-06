using Eritar.Framework;
using UnityEngine;

namespace Eritar
{
  class PlayerController : MonoBehaviour
  {
    private Player _player;

    public Player Player
    {
      get { return _player; }
      private set { _player = value; }
    }

    public bool IsHuman = false;


    // Use this for initialization
    void Start()
    {
      if (IsHuman)
      {
        this.Player = new HumanPlayer();
        this.Player.Start();

        this.gameObject.AddComponent<CameraController>();
      }
      else
      {
        // Make it AI Player (or other Player?)
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
  }
}
