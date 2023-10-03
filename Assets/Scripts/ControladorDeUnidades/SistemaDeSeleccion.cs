using UnityEngine;

public class SistemaDeSeleccion : MonoBehaviour
{
    public Camera Camara;

    private Vector2 _posicionInicioArrastre;
    private Vector2 _posicionFinArrastre;

    private Rect _rectanguloSeleccion;
    private Rect _rectanguloRelleno;
    private Rect _rectanguloVacio = new Rect(0, 0, 0, 0);


    public LayerMask mascaraSoloSuelo;
    public LayerMask mascaraSoloUnidades;

    private bool _estaArrastrando = false;

    public Texture TexturaSeleccion;

    public void Start()
    {
        _rectanguloSeleccion = new Rect();
        _rectanguloRelleno = new Rect();
    }
    public void Update()
    {
        // Comprobar la pulsación, liberación y mantenimiento del botón del mouse
        if (Input.GetMouseButtonDown(0))
        {
            BotonIzquierdoPresionado(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            BotonIzquierdoLiberado(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            if (SistemaDeConstruccion.instance.cursorSobreInterfaz == true)
            {
                return;
            }
            ArrastreBotonIzquierdo(Input.mousePosition);
        }
    }

    void OnGUI()
    {
        if (_estaArrastrando)
        {
            ObtenerRectanguloEnPantalla();
            ObtenerRectanguloRelleno();
            GUI.DrawTexture(_rectanguloRelleno, TexturaSeleccion, ScaleMode.StretchToFill, true);
        }
        else
        {
            _rectanguloSeleccion = _rectanguloVacio;
        }
    }

    private void ArrastreBotonIzquierdo(Vector2 posicionMouse)
    {
        if (posicionMouse != _posicionInicioArrastre)
        {
            _estaArrastrando = true;
            _posicionFinArrastre = posicionMouse;

            for (int i = 0; i < ControladorDeUnidades.instance.unidades.Count; i++)
            {
                Vector2 puntoEnPantallaActual = Camera.main.WorldToScreenPoint(ControladorDeUnidades.instance.unidades[i].transform.position);

                if (_rectanguloSeleccion.Contains(puntoEnPantallaActual))
                {
                    if (!ControladorDeUnidades.instance.unidadesSeleccionadas.Contains(ControladorDeUnidades.instance.unidades[i]))
                    {
                        ControladorDeUnidades.instance.unidadesSeleccionadas.Add(ControladorDeUnidades.instance.unidades[i]);
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift) == false)
                    {
                        if (ControladorDeUnidades.instance.unidadesSeleccionadas.Contains(ControladorDeUnidades.instance.unidades[i]))
                        {
                            ControladorDeUnidades.instance.unidadesSeleccionadas.Remove(ControladorDeUnidades.instance.unidades[i]);
                        }
                    }
                }
            }
        }
    }

    private void BotonIzquierdoLiberado(Vector2 posicionMouse)
    {
        _estaArrastrando = false;
        _posicionFinArrastre = posicionMouse;
    }

    private void BotonIzquierdoPresionado(Vector2 posicionMouse)
    {
        _posicionInicioArrastre = posicionMouse;
    }

    /// <summary>
    /// Crea el rectángulo de selección, normalizando los puntos para que
    /// los valores sean siempre positivos.
    /// </summary>
    private void ObtenerRectanguloRelleno()
    {
        // Para empezar, asumimos que el usuario está arrastrando hacia abajo y hacia la derecha,
        // ya que esto siempre dará como resultado un ancho y alto positivos.
        float x = _posicionInicioArrastre.x;
        float y = _posicionInicioArrastre.y;
        float ancho = _posicionFinArrastre.x - _posicionInicioArrastre.x;
        float alto = (Screen.height - _posicionFinArrastre.y) - (Screen.height - _posicionInicioArrastre.y);

        // Si el ancho es negativo (el usuario está arrastrando hacia la izquierda), intercambia la posición x y hace que el ancho sea positivo.
        if (ancho < 0)
        {
            x = _posicionFinArrastre.x;
            ancho = Mathf.Abs(ancho);
        }

        // Si la altura es negativa (el usuario está arrastrando hacia arriba), intercambia la posición y hace que la altura sea positiva.
        if (alto < 0)
        {
            y = _posicionFinArrastre.y;
            alto = Mathf.Abs(alto);
        }

        // Establece el rectángulo en función de los valores.
        _rectanguloRelleno.x = x;
        _rectanguloRelleno.y = Screen.height - y;
        _rectanguloRelleno.width = ancho;
        _rectanguloRelleno.height = alto;
    }

    private void ObtenerRectanguloEnPantalla()
    {
        float x = _posicionInicioArrastre.x;
        float y = _posicionInicioArrastre.y;
        float ancho = _posicionFinArrastre.x - _posicionInicioArrastre.x;
        float alto = _posicionFinArrastre.y - _posicionInicioArrastre.y;

        if (ancho < 0)
        {
            x = _posicionFinArrastre.x;
            ancho *= -1;
        }

        if (alto < 0)
        {
            y = _posicionFinArrastre.y;
            alto *= -1;
        }

        _rectanguloSeleccion.x = x;
        _rectanguloSeleccion.y = y;
        _rectanguloSeleccion.width = ancho;
        _rectanguloSeleccion.height = alto;
    }
}