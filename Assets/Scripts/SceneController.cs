using UnityEngine;
using TMPro;

public class SceneController : MonoBehaviour
{
    //temp variables
    private Ship _sourceShip;

    //layout
    private const int _gridRows = 2;
    private const int _gridCols = 4;
    private const float _offsetX = 2f;
    private const float _offsetY = 2.5f;

    //GUI
    [SerializeField] private TMP_Dropdown _actionSelector;
    [SerializeField] private CanvasGroup _actionSelectorGroup;
    [SerializeField] private TMPro.TMP_Text _nextStepText;
    [SerializeField] private TMPro.TMP_Text _messageText;

    //original instances
    [SerializeField] private Ship _originalShip;
    [SerializeField] private Fleet _originalFleet;

    [SerializeField] private Fleet _playerFleet;
    [SerializeField] private Fleet _opponentFleet;

    //properties
    public Fleet originalFleet
    {
        get { return _originalFleet; }
    }
    public Ship originalShip
    {
        get { return _originalShip; }
    }

    //methods
    // Start is called before the first frame update
    void Start()
    {
        _playerFleet = _originalFleet.LoadFleet(1);
        _opponentFleet = _originalFleet.LoadFleet(2);
        Ship ship;

        HideActionSelector();

        Vector3 startPos = _originalShip.transform.position;
        for (int i = 0; i < _gridCols; i++)
        {
            for (int j = 0; j < _gridRows; j++)
            {
                if (j == 0)
                {
                    ship = _playerFleet.GetShip(i);
                }
                else
                {
                    ship = _opponentFleet.GetShip(i);
                }

                float posX = (_offsetX * i) + startPos.x;
                float posY = -(_offsetY * j) + startPos.y;
                ship.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
        _originalShip.gameObject.SetActive ( false);
    }
    public void SetAction(Ship ship)
    {
        if (ship.player == 1)
        {
            //ship.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.2f);
            ship.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
            ShowActionSelector(ship);
            _sourceShip = ship;
            _nextStepText.text = "Select Action...";
        }
        else
        {
            if (_sourceShip != null && _sourceShip.ActionNeedsTarget())
            {
                _sourceShip.target = ship;
                HideActionSelector();
                _sourceShip = null;
                _nextStepText.text = "Select Ship...";
            }
        }

    }
    public void ShowActionSelector(Ship ship)
    {
        _actionSelector.ClearOptions();
        _actionSelector.AddOptions(ship.GetActions());
        _actionSelectorGroup.alpha = 1f;
        _actionSelectorGroup.blocksRaycasts = true;
        _actionSelector.Show();
    }
    public void HideActionSelector()
    {
        _actionSelectorGroup.alpha = 0f;
        _actionSelectorGroup.blocksRaycasts = false;
    }
    public void EndTurn()
    {
        //ready to end turn?
        bool showMsg = false;
        string message = "";
        for (int i = 0; i < _playerFleet.fleetSize; i++)
        {
            if (_playerFleet.GetShip(i).action == "")
            {
                showMsg = true;
                message = "Not all ships are doing something, are you sure?";
            }
        }
        _messageText.text = message;
        //process turn
        SetEnemyActionsRandom();
        ProcessTurn(_playerFleet);
        ProcessTurn(_opponentFleet);
        CleanUpShips();
        //_messageText.text = "";

        string gameOverMsg = CheckForGameOver();
        if (gameOverMsg.Length > 0) {
            _messageText.text = gameOverMsg;
        }
        
        string s = "";
    }
    private void SetEnemyActionsRandom()
    {
        for (int i = 0; i < _opponentFleet.fleetSize; i++)
        {
            Ship activeShip = _opponentFleet.GetShip(i);
            int actionNumber = Random.Range(1, 3);
            activeShip.setAction(actionNumber);
            if (activeShip.ActionNeedsTarget())
            {
                int targetNumber = Random.Range(0, _playerFleet.fleetSize);
                activeShip.target = _playerFleet.GetShip(targetNumber);
            }
        }
    }
    private void CleanUpShips()
    {
        CleanUpShipsInFleet(_playerFleet);
        CleanUpShipsInFleet(_opponentFleet);
    }
    private void CleanUpShipsInFleet(Fleet activeFleet)
    {
        for (int i = activeFleet.fleetSize-1;i>=0; i--)
        {
            Ship activeShip = activeFleet.GetShip(i);
            if (activeShip.integrity < 1)
            {
                activeShip.ShipDestroyed();
                activeFleet.RemoveShip(activeShip);
            }
            else {
                activeShip.setAction(0);
                activeShip.target = null;
            }
        }
    }
    private void ProcessTurn(Fleet activeFleet)
    {
        for (int i = 0; i < activeFleet.fleetSize; i++)
        {
            Ship activeShip = activeFleet.GetShip(i);
            if (activeShip.action == "Attack")
            {
                activeShip.target.integrity -= activeShip.firePower;
            }
            else if (activeShip.action == "Repair")
            {
                activeShip.integrity += Random.Range(1, 3);
            }
            else if (activeShip.action == "Defend")
            {

            }
        }
    }
    private string CheckForGameOver() {
        string gameOverMsg = "";
        if(_playerFleet.fleetSize == 0 && _opponentFleet.fleetSize == 0) {
            gameOverMsg = "Mutual Destruction";
        }
        else if (_playerFleet.fleetSize == 0) {
            gameOverMsg = "You Lose";
        }
        else if (_opponentFleet.fleetSize == 0) {
            gameOverMsg = "You Win";
        }
        return gameOverMsg;
    }

    //events
    public void ActionSelector_IndexChanged(int index)
    {
        if (index != 0)
        {
            if(_sourceShip is null) {
                string s = "s";
            }
            _sourceShip.setAction(index);
            if (!_sourceShip.ActionNeedsTarget()) {
                HideActionSelector();
                _nextStepText.text = "Select Ship...";
            }
            else {
                _nextStepText.text = "Select Target...";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
