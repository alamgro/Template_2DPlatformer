using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Posici�n de la c�mara respecto al jugador.")]
    [SerializeField] private Vector3 cameraOffset;

    private Transform player;
    private Vector3 camFinalPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void LateUpdate()
    {
        //Add the offset based on the player's position
        camFinalPosition = player.position + cameraOffset; 
        //Set camera new position
        transform.position = new Vector3(camFinalPosition.x, cameraOffset.y, camFinalPosition.z);
    }
}
