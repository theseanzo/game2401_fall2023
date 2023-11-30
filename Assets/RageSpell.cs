using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSpell : MonoBehaviour
{
    private ParticleSystem particleSystem;

    [SerializeField]
    private GameObject unitParticle;

    [SerializeField]
    private float increaseMoveSpeed;
    [SerializeField]
    private int increaseAttackPower;
    [SerializeField]
    private float decreaseeAttackInterval;

    private List<GameObject> particleInChild = new List<GameObject>();

    private List<Unit> affectedUnit = new List<Unit>();

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem == null)
        {
            Debuff();            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Unit>() && !other.gameObject.GetComponentInChildren<ParticleSystem>()) //Buffs units affected by a spells and makes sure it doesnt buff them twice
        {
            affectedUnit.Add(other.gameObject.GetComponent<Unit>());
            particleInChild.Add(Instantiate(unitParticle, other.transform));
            affectedUnit[i].transform.parent = particleInChild[i].transform;
            affectedUnit[i].moveSpeed += increaseMoveSpeed;
            affectedUnit[i].attackPower += increaseAttackPower;
            affectedUnit[i].attackInterval -= decreaseeAttackInterval;
            i++;
        }
    }
    private void Debuff()//Debuff the units affected by the spell
    {
        if (affectedUnit != null)
        {
            for (int i = 0; i < affectedUnit.Count; i++)
            {
                Destroy(particleInChild[i]);
                affectedUnit[i].moveSpeed -= increaseMoveSpeed;
                affectedUnit[i].attackPower -= increaseAttackPower;
                affectedUnit[i].attackInterval += decreaseeAttackInterval;
            }
        }

    }
}
