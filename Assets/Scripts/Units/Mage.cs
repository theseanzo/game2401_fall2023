using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit
{
    [SerializeField]
    Transform magicStartPos;

    [SerializeField]
    float bigSpellCooldown = 3f;

    private float magicSpellDespawnTimer = 3f;

    private bool isCooldownActive = false;

    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        Debug.Log("Mage OnAttackActionEvent");

        if (attackTarget != null)
        {
            PoolObject magicSpell = PoolManager.Instance.Spawn("MagicSpell");

            if (magicSpell != null)
            {
                Debug.Log("Spawning magic spell");
                magicSpell.transform.position = magicStartPos.position;
                magicSpell.transform.rotation = magicStartPos.rotation;
                magicSpell.GetComponent<Projectile>().Init(attackTarget, attackPower);

                // Start the despawn timer for the magic spell
                StartCoroutine(DespawnMagicSpell(magicSpell));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldownActive)
        {
            StartCoroutine(SpawnBigSpell());
        }
    }

    IEnumerator SpawnBigSpell()
    {
        // Set the flag to prevent multiple coroutines
        isCooldownActive = true;

        yield return new WaitForSeconds(bigSpellCooldown);

        // Code to spawn a bigger stronger spell
        PoolObject bigMagicSpell = PoolManager.Instance.Spawn("BigMagicSpell");

        if (bigMagicSpell != null)
        {
            Debug.Log("Spawning big magic spell");
            bigMagicSpell.transform.position = magicStartPos.position;
            bigMagicSpell.transform.rotation = magicStartPos.rotation;
            bigMagicSpell.GetComponent<Projectile>().Init(attackTarget, attackPower * 2);

            // Start the despawn timer for the big magic spell
            StartCoroutine(DespawnMagicSpell(bigMagicSpell));
        }

        // Reset the flag after spawning the big spell
        isCooldownActive = true;
    }

    IEnumerator DespawnMagicSpell(PoolObject magicSpell)
    {
        yield return new WaitForSeconds(magicSpellDespawnTimer);
        magicSpell.OnDeSpawn();
    }
}
