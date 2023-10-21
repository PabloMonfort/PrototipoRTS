using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
[System.Serializable]
public enum TiposDeConstruccion
{
    Ayuntamiento = 0,
    Almacén = 1,
}
[System.Serializable]
public class TareaDeConstruccion
{
    public TiposDeConstruccion tipoDeConstruccion;
    public MarcaDeConstruccion marcaDeConstruccion;
}
public class SistemaDeConstruccion : MonoBehaviour
{
    public static SistemaDeConstruccion instance;
    public VistaPreviaDeConstruccion vistaPreviaDeConstruccionActual;
    public List<VistaPreviaDeConstruccion> construccionesParaVistaPrevia;
    public List<GameObject> marcasDeConstruccion;

    public List<TareaDeConstruccion> tareasDeConstruccion;

    public Material celdaEnSuelo;
    public Material celdaFueraDeSuelo;
    private Terrain _terrenoObjetivo;
    public float alturaSobre = 0f;
    public float alturaFinal = 0f;
    public float sizeDeCelda = 0.5f;
    public LayerMask mascaraDeCapas;
    public TMP_Text textoDeConstruccionSeleccionada;
    public TiposDeConstruccion tipoDeConstruccionAConstruir;
    public RectTransform seleccionada;
    public bool modoEliminarActivado = false;
    public bool enPanelDeConstruccion = false;
    public bool cursorSobreInterfaz = false;
    public GameObject objetoARemoverSiEsPosible = null;
    public NavMeshSurface superficie;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (enPanelDeConstruccion == false || cursorSobreInterfaz == true)
            return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var golpe, float.MaxValue, mascaraDeCapas))
        {
            if (golpe.transform.TryGetComponent(out Terrain terreno)) _terrenoObjetivo = terreno;

            Vector3 puntoDeGolpe = golpe.point;
            // Redondea las coordenadas del punto de golpe al múltiplo más cercano de tamañoDeCelda
            puntoDeGolpe.x = Mathf.Round(puntoDeGolpe.x / sizeDeCelda) * sizeDeCelda;
            puntoDeGolpe.z = Mathf.Round(puntoDeGolpe.z / sizeDeCelda) * sizeDeCelda;

            // Actualiza la posición de la vista previa de edificio con el punto de golpe redondeado
            vistaPreviaDeConstruccionActual.transform.position = puntoDeGolpe;

            alturaSobre = vistaPreviaDeConstruccionActual.transform.position.y;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (alturaSobre >= 0.6f && alturaSobre < 1.1f)
            {
                alturaFinal = 0.001f;
            }
            else if (alturaSobre > 1.1f)
            {
                alturaFinal = 0.002f;
            }
            else if (alturaSobre < 0.6f)
            {
                alturaFinal = 0.000f;
            }
            else
            {
                alturaFinal = 0.000f;
            }
            if (modoEliminarActivado == false)
            {
                if (vistaPreviaDeConstruccionActual.puedeConstruir == true)
                    Construir();
            }
            else
            {
                Destruir();
            }
            //NivelarTerreno(puntoDeGolpe, alturaFinal, 20, 10);
        }
    }
    private void Construir()
    {
        GameObject nuevaMarca = Instantiate(marcasDeConstruccion[(int)tipoDeConstruccionAConstruir], vistaPreviaDeConstruccionActual.transform.position, vistaPreviaDeConstruccionActual.transform.rotation);
        nuevaMarca.SetActive(true);
        MarcaDeConstruccion marcaDeConstruccion = nuevaMarca.GetComponent<MarcaDeConstruccion>();
        TareaDeConstruccion tareaNueva = new TareaDeConstruccion()
        {
            tipoDeConstruccion = tipoDeConstruccionAConstruir,
            marcaDeConstruccion = marcaDeConstruccion
        };
        tareasDeConstruccion.Add(tareaNueva);
    }
    private void Destruir()
    {
        if (objetoARemoverSiEsPosible == null) return;

        Debug.Log(string.Format("Intento de destrucción del objeto del juego. NOMBRE: {0}", objetoARemoverSiEsPosible.name));

        if (objetoARemoverSiEsPosible.CompareTag("MarcaDeEdificio"))
            Destroy(objetoARemoverSiEsPosible);
        else
            Debug.Log("No es una marca de construcción, así que debemos mostrar el panel para destruir este construcción o no");
    }

    public void ClicEnBotonDeConstruccion(RectTransform numeroDelBoton)
    {
        if (numeroDelBoton.GetSiblingIndex() > 7 || numeroDelBoton.GetSiblingIndex() < 0) { Debug.Log("Tipo de construcción incorrecto"); return; }
        modoEliminarActivado = false;
        vistaPreviaDeConstruccionActual = construccionesParaVistaPrevia[numeroDelBoton.GetSiblingIndex()];
        for (int i = 0; i < construccionesParaVistaPrevia.Count; i++)
        {
            construccionesParaVistaPrevia[i].gameObject.SetActive(false);
        }
        construccionesParaVistaPrevia[numeroDelBoton.GetSiblingIndex()].gameObject.SetActive(true);
        seleccionada.SetParent(numeroDelBoton);
        seleccionada.anchoredPosition = new Vector2(0, 0);
        TiposDeConstruccion tipoDeConstruccion = (TiposDeConstruccion)numeroDelBoton.GetSiblingIndex();
        tipoDeConstruccionAConstruir = tipoDeConstruccion;
        textoDeConstruccionSeleccionada.text = string.Format("Construir {0}", tipoDeConstruccion.ToString());
        Debug.Log(tipoDeConstruccion + " Seleccionado");
    }
    public void ClicEnBotonDeDestruccion(RectTransform numeroDelBoton)
    {
        modoEliminarActivado = true;
        for (int i = 0; i < construccionesParaVistaPrevia.Count; i++)
        {
            construccionesParaVistaPrevia[i].gameObject.SetActive(false);
        }
        construccionesParaVistaPrevia[0].gameObject.SetActive(true);
        seleccionada.SetParent(numeroDelBoton);
        seleccionada.anchoredPosition = new Vector2(0, 0);
        TiposDeConstruccion tipoDeEdificio = 0;
        tipoDeConstruccionAConstruir = tipoDeEdificio;
        textoDeConstruccionSeleccionada.text = "Demoler";

        Debug.Log(tipoDeEdificio + " Seleccionado");
    }
    public void DetenerLogicaDeConstruccion()
    {
        for (int i = 0; i < construccionesParaVistaPrevia.Count; i++)
        {
            construccionesParaVistaPrevia[i].gameObject.SetActive(false);
        }
        enPanelDeConstruccion = false;
    }
    public void IniciarLogicaDeConstruccion()
    {
        construccionesParaVistaPrevia[(int)tipoDeConstruccionAConstruir].gameObject.SetActive(true);
        enPanelDeConstruccion = true;
    }
    public void CursorSobre(bool sobre)
    {
        cursorSobreInterfaz = sobre;
    }
    public void HornearSuperficie()
    {
        superficie.BuildNavMesh();
    }
    public void NivelarTerreno(Vector3 posicionEnElMundo, float altura, int anchoDelCepillo, int altoDelCepillo)
    {
        var posicionDelCepillo = ObtenerPosicionDelCepillo(posicionEnElMundo, anchoDelCepillo, altoDelCepillo);

        var sizeDelCepillo = ObtenerSizeSeguroDelCepillo(posicionDelCepillo.x, posicionDelCepillo.y, anchoDelCepillo, altoDelCepillo);

        var datosDelTerreno = ObtenerDatosDelTerreno();

        var alturas = datosDelTerreno.GetHeights(posicionDelCepillo.x, posicionDelCepillo.y, sizeDelCepillo.x, sizeDelCepillo.y);

        for (var y = 0; y < sizeDelCepillo.y; y++)
        {
            for (var x = 0; x < sizeDelCepillo.x; x++)
            {
                alturas[y, x] = altura;
            }
        }

        datosDelTerreno.SetHeights(posicionDelCepillo.x, posicionDelCepillo.y, alturas);
    }
    private TerrainData ObtenerDatosDelTerreno() => _terrenoObjetivo.terrainData;

    private int ObtenerResolucionDelMapaDeAlturas() => ObtenerDatosDelTerreno().heightmapResolution;

    private Vector3 ObtenerTamañoDelTerreno() => ObtenerDatosDelTerreno().size;

    public Vector3 MundoAPosicionDelTerreno(Vector3 posicionEnElMundo)
    {
        var posicionDelTerreno = posicionEnElMundo - _terrenoObjetivo.GetPosition();

        var sizeDelTerreno = ObtenerTamañoDelTerreno();

        var resolucionDelMapaDeAlturas = ObtenerResolucionDelMapaDeAlturas();

        posicionDelTerreno = new Vector3(posicionDelTerreno.x / sizeDelTerreno.x, posicionDelTerreno.y / sizeDelTerreno.y, posicionDelTerreno.z / sizeDelTerreno.z);

        return new Vector3(posicionDelTerreno.x * resolucionDelMapaDeAlturas, 0, posicionDelTerreno.z * resolucionDelMapaDeAlturas);
    }
    public Vector2Int ObtenerPosicionDelCepillo(Vector3 posicionEnElMundo, int anchoDelCepillo, int altoDelCepillo)
    {
        var posicionDelTerreno = MundoAPosicionDelTerreno(posicionEnElMundo);

        var resolucionDelMapaDeAlturas = ObtenerResolucionDelMapaDeAlturas();

        return new Vector2Int((int)Mathf.Clamp(posicionDelTerreno.x - anchoDelCepillo / 2.0f, 0.0f, resolucionDelMapaDeAlturas), (int)Mathf.Clamp(posicionDelTerreno.z - altoDelCepillo / 2.0f, 0.0f, resolucionDelMapaDeAlturas));
    }
    public Vector2Int ObtenerSizeSeguroDelCepillo(int cepilloX, int cepilloY, int anchoDelCepillo, int altoDelCepillo)
    {
        var resoluciónDelMapaDeAlturas = ObtenerResolucionDelMapaDeAlturas();

        while (resoluciónDelMapaDeAlturas - (cepilloX + anchoDelCepillo) < 0) anchoDelCepillo--;

        while (resoluciónDelMapaDeAlturas - (cepilloY + altoDelCepillo) < 0) altoDelCepillo--;

        return new Vector2Int(anchoDelCepillo, altoDelCepillo);
    }
    public float MuestrearAlturaPromedio(Vector3 posicionEnElMundo, int anchoDelCepillo, int altoDelCepillo)
    {
        var posicionDelCepillo = ObtenerPosicionDelCepillo(posicionEnElMundo, anchoDelCepillo, altoDelCepillo);

        var sizeDelCepillo = ObtenerSizeSeguroDelCepillo(posicionDelCepillo.x, posicionDelCepillo.y, anchoDelCepillo, altoDelCepillo);

        var alturas2D = ObtenerDatosDelTerreno().GetHeights(posicionDelCepillo.x, posicionDelCepillo.y, sizeDelCepillo.x, sizeDelCepillo.y);

        var alturas = new float[alturas2D.Length];

        var i = 0;

        for (int y = 0; y <= alturas2D.GetUpperBound(0); y++)
        {
            for (int x = 0; x <= alturas2D.GetUpperBound(1); x++)
            {
                alturas[i++] = alturas2D[y, x];
            }
        }

        return alturas.Average();
    }
}