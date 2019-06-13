using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ship : MonoBehaviour
{
    [SerializeField] private SceneController _controller;
    [SerializeField] private int _integrity;
    [SerializeField] private int _firePower;
    [SerializeField] private string _action;
    [SerializeField] private Ship _target;
    [SerializeField] private int _player = 0;
    [SerializeField] private Sprite[] _images;
    [SerializeField] private Sprite _explosion;
    private Animator _anim;
    private TextMesh _shipIntegrityTextBox;
    private TextMesh _firePowerTextBox;

    //properties
    public int player
    {
        get { return _player; }
    }
    public string action
    {
        get { return _action; }
    }
    public Ship target
    {
        get { return _target; }
        set { _target = value; }
    }
    public int integrity
    {
        get { return _integrity; }
        set { _integrity = value; }
    }
    public int firePower
    {
        get { return _firePower; }
        set { _firePower = value; }
    }
    public void SetShip( int player,int image)
    {
        _player = player;
        _integrity = (Random.Range(1, 10));
        _firePower = (Random.Range(1, 5));
        GetComponent<SpriteRenderer>().sprite = _images[image];
    }

    //methods
    public List<string> GetActions()
    {
        List<string> actions = new List<string>();
        actions.Add("Select an Action");
        actions.Add("Attack");
        actions.Add("Defend");
        actions.Add("Repair");
        return actions;
    }
    public void setAction(int index)
    {
        _action = this.GetActions()[index];
    }
    public bool ActionNeedsTarget()
    {
        return "AttackDefend".Contains(_action);
    }
    public void ShipDestroyed()
    {
        _anim.SetBool("Destroyed", true);
        StartCoroutine( SetShipInactive());
    }
    IEnumerator SetShipInactive() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }


    //events
    public void OnMouseDown()
    {
        _controller.SetAction(this);
        Debug.Log("id:"  + "integrity:" + _integrity + ";firePower:" + _firePower);
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        
        foreach (TextMesh t in this.GetComponentsInChildren<TextMesh>()) {
            if (t.name == "shipIntegrityText") _shipIntegrityTextBox = t;
            if (t.name == "shipFirePowerText") _firePowerTextBox = t;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _shipIntegrityTextBox.text = this.integrity.ToString();
        _firePowerTextBox.text = this.firePower.ToString();
    }
}
