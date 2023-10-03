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
    public float distanciaRayoVerificadorDeConstruicciones = 10.002f; // Ajusta la distancia seg�n sea necesario.
    public float distanciaRayoVerificadorDeTerreno = 0.002f; // Ajusta la distancia seg�n sea necesario.
    public bool estaBloqueandoConstruccion = false;
    public bool estaCercaDelTerreno = false;
    public LayerMask m�scaraDeCapas;
    public bool DetectorDeDestrucci�n = false;

    void Update()
    {
        // Llama al m�todo de raycast cuando sea necesario, por ejemplo, en la funci�n Update().
        estaCercaDelTerreno = VerificarProximidadAlTerreno();

        if (!estaCercaDelTerreno) return;

        estaBloqueandoConstruccion = VerificarProximidadConstrucci�n();
    }

    bool VerificarProximidadAlTerreno()
    {
        // Crea una variable de hit de raycast para almacenar la informaci�n del impacto.
        RaycastHit impacto;

        // Crea un raycast desde la posici�n actual, movi�ndose hacia abajo.
        Vector3 origenRayo = transform.position;
        Vector3 direcci�nRayo = Vector3.down;

        // Realiza el raycast.
        if (Physics.Raycast(origenRayo, direcci�nRayo, out impacto, distanciaRayoVerificadorDeTerreno, m�scaraDeCapas))
        {
            // Comprueba si el colisionador impactado es un colisionador de terreno.
            if (impacto.collider.CompareTag("Suelo"))
            {
                // Tambi�n puedes comprobar la distancia si es necesario.
                // float distanciaAlTerreno = impacto.distance;
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaEnSuelo;
                // Visualiza el rayo con fines de depuraci�n.
                Debug.DrawRay(origenRayo, direcci�nRayo * distanciaRayoVerificadorDeTerreno, Color.green);

                // Devuelve true si est� cerca del terreno.
                return true;
            }
            else
            {
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
                Debug.DrawRay(origenRayo, direcci�nRayo * distanciaRayoVerificadorDeTerreno, Color.red);
                return false;
            }
        }
        GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
        // Visualiza el rayo incluso si no se golpea ning�n terreno.
        Debug.DrawRay(origenRayo, direcci�nRayo * distanciaRayoVerificadorDeTerreno, Color.red);

        // Devuelve false si no est� cerca del terreno.
        return false;
    }

    bool VerificarProximidadConstrucci�n()
    {
        // Crea una variable de hit de raycast para almacenar la informaci�n del impacto.
        RaycastHit impacto;

        // Crea un raycast desde la posici�n actual, movi�ndose hacia abajo.
        Vector3 origenRayo = transform.position;
        origenRayo.y += 10;
        Vector3 direcci�nRayo = Vector3.down;

        // Realiza el raycast.
        if (Physics.Raycast(origenRayo, direcci�nRayo, out impacto, distanciaRayoVerificadorDeConstruicciones, m�scaraDeCapas))
        {
            // Comprueba si el colisionador impactado es un colisionador de construcci�n.
            if (impacto.collider.CompareTag("Construcciones"))
            {
                GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaFueraDeSuelo;
                Debug.DrawRay(origenRayo, direcci�nRayo * distanciaRayoVerificadorDeConstruicciones, Color.red);

                if (DetectorDeDestrucci�n) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = impacto.collider.gameObject;

                return true;
            }
            else
            {
                if (DetectorDeDestrucci�n) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
            }
        }
        else
        {
            if (DetectorDeDestrucci�n) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
        }
        if (DetectorDeDestrucci�n) SistemaDeConstruccion.instance.objetoARemoverSiEsPosible = null;
        GetComponent<Renderer>().sharedMaterial = SistemaDeConstruccion.instance.celdaEnSuelo;
        // Visualiza el rayo con fines de depuraci�n.
        Debug.DrawRay(origenRayo, direcci�nRayo * distanciaRayoVerificadorDeConstruicciones, Color.green);

        // Devuelve false si no est� cerca de una construcci�n.
        return false;
    }
}
