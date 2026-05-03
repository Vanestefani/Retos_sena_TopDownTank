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
    public float turretLagSpeed = 90;
    private Vector3 finalTurretLookDir;

    public Transform reticleTransform;
    public AudioSource audioMotor;
    public float AUD_Quieto = 0.9f;   
    public float AUD_Maximo = 1.6f;   
    public float AUD_Suavizado = 3f;
    [Header("Dash Settings")]
    public float dashForce = 50f;  
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1.5f;  
    private float nextDashTime = 0f;
    private float currentSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = tankSpeed;
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
            ActualizarSonidoMotor();
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) && Time.time >= nextDashTime)
            {
                ExecuteDash();
            }
        }
        
    }
    private void ExecuteDash()
    {
           nextDashTime = Time.time + dashCooldown;
               Vector3 dashDirection = transform.forward;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

        Debug.Log("Dash ejecutado!");
    }
    protected virtual void HandleMovement()
    {
        // Move Tank
        Vector3 wantedPosition = transform.position + (transform.forward * input.ForwardInput * currentSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        // Rotate Tank
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * tankRotationSpeed * input.RotationInput * Time.deltaTime);
        rb.MoveRotation(wantedRotation);
  
    }

    protected virtual void HandleTurret()
    {
        if (turretTransform)
        {
          
            Vector3 turretLookDir = input.reticlePosition - turretTransform.position;
            turretLookDir.y = 0f;

            if (turretLookDir != Vector3.zero)
            {
       
                Quaternion finalTurretLookDir = Quaternion.LookRotation(turretLookDir);
                turretTransform.rotation = Quaternion.RotateTowards(
                    turretTransform.rotation,
                    finalTurretLookDir,
                    turretLagSpeed * Time.deltaTime
                );
            }
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
    protected void ActualizarSonidoMotor()
    {
        if (audioMotor == null) return;
        float intensidadInput = Mathf.Max(Mathf.Abs(input.ForwardInput), Mathf.Abs(input.RotationInput));
        float pitchObjetivo = Mathf.Lerp(AUD_Quieto, AUD_Maximo, intensidadInput);
        audioMotor.pitch = Mathf.Lerp(audioMotor.pitch, pitchObjetivo, Time.deltaTime * AUD_Suavizado);
    }
    public void SetSpeedMultiplier(float multiplier)
    {
        currentSpeed = tankSpeed * multiplier;
    }
}

