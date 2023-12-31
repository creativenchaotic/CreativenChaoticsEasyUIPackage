using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script is based on Kemble Software's "Custom text effects in Unity with TextMeshPro, in like 130 seconds!" tutorial https://www.youtube.com/watch?v=FXMqUdP3XcE
//Expanded it adding premade effects that can be toggled in the inspector

public class Text_Effects_CreativenChaotic : MonoBehaviour
{
    [SerializeField] TMP_Text textToAnimate;

    /// <summary>
    /// TextMeshPro creates a mesh per character. We modify those meshes.
    /// </summary>

    // Update is called once per frame
    void Update()
    {
        textToAnimate.ForceMeshUpdate();//Make sure the meshes are up to date

        var textInfo = textToAnimate.textInfo;


        for (int i = 0; i < textInfo.characterCount; i++)//We go through each character
        {
            var characterInfo = textInfo.characterInfo[i];

            if (!characterInfo.isVisible)//We skip invisible characters
            {
                continue;
            }

            //All characters that share 1 unity material are combined into 1 mesh each
            //Each vertex in the mesh represent an individual character

            //Get the vertices from the mesh associated with the current characters material
            var vertices = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;

            //Loop through each vertex in the character
            for (int vert = 0; vert < 4; vert++){
                var originalVert = vertices[characterInfo.vertexIndex + vert];

                //We now modify the vertices to create the effect
                vertices[characterInfo.vertexIndex + vert] = originalVert + new Vector3(0, Mathf.Sin(Time.time * 2.0f + originalVert.x * 0.01f) * 10.0f, 0);
            }
        }

        //The object from textInfo.meshInfo contains 2 copies of the vertices which you can use as a draft copy and working copy
        //textInfo.meshInfo[i].vertices <--- Draft copy (var vertices defined above)
        //textInfo.meshInfo[i].mesh.vertices <--- Working copy
        //We have modified the draft copy so we need to update the working copy
        for (int finalCharVerts = 0; finalCharVerts < textInfo.meshInfo.Length; finalCharVerts++)
        {
            var meshInfo = textInfo.meshInfo[finalCharVerts];
            meshInfo.mesh.vertices = meshInfo.vertices;//Updating the working copy with the draft copy
            textToAnimate.UpdateGeometry(meshInfo.mesh, finalCharVerts);//Setting the final vertex geometry positions
        }
    }
}
