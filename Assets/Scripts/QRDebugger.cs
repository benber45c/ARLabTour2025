using UnityEngine;
using UnityEngine.InputSystem;

#if ZXING_ENABLED
using PassthroughCameraSamples;
#endif

public class QRDebugger : MonoBehaviour
{
    [Header("Auto-Find References")]
    [SerializeField] private bool autoFindReferences = true;
    
    [Header("Manual References (if auto-find fails)")]
    [SerializeField] private WebCamTextureManager webCamManager;
    [SerializeField] private MonoBehaviour qrScanner; // Changed from QrCodeScanner
    [SerializeField] private ObjectSpawnerOnQR objectSpawner;
    
    [Header("Debug Controls")]
    [SerializeField] private bool enableVRControls = true;
    
    private void Start()
    {
        if (autoFindReferences)
        {
            FindAllReferences();
        }
        
        Debug.Log("QRDebugger initialized. Press SPACE or Right Controller A to debug.");
    }
    
    private void FindAllReferences()
    {
        if (webCamManager == null)
            webCamManager = FindObjectOfType<WebCamTextureManager>();
            
        if (qrScanner == null)
        {
            // Try to find QrCodeScanner by name since we don't have the namespace
            var scanners = FindObjectsOfType<MonoBehaviour>();
            foreach (var scanner in scanners)
            {
                if (scanner.GetType().Name == "QrCodeScanner")
                {
                    qrScanner = scanner;
                    break;
                }
            }
        }
            
        if (objectSpawner == null)
            objectSpawner = FindObjectOfType<ObjectSpawnerOnQR>();
            
        Debug.Log($"Auto-found references: WebCam={webCamManager != null}, QRScanner={qrScanner != null}, ObjectSpawner={objectSpawner != null}");
    }
    
    private void Update()
    {
        // New Input System - Check for Space key
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DebugQRSystem();
        }
        
        // VR Controller debug (OVR Input still works)
        if (enableVRControls && (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Two)))
        {
            DebugQRSystem();
        }
    }
    
    private void DebugQRSystem()
    {
        Debug.Log("=== QR SYSTEM DEBUG START ===");
        
        // Check WebCam Status
        DebugWebCam();
        
        // Check QR Scanner
        DebugQRScanner();
        
        // Check Object Spawner
        DebugObjectSpawner();
        
        // Check Permissions
        DebugPermissions();
        
        // Check Dependencies
        DebugDependencies();
        
        Debug.Log("=== QR SYSTEM DEBUG END ===");
    }
    
    private void DebugWebCam()
    {
        Debug.Log("--- WebCam Status ---");
        if (webCamManager != null && webCamManager.WebCamTexture != null)
        {
            var webCam = webCamManager.WebCamTexture;
            Debug.Log($"✓ WebCam Found: {webCam.deviceName}");
            Debug.Log($"✓ Is Playing: {webCam.isPlaying}");
            Debug.Log($"✓ Resolution: {webCam.width}x{webCam.height}");
            Debug.Log($"✓ FPS: {webCam.requestedFPS}");
        }
        else
        {
            Debug.LogError("✗ WebCamTextureManager or WebCamTexture is null!");
        }
    }
    
    private void DebugQRScanner()
    {
        Debug.Log("--- QR Scanner Status ---");
        if (qrScanner != null)
        {
            Debug.Log($"✓ QR Scanner Found: {qrScanner.name}");
            Debug.Log($"✓ Enabled: {qrScanner.enabled}");
            Debug.Log($"✓ GameObject Active: {qrScanner.gameObject.activeInHierarchy}");
        }
        else
        {
            Debug.LogError("✗ QrCodeScanner not found!");
        }
    }
    
    private void DebugObjectSpawner()
    {
        Debug.Log("--- Object Spawner Status ---");
        if (objectSpawner != null)
        {
            Debug.Log($"✓ Object Spawner Found: {objectSpawner.name}");
            Debug.Log($"✓ Enabled: {objectSpawner.enabled}");
        }
        else
        {
            Debug.LogError("✗ ObjectSpawnerOnQR not found!");
        }
    }
    
    private void DebugPermissions()
    {
        Debug.Log("--- Permissions Status ---");
        
        // Check Scene permission
        bool scenePermission = OVRPermissionsRequester.IsPermissionGranted(OVRPermissionsRequester.Permission.Scene);
        Debug.Log($"{(scenePermission ? "✓" : "✗")} Scene Permission: {scenePermission}");
    }
    
    private void DebugDependencies()
    {
        Debug.Log("--- Dependencies Status ---");
        
        // Check ZXing
        #if ZXING_ENABLED
        Debug.Log("✓ ZXing is enabled");
        #else
        Debug.LogError("✗ ZXing is NOT enabled - check Scripting Define Symbols");
        #endif
        
        // Check if we're in editor or on device
        #if UNITY_EDITOR
        Debug.Log("ℹ Running in Unity Editor");
        #elif UNITY_ANDROID
        Debug.Log("ℹ Running on Android Device");
        #endif
    }
    
    // Manual trigger for inspector button
    [ContextMenu("Debug QR System")]
    public void ManualDebug()
    {
        DebugQRSystem();
    }
}
