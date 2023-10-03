using System.Collections.Generic;
using UnityEngine;

public class CustomRect
{
    public float x;
    public float y;
    public float width;
    public float height;
}
public class ControladorDeUnidades : MonoBehaviour
{
    public static ControladorDeUnidades instance;

    public List<Unidad> unidades = new List<Unidad>();
    public List<Unidad> unidadesSeleccionadas = new List<Unidad>();
    private List<Unidad> unidadesSeleccionadasCache = new List<Unidad>();

    public Vector2 _arrastradoComienzo;
    public Vector2 _arrastradoFinal;

    Vector3 total;
    Vector3 centro;

    public CustomRect _selectionRect;

    public bool arrastrando = false;

    public LayerMask mascaraParaDetectarSuelo;

    private void Awake()
    {
        instance = this;
    }
    //
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(1) == true) //si soltamos el boton derecho del mouse
        {
            Vector3 posicionEnElMundo = Utilidades.ObtenerPosicionDelRaycast(); //obtenemos la posicion usando la libreria nuestra de Utils.
            MoverUnidadesSeleccionadas(posicionEnElMundo); //<--- enviamos las unidades seleccionadas a esta posicion
        }
    }
    public void DeseleccionarTodo()
    {
        unidadesSeleccionadas.Clear();
    }
    private void MoverUnidadesSeleccionadas(Vector3 posicionEnElMundo)
    {
        if (unidadesSeleccionadas != unidadesSeleccionadasCache)
        {
            unidadesSeleccionadasCache = unidadesSeleccionadas;
        }
        total = new Vector3(0,0,0);
        centro = new Vector3(0, 0, 0);
        if (unidadesSeleccionadasCache.Count > 1)
        {
            for (int i = 0; i < unidadesSeleccionadasCache.Count; i++)
            {
                total += unidadesSeleccionadasCache[i].gameObject.transform.position;
                centro = total / unidadesSeleccionadasCache.Count;
            }
        }
        if (unidadesSeleccionadasCache.Count == 0)
        {
            return;
        }
        if (unidadesSeleccionadasCache.Count == 1)
        {
            //al ser solo una unidad la mandamos como viene.
            unidadesSeleccionadasCache[0].EnviarUnidadAPosicion(posicionEnElMundo);
        }
        if (unidadesSeleccionadasCache.Count > 1)
        {
            // esto es para mover las unidades en grupo manteniendo la posicion al momento de seleccionarlas
            for (int i = 0; i < unidadesSeleccionadasCache.Count; i++)
            {
                Vector3 vectorOrigen = unidadesSeleccionadasCache[i].gameObject.transform.position - centro;
                Vector3 vectorFinal = posicionEnElMundo + vectorOrigen;

                unidadesSeleccionadasCache[i].EnviarUnidadAPosicion(vectorFinal);
            }
        }
    }

    public void AgregarUnidadAlEquipo(Unidad p)
    {
        if (p != null)
        {
            if (!unidades.Contains(p))// si no existe en la lista 
            {
                unidades.Add(p); // le agregamos a la lista
                p.id = unidades.IndexOf(p); // le seteamos el id al script de referencia
            }
        }
    }
    public List<Unidad> ObtenerUnidades()
    {
        return unidades;
    }
}
