using UnityEngine;

public class AspectRatio : MonoBehaviour
{
   [SerializeField] private float minAspectRatio = 0.5f; // Example: 4:3
   [SerializeField] private float maxAspectRatio = 0.5617977528f; // Example: 16:9

   private Camera _mainCamera;
   private Camera _blackBarCamera;

   void Start()
   {
      _mainCamera = GetComponent<Camera>();
      if (_mainCamera == null)
      {
         Debug.LogError("Main Camera component not found!");
         return;
      }

      // Create a secondary camera for black bars
      GameObject blackBarCameraObject = new GameObject("BlackBarCamera");
      _blackBarCamera = blackBarCameraObject.AddComponent<Camera>();
      _blackBarCamera.clearFlags = CameraClearFlags.SolidColor;
      _blackBarCamera.backgroundColor = Color.black;
      _blackBarCamera.cullingMask = 0; // Render nothing except the black background
      _blackBarCamera.depth = _mainCamera.depth - 1; // Render behind the main camera
   }

   void Update()
   {
      if (_mainCamera == null || _blackBarCamera == null) return;

      float currentAspectRatio = (float)Screen.width / Screen.height;

      if (currentAspectRatio < minAspectRatio)
      {
         float viewportWidth = currentAspectRatio / minAspectRatio;
         _mainCamera.rect = new Rect(0, (1 - viewportWidth) / 2, 1, viewportWidth);
      }
      else if (currentAspectRatio > maxAspectRatio) // Too wide
      {
         float viewportHeight = maxAspectRatio / currentAspectRatio;
         _mainCamera.rect = new Rect((1 - viewportHeight) / 2, 0, viewportHeight, 1);
      }
      else
      {
         _mainCamera.rect = new Rect(0, 0, 1, 1); // Fullscreen if within the aspect ratio range
      }
   }
   
}