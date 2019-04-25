using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Launch", menuName = "SpellSystem/Modifiers/Launch")]
public class LaunchModifier : SpellScriptableModifier
{

    [SerializeField] private float force = 100f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        var component = spellObject.GetComponent<Launch>();
        if(component != null)
        {
            component.force += this.force;
            return;
        }

        Launch launch = spellObject.AddComponent<Launch>();
        launch.force = this.force;
    }

}
