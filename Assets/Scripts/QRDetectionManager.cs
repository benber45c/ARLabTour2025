using UnityEngine;

#if ZXING_ENABLED
using PassthroughCameraSamples;

public class QRDetectionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private QrCodeScanner qrScanner;
    [SerializeField] private ObjectSpawnerOnQR objectSpawner;
    
    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;
    
    private void Awake()
    {
        // Auto-find components if not assigned
        if (qrScanner == null)
            qrScanner = FindObjectOfType<QrCodeScanner>();
            
        if (objectSpawner == null)
            objectSpawner = FindObjectOfType<ObjectSpawnerOnQR>();
    }
    
    private void Start()
    {
        if (qrScanner == null)
        {
            Debug.LogError("QrCodeScanner not found! Make sure it's in the scene.");
            return;
        }
        
        if (objectSpawner == null)
        {
            Debug.LogError("ObjectSpawnerOnQR not found! Make sure it's in the scene.");
            return;
        }
        
        // Since QrCodeScanner might not have events, we'll use a different approach
        if (enableDebugLogs)
            Debug.Log("QR Detection Manager initialized successfully");
            
        // We'll need to modify the QrCodeScanner to add event support
        Debug.LogWarning("QRDetectionManager: Events need to be added to QrCodeScanner for full functionality");
    }
    
    // This method will be called manually from a modified QrCodeScanner
    public void OnQRCodeDetected(QrCodeResult qrCode)
    {
        if (enableDebugLogs)
            Debug.Log($"QR Code detected: {qrCode.text}");
            
        // Forward to object spawner
        if (objectSpawner != null)
            objectSpawner.OnQRCodeDetected(qrCode);
    }
}
#else
public class QRDetectionManager : MonoBehaviour
{
    private void Start()
    {
        Debug.LogError("ZXING_ENABLED is not defined. Please add it to Scripting Define Symbols.");
    }
}
#endif