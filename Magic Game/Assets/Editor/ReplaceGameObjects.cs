    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    // CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
    // March 2010
     
    //Modified by Kristian Helle Jespersen
    //June 2011
     
    public class ReplaceGameObjects : ScriptableWizard
    {
        //public bool copyValues = true;
        public GameObject NewType;
        public GameObject[] OldObjects;
     
        [MenuItem("Custom/Replace GameObjects")]
     
     
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace");
        }
     
        void OnWizardCreate()
        {
            //Transform[] Replaces;
            //Replaces = Replace.GetComponentsInChildren<Transform>();
			
			if (NewType == null)
			{
				Debug.LogWarning("No new gameobject/prefab assigned!");
				return;
			}
			
			if (OldObjects.Length <= 0)
			{
				Debug.LogWarning("No objects assigned to be replaced!");
				return;
			}
			
			int replacedObjectCount = 0;
			
			foreach (GameObject go in OldObjects)
			{
				if (go == null)
				{
					Debug.LogWarning("A null object was found, ignoring replacement...");
					continue;
				}
				
				GameObject newObject;
				newObject = (GameObject)PrefabUtility.InstantiatePrefab(NewType);
				newObject.transform.position = go.transform.position;
				newObject.transform.rotation = go.transform.rotation;
				newObject.transform.parent = go.transform.parent;
			 
				DestroyImmediate(go);
				replacedObjectCount++;
			}
			
			Debug.Log("Replaced " + replacedObjectCount + " object(s).");
        }
    }
