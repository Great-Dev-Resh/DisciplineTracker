using TMPro;
using UnityEngine;
using UnityNative.Sharing;

namespace GreatDevResh.AppliedStuff.Reports
{
    public class ReportsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _reportText;

        private string generatedText;
        private string textToShare;

        private static IUnityNativeSharing unityNativeSharing = null;

        private static void CreateUnityNativeSharing()
        {
            if (unityNativeSharing == null)
                unityNativeSharing = UnityNativeSharing.Create();
        }
        
        public void GenerateText()
        {
            generatedText = _inputField.text + "\n<sprite=0><sprite=1><sprite=2>";
            textToShare = _inputField.text + "\n🚀⚡️🔥🚀";

            _reportText.text = generatedText;
        }

        public void Share()
        {
            CreateUnityNativeSharing();
            unityNativeSharing.ShareText(textToShare, true, "Select App To Share With");
        }
    }
}