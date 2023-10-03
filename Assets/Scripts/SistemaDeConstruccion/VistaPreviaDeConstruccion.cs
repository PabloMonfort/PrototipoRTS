using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VistaPreviaDeConstruccion : MonoBehaviour
{
    [SerializeField] List<DetectorDeSuelo> detectoresDeSueloDeCelda = new List<DetectorDeSuelo>();
    public Renderer renderizadorDeMalla;
    public int cantidadDeCeldasBuenas = 0;
    public bool puedeConstruir = false;

    private void Update()
    {
        cantidadDeCeldasBuenas = 0;
        for (int i = 0; i < detectoresDeSueloDeCelda.Count; i++)
        {
            if (detectoresDeSueloDeCelda[i].estaCercaDelTerreno == true && detectoresDeSueloDeCelda[i].estaBloqueandoConstruccion == false)
            {
                cantidadDeCeldasBuenas++;
            }
        }
        if (cantidadDeCeldasBuenas == detectoresDeSueloDeCelda.Count)
        {
            Material[] materiales = new Material[2];
            materiales[0] = SistemaDeConstruccion.instance.celdaEnSuelo;
            materiales[1] = SistemaDeConstruccion.instance.celdaEnSuelo;
            renderizadorDeMalla.sharedMaterials = materiales;
            puedeConstruir = true;
        }
        else
        {
            Material[] materiales = new Material[2];
            materiales[0] = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
            materiales[1] = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
            renderizadorDeMalla.sharedMaterials = materiales;
            puedeConstruir = false;
        }
    }
}
