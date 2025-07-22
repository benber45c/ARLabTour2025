using System.Collections.Generic;
using PassthroughCameraSamples;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine;
using System;
#if ZXING_ENABLED
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Multi;
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

    [Tooltip("Define the mapping between QR code content and the prefabs to spawn.")]
    [SerializeField]
    private List<QRCodePrefabMapping> qrCodeMappings;

    // A dictionary for lookups of prefabs by their QR code text.
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();

    // A dictionary to track the objects already spawned for each QR code text,
    // preventing duplicate spawning.
    private Dictionary<string, GameObject> _spawnedObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Populate the prefab dictionary from Inspector list for quick lookups later.
        foreach (var mapping in qrCodeMappings)
        {
            if (!string.IsNullOrEmpty(mapping.QRCodeText) && !_prefabDictionary.ContainsKey(mapping.QRCodeText))
            {
                _prefabDictionary.Add(mapping.QRCodeText, mapping.PrefabToSpawn);
            }
        }
    }

    // This public method will be connected to the QRCodeScanner's UnityEvent.
    // The 'QRCode' type here should now correctly resolve to 'QuestCameraKit.QRCode'.
    public void OnQRCodeDetected(Result qrCode)
    {
        string decodedText = qrCode.Text; 

        // Check if a prefab associated with this QR code's text.
        if (_prefabDictionary.TryGetValue(decodedText, out GameObject prefabToSpawn))
        {
            // Check heck if already spawned an object for this text.
            if (_spawnedObjects.TryGetValue(decodedText, out GameObject spawnedObject))
            {
                // Update its position and rotation to match the QR code.
                spawnedObject.transform.SetPositionAndRotation(
                    qrCode.WorldMatrix.GetColumn(3), // Position
                    Quaternion.LookRotation(qrCode.WorldMatrix.GetColumn(2), -qrCode.WorldMatrix.GetColumn(1)) // Rotation
                );
            }
            else
            {
                // If this is a new QR code and hasn't spawned an object yet, instantiate it.
                Debug.Log($"Found new QR Code with text: '{decodedText}'. Spawning object.");

                GameObject newObject = Instantiate(
                    prefabToSpawn,
                    qrCode.WorldMatrix.GetColumn(3), // Position
                    Quaternion.LookRotation(qrCode.WorldMatrix.GetColumn(2), -qrCode.WorldMatrix.GetColumn(1)) // Rotation
                );

                // Add the newly spawned object to tracking dictionary.
                _spawnedObjects.Add(decodedText, newObject);
            }
        }
    }
#endif
}