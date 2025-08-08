using UnityEngine;

[CreateAssetMenu(fileName = "NewQRMapping", menuName = "Scriptable Objects/QR Code Mapping", order = 1)]
public class QRCodeMapping : ScriptableObject
{
	public string qrCodeString;
	public GameObject associatedPrefab;
}
