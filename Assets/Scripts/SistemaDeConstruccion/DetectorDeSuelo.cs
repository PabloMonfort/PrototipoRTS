using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.TextCore.Text;

public class DetectorDeSuelo : MonoBehaviour
{
    public float distanciaRayoVerificadorDeConstruicciones = 10.002f; // Ajusta la distancia según sea necesario.
    public float distanciaRayoVerificadorDeTerreno = 0.002f; // Ajusta la distancia según sea necesario.
    public bool estaBloqueandoConstruccion = false;
    public bool estaCercaDelTerreno = false;
    public LayerMask máscaraDeCapas;
    public bool DetectorDeDestrucción = false;

    void Update()
    {
        // Llama al método de raycast cuando sea necesario, por ejemplo, en la función Update().
        estaCercaDelTerreno = VerificarProximidadAlTerreno();

        if (!estaCercaDelTerreno) return;

        estaBloqueandoConstruccion = VerificarProximidadConstrucción();
    }

    bool VerificarProximidadAlTerreno()
    {
        // Crea una variable de hit de raycast para almacenar la información del impacto.
        RaycastHit impacto;

        // Crea un raycast desde la posición actual, moviéndose hacia abajo.
        Vector3 origenRayo = transform.position;
        Vector3 direcciónRayo = Vector3.down;

        // Realiza el raycast.
        if (Physics.Raycast(origenRayo, direcciónRayo, out impacto, distanciaRayoVerificadorDeTerreno, máscaraDeCapas))
        {
            // Comprueba si el colisionador impactado es un colisionador de terreno.
            if (impacto.collider.CompareTag("Suelo"))
            {
                // También puedes comprobar la distancia si es necesario.
                // float distanciaAlTerreno = impacto.distance;
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaEnSuelo;
                // Visualiza el rayo con fines de depuración.
                Debug.DrawRay(origenRayo, direcciónRayo * distanciaRayoVerificadorDeTerreno, Color.green);

                // Devuelve true si está cerca del terreno.
                return true;
            }
            else
            {
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
                Debug.DrawRay(origenRayo, direcciónRayo * distanciaRayoVerificadorDeTerreno, Color.red);
                return false;
            }
        }
        GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
        // Visualiza el rayo incluso si no se golpea ningún terreno.
        Debug.DrawRay(origenRayo, direcciónRayo * distanciaRayoVerificadorDeTerreno, Color.red);

        // Devuelve false si no está cerca del terreno.
        return false;
    }

    bool VerificarProximidadConstrucción()
    {
        // Crea una variable de hit de raycast para almacenar la información del impacto.
        RaycastHit impacto;

        // Crea un raycast desde la posición actual, moviéndose hacia abajo.
        Vector3 origenRayo = transform.position;
        origenRayo.y += 10;
        Vector3 direcciónRayo = Vector3.down;

        // Realiza el raycast.
        if (Physics.Raycast(origenRayo, direcciónRayo, out impacto, distanciaRayoVerificadorDeConstruicciones, máscaraDeCapas))
        {
            // Comprueba si el colisionador impactado es un colisionador de construcción.
            if (impacto.collider.CompareTag("Construcciones"))
            {
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
                Debug.DrawRay(origenRayo, direcciónRayo * distanciaRayoVerificadorDeConstruicciones, Color.red);

                if (DetectorDeDestrucción) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = impacto.collider.gameObject;

                return true;
            }
            else
            {
                if (DetectorDeDestrucción) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
            }
        }
        else
        {
            if (DetectorDeDestrucción) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
        }
        if (DetectorDeDestrucción) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
        GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaEnSuelo;
        // Visualiza el rayo con fines de depuración.
        Debug.DrawRay(origenRayo, direcciónRayo * distanciaRayoVerificadorDeConstruicciones, Color.green);

        // Devuelve false si no está cerca de una construcción.
        return false;
    }
}
