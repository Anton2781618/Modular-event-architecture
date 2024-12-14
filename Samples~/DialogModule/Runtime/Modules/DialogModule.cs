using UnityEngine;
using ModularEventArchitecture;
using System.Collections;

namespace ModularEventArchitecture.Modules.Dialog
{
    public class DialogModule : ModuleBase
    {
        [SerializeField] private bool autoSkipEnabled = false;
        [SerializeField] private float autoSkipDelay = 3f;
        
        private string currentDialog;
        private bool isDialogActive;
        private Coroutine autoSkipCoroutine;

        public override void Initialize()
        {
            Debug.Log($"DialogModule initialized. Auto skip is {(autoSkipEnabled ? "enabled" : "disabled")} with delay {autoSkipDelay}s");
        }

        public override void Dispose()
        {
            if (isDialogActive)
            {
                CloseDialog();
            }
            Debug.Log("DialogModule disposed");
        }

        public void ShowDialog(string text)
        {
            if (isDialogActive)
            {
                CloseDialog();
            }

            currentDialog = text;
            isDialogActive = true;
            Debug.Log($"Showing dialog: {text}");

            if (autoSkipEnabled)
            {
                Debug.Log($"Dialog will auto-skip after {autoSkipDelay} seconds");
                autoSkipCoroutine = StartCoroutine(AutoSkipDialog());
            }
        }

        public void CloseDialog()
        {
            if (!isDialogActive) return;

            if (autoSkipCoroutine != null)
            {
                StopCoroutine(autoSkipCoroutine);
                autoSkipCoroutine = null;
            }

            currentDialog = null;
            isDialogActive = false;
            Debug.Log("Dialog closed");
        }

        private IEnumerator AutoSkipDialog()
        {
            yield return new WaitForSeconds(autoSkipDelay);
            CloseDialog();
        }

        public bool IsDialogActive() => isDialogActive;
        public string GetCurrentDialog() => currentDialog;
    }
}
