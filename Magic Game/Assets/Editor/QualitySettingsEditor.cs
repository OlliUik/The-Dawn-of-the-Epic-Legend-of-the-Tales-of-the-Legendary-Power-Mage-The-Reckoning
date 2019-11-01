//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QualitySettingsEditor : ScriptableWizard
{
    public QualityData data = null;

    [MenuItem("Custom/Quality Settings")]

    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<QualitySettingsEditor>("Quality Settings", "Apply");
    }

    private void OnWizardUpdate()
    {
        if (data == null)
        {
            if (QualityManager.DATA == null)
            {
                Debug.LogWarning("Initializing QualityManager...");
                QualityManager.LoadSettings();
            }
            data = QualityManager.DATA;
        }
    }

    private void OnWizardCreate()
    {
        QualityManager.DATA = data;
        QualityManager.SaveSettings();
        QualityManager.ApplySettings();
    }
}
