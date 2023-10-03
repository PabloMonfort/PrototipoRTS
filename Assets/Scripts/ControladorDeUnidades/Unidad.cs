using UnityEngine;
using UnityEngine.AI;

public class Unidad : MonoBehaviour
{
    public string tipoDeUnidad = "Soldier";

    public int tiempoDeRotacion = 0;
    public int id = 0;

    public float radio = 1.13f;
    public float ultimaDireccionX;
    public float velocidad = 5.0f;

    public NavMeshAgent navegadorIA;

    public GameObject cajaDeSeleccion;

    private void Start()
    {
        ControladorDeUnidades.instance.AgregarUnidadAlEquipo(this);
    }
    public void EnviarUnidadAPosicion(Vector3 positionToSend)
    {
        navegadorIA.SetDestination(positionToSend);
    }
}
