using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet :MonoBehaviour
{
    [SerializeField] private SceneController _controller;
    [SerializeField] private int _playerNumber;
    [SerializeField] private List<Ship> _ships = new List<Ship>();

    //constructor (from template)
    public Fleet LoadFleet(int playerNumber)
    {
        Ship originalShip = _controller.originalShip ;
        Fleet originalFleet = _controller.originalFleet;
        Ship ship;
        Fleet fleet = Instantiate(originalFleet) as Fleet;
        fleet.playerNumber = playerNumber;
        
        ship = Instantiate(originalShip) as Ship;
        ship.SetShip( playerNumber, 0);
        fleet.AddShip(ship);
        ship = Instantiate(originalShip) as Ship;
        ship.SetShip( playerNumber, 1);
        fleet.AddShip(ship);
        ship = Instantiate(originalShip) as Ship;
        ship.SetShip( playerNumber, 2);
        fleet.AddShip(ship);
        ship = Instantiate(originalShip) as Ship;
        ship.SetShip( playerNumber, 3);
        fleet.AddShip(ship);
        return fleet;
    }

    //properties
    public int playerNumber
    {
        get { return _playerNumber; }
        set { _playerNumber = value; }
    }
    public int fleetSize
    {
        get { return _ships.Count; }
    }

    //methods
    public void AddShip(Ship ship)
    {
        _ships.Add(ship);
    }
    public Ship GetShip(int id)
    {
        return _ships[id];
    }
    public void RemoveShip(Ship ship)
    {
        _ships.Remove(ship);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
