using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCam;

    private Vector3 dragOrigin;
    [SerializeField] private float dragSpeed = 0.5f;

    private float maxZoom = 50f;
    private float minZoom = 5f;
    private float currentZoomLevel;

    private void Awake()
    {
        mainCam = Camera.main;
        currentZoomLevel = mainCam.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics.Raycast(clickPos, Vector3.down,out var newRaycastHit, 3);
            if (hit == false) return;
            var selectedCell = newRaycastHit.transform.gameObject.GetComponent<Cell>();
            selectedCell.OnCellSelected();
        }
    }

    private void RecalculateDragSpeed()
    {
        dragSpeed = 0.2215f * mainCam.orthographicSize - 0.005f;
    }

    private void LateUpdate()
    {
        if (mainCam.orthographicSize < 2)
        {
            mainCam.orthographicSize = 2;
            RecalculateDragSpeed();
            return;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            currentZoomLevel -= Input.mouseScrollDelta.y;
            currentZoomLevel = Mathf.Clamp(currentZoomLevel, minZoom, maxZoom);
            mainCam.orthographicSize = currentZoomLevel;
            RecalculateDragSpeed();
            
        }
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(2)) return;
        var delta = dragOrigin - Input.mousePosition;
        transform.Translate(delta * (Time.fixedDeltaTime * dragSpeed));
        dragOrigin = Input.mousePosition;
    }
    
    public void ReturnToOrigin()
    {
        mainCam.transform.position = new Vector3(GameManager.Instance._currentBoardSizeX/2f, mainCam.transform.position.y, GameManager.Instance._currentBoardSizeY/2f);
    }

}
