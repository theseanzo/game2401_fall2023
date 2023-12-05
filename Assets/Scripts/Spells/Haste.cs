using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Haste : Spell
{
    [SerializeField] private float _radius = 50f;
    [SerializeField] private float _boostMuliplier = 2f;
    [SerializeField] private float _duration = 3f;
        private List<Unit> _unitList = new List<Unit>();
    private ParticleSystem _particleSystem;
    private bool _canCast = true;



    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        transform.localScale = new Vector3(_radius, _radius, _radius);
    }
    public override void Cast()
    {
        if(_canCast == true)
        {
            Unit[] units = FindObjectsOfType<Unit>();
            foreach (Unit unit in units)
            {
                _unitList.Add(unit);
            }
            Debug.Log(_unitList.Count);
            _particleSystem.Play();
            StartCoroutine("SpellEffect");
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            _canCast = false; //prevents player from casting spell multiple times
        }
        
    }
    public override void Effect()
    {
        base.Effect();
    }

    IEnumerator SpellEffect()
    {
        foreach (Unit unit in _unitList)
        {
            unit.moveSpeed = unit.moveSpeed*_boostMuliplier;
        }
        yield return new WaitForSeconds(_duration);
        foreach (Unit unit in _unitList)
        {
            unit.moveSpeed = unit.moveSpeed / _boostMuliplier;
        }
        _unitList.Clear();
        Destroy(gameObject);
    }

   

}
