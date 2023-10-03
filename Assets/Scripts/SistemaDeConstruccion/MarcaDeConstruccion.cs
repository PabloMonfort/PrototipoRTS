using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcaDeConstruccion : MonoBehaviour
{
    public List<Transform> lugaresDeConstruccion;
    public List<Unidad> constructoresEnSitio;

    public int cantidadMaximaDeConstructores = 1;

    public int cantidadDePiedraNecesaria = 0;
    public int cantidadDeMaderaNecesaria = 0;

    public int cantidadDePiedraEnSitio = 0;
    public int cantidadDeMaderaEnSitio = 0;

    public bool esperandoMateriales = false;

    private void Start()
    {
        cantidadMaximaDeConstructores = lugaresDeConstruccion.Count;
    }
    public void AgregarPiedraAlSitio(Unidad ayudanteUnit)
    {
        if (cantidadDePiedraEnSitio >= cantidadDePiedraNecesaria) return;

        cantidadDePiedraEnSitio++;
        //ayudanteUnit.RemoverMaterialDeCarga();
    }
    public void AgregarMaderaAlSitio(Unidad ayudanteUnit)
    {
        if (cantidadDeMaderaEnSitio >= cantidadDeMaderaNecesaria) return;

        cantidadDeMaderaEnSitio++;

        //ayudanteUnit.RemoverMaterialDeCarga();
    }
}
