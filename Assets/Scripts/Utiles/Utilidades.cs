using UnityEngine;

public static class Utilidades
{
    public static Vector3 ObtenerPosicionDelRaycast()
    {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitDetectado;
        if (Physics.Raycast(rayo, out hitDetectado, 1500f, ControladorDeUnidades.instance.mascaraParaDetectarSuelo))
        {
            if (hitDetectado.transform.gameObject.tag == "Suelo")
            {
                return hitDetectado.point;
            }
        }
        return Vector3.zero;
    }
}
