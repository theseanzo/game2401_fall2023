using System.Collections;
using UnityEngine;

public class Enrager : Unit
{
    public float buffRadius = 5f;
    public float buffDuration = 5f;
    public float buffMovementSpeedIncrease = 1.5f;
    public int buffDamageIncrease = 5;
   

    private bool isBuffing = false;

    public ParticleSystem buffParticles;
    public AudioSource song;

    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();

        // Check if not already buffing
        if (!isBuffing)
        { 
            StartCoroutine(ProvideBuffToNearbyUnits());
        }
    }

    private IEnumerator ProvideBuffToNearbyUnits()
    {
        isBuffing = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRadius);
        Debug.Log("Number of colliders detected: " + colliders.Length);

        foreach (Collider collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null && unit != this)
            {
                unit.ApplyBuff(buffDuration, buffMovementSpeedIncrease, buffDamageIncrease);
                Debug.Log("Buff applied to unit: " + unit.gameObject.name);
            }
        }

        // Activate particle system when buffing
        buffParticles.Play();
        song.Play();

        // Wait for the specified duration before allowing another buff
        yield return new WaitForSeconds(buffDuration);

        // Deactivate particle system when buffing ends
        buffParticles.Stop();
        song.Stop();

        isBuffing = false;
    }
    //if buffing is on play the animation, if buffing is off turn off the animation
    private void Update()
    {

    }
}
