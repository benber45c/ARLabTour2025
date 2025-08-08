using System.Collections.Generic;
using UnityEngine;
using PassthroughCameraSamples;

#if ZXING_ENABLED
public class ObjectSpawnerOnQR : MonoBehaviour
{
    // A helper class to create a mapping in the Inspector between
    // the text content of a QR code and the prefab to spawn.
    [System.Serializable]
    public class QRCodePrefabMapping
    {
        public string QRCodeText;
        public GameObject PrefabToSpawn;
    }

    [Tooltip("Populate this array in inspector with QRCodeMapping scriptable objects.")]
    [SerializeField]
    private QRCodeMapping[] qrCodeMappings; // Use QRCodeMapping consistently

    // A dictionary for lookups of prefabs by their QR code text.
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();

    // A dictionary to track the objects already spawned for each QR code text,
    private Dictionary<string, GameObject> _spawnedObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach (QRCodeMapping mapping in qrCodeMappings)
        {
            if (!string.IsNullOrEmpty(mapping.qrCodeString) && !_prefabDictionary.ContainsKey(mapping.qrCodeString))
            {
                _prefabDictionary.Add(mapping.qrCodeString, mapping.associatedPrefab);
                Debug.Log($"Added QR mapping: '{mapping.qrCodeString}' -> {mapping.associatedPrefab.name}");
            }
        }
        
        Debug.Log($"ObjectSpawnerOnQR initialized with {_prefabDictionary.Count} mappings");
    }

    // Update the parameter type to match the QrCodeResult from the QuestCameraKit sample
    public void OnQRCodeDetected(QrCodeResult qrCode)
    {
        string decodedText = qrCode.text;
        Debug.Log($"Processing QR Code: '{decodedText}'");

        if (_prefabDictionary.TryGetValue(decodedText, out GameObject prefabToSpawn))
        {
            if (_spawnedObjects.TryGetValue(decodedText, out GameObject spawnedObject))
            {
                // Calculate position and rotation from QR code corners
                Vector3 position = Vector3.zero;
                foreach (var corner in qrCode.corners)
                {
                    position += corner;
                }
                position /= qrCode.corners.Length;

                // Calculate rotation from QR code corners
                Vector3 forward = Vector3.Cross(
                    qrCode.corners[1] - qrCode.corners[0],
                    qrCode.corners[3] - qrCode.corners[0]
                ).normalized;

                Vector3 up = (qrCode.corners[1] - qrCode.corners[0]).normalized;

                spawnedObject.transform.SetPositionAndRotation(
                    position,
                    Quaternion.LookRotation(forward, up)
                );
            }
            else
            {
                // If this is a new QR code and hasn't spawned an object yet, instantiate it.
                Debug.Log($"Found new QR Code with text: '{decodedText}'. Spawning object.");

                Vector3 position = Vector3.zero;
                foreach (var corner in qrCode.corners)
                {
                    position += corner;
                }
                position /= qrCode.corners.Length;

                Vector3 forward = Vector3.Cross(
                    qrCode.corners[1] - qrCode.corners[0],
                    qrCode.corners[3] - qrCode.corners[0]
                ).normalized;

                Vector3 up = (qrCode.corners[1] - qrCode.corners[0]).normalized;

                GameObject newObject = Instantiate(
                    prefabToSpawn,
                    position,
                    Quaternion.LookRotation(forward, up)
                );

                // Add the newly spawned object to tracking dictionary.
                _spawnedObjects.Add(decodedText, newObject);
            }
        }
        else
        {
            Debug.LogWarning($"No prefab mapping found for QR code text: '{decodedText}'");
        }
    }
}
#endif
