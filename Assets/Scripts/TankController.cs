using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public ParticleSystem Particulas_Exhosto; 
    public float Exhosto_Quieto = 10f;      
    public float Exhosto_Movimiento = 100f;
    private ParticleSystem.EmissionModule moduloEmision;

    public float tankSpeed = 15f;
    public float tankRotationSpeed = 20f;

    private Rigidbody rb;
    private Tank_Inputs input;

    public Transform turretTransform;
    public float turretLagSpeed = 6;
    private Vector3 finalTurretLookDir;

    public Transform reticleTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        input = GetComponent<Tank_Inputs>();
        if (Particulas_Exhosto != null)
        {
            moduloEmision = Particulas_Exhosto.emission;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb && input)
        {
            HandleMovement();
            HandleReticle();
            HandleTurret();
            Exhosto_Cambiar_Num_Particu ();
        }
        
    }

    protected virtual void HandleMovement()
    {
        // Move Tank
        Vector3 wantedPosition = transform.position + (transform.forward * input.ForwardInput * tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        // Rotate Tank
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * tankRotationSpeed * input.RotationInput * Time.deltaTime);
        rb.MoveRotation(wantedRotation);

    }

    protected virtual void HandleTurret()
    {
        if(turretTransform)
        {
            Vector3 turretLookDir = input.reticlePosition - turretTransform.position;

            turretLookDir.y = 0f;

            finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretLagSpeed);
            turretTransform.rotation = Quaternion.LookRotation(finalTurretLookDir);
        }
    }
    protected virtual void HandleReticle()
    {
        reticleTransform.position = input.reticlePosition;
    }
    protected void Exhosto_Cambiar_Num_Particu()
    {
        if (Particulas_Exhosto == null) return;
        bool estaMoviendose = Mathf.Abs(input.ForwardInput) > 0.1f || Mathf.Abs(input.RotationInput) > 0.1f;
        moduloEmision.rateOverTime = estaMoviendose ? Exhosto_Movimiento : Exhosto_Quieto;
    }
}
