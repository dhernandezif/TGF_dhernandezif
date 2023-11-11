using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MakeSound : MonoBehaviour
{
    public GameObject terrain; // Referencia al GameObject del terreno (cubo)
    public Camera mainCamera;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clic izquierdo
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject == terrain)
                {
                    // Obtiene las coordenadas del terreno donde se hizo clic
                    Vector3 clickPoint = hitInfo.point;
                    clickPoint.y += 1.5f;
             //       GameObject.Find("Enemy_Target").transform.position = clickPoint;
              //      GameObject.Find("AASSA").gameObject.transform.LookAt(clickPoint);
           //         GameObject.Find("AASSA").GetComponent<EnemyAI>().playerHeardAt(clickPoint);//
                    
                    //Debug.Log("Clic en las coordenadas: " + clickPoint);
                }
            }
        }
    }

    /*public GameObject cubePrefab, player; // Prefab del cubo que deseas generar
    private GameObject lastCube;


    private void Update()
    {
        // Busca el dispositivo de la mano izquierda
       InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // Verifica si se encuentra el dispositivo y si el joystick está presionado
        if (leftHandDevice.isValid && leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool joystickPressed) && joystickPressed)
        {
            Debug.Log("anchuuuuuuu");
            GameObject.Find("AASSA").GetComponent<EnemyAI>().playerHeardAt(player.transform.position);
        }
    }*/
}

