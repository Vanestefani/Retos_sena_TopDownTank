using UnityEngine;

public class AGUA : MonoBehaviour
{
    public float multiplicador = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        TankController tank = other.GetComponent<TankController>();
        if (tank != null)
        {
            tank.SetSpeedMultiplier(multiplicador);
            Debug.Log("Entrando en zona: Velocidad reducida");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank != null)
        {
            tank.SetSpeedMultiplier(1f); 
            Debug.Log("Saliendo de zona: Velocidad normal");
        }
    }
}
