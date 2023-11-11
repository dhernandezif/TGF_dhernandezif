using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    public GameObject OBJECTIU;
    RaycastHit hitInfo;
    private void Update()
    {
        RealizarRaycast(this.gameObject, OBJECTIU);
    }
    public bool RealizarRaycast(GameObject objetoOrigen, GameObject objetoDestino)
    {
        RaycastHit hitInfo; // No es necesario declarar hitInfo aqu�.

        if (objetoOrigen == null || objetoDestino == null)
        {
            Debug.LogError("Objeto de origen o destino nulo.");
            return false;
        }

        Vector3 origen = objetoOrigen.transform.position;
        Vector3 direccion = objetoDestino.transform.position - origen;

        Ray rayo = new Ray(origen, direccion);

        if (Physics.Raycast(rayo, out hitInfo))
        {
            // El raycast golpe� algo
            Debug.Log("Raycast golpe� a " + hitInfo.transform.gameObject.name);
            Debug.DrawLine(rayo.origin, hitInfo.point, Color.green);
            return true;
        }
        else
        {
            // El raycast no golpe� nada
            Debug.Log("Raycast no golpe� nada.");
            Debug.DrawRay(rayo.origin, rayo.direction * 100f, Color.red); // Dibuja el rayo rojo
            return false;
        }
    }

}

