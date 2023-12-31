using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics;
using UnityEditor;
using System.Globalization;

//This script is based on Kemble Software's "Custom text effects in Unity with TextMeshPro, in like 130 seconds!" tutorial https://www.youtube.com/watch?v=FXMqUdP3XcE
//Expanded it adding premade effects that can be toggled in the inspector

//TODO: EXPAND LATER WITH A MAP WHERE EACH WORD GETS AUTOMATICALLY ADDED AND YOU CAN CHOOSE AN ANIMATION PER WORD OR TEXT
//OR ADD A VECTOR WHERE WORDS CAN BE ADDED WITH MATCHING ANIMATIONS IN THE INSPECTOR TO ADD WHAT WORDS TO ANIMATE AS NEEDED

public enum TextAnimationTypes
{
    NoAnimation, SineWave, Jiggle, PerlinNoiseVertical, Falling, Spin, Wobble, Typing, WordWobble
}

public class Text_Effects_CreativenChaotic : MonoBehaviour
{

    [SerializeField] TMP_Text textToAnimate;
    [SerializeField] bool rainbowText = false;
    [SerializeField] public Gradient rainbow;
    [SerializeField] TextAnimationTypes howToAnimate;
    [SerializeField] float intensity = 1;
    float gravity = -9.81f;
    float velocity;
    bool hasJumped = false;

    List<int> wordIndexes;
    List<int> wordLengths;
    Color[] colors;

    /// <summary>
    /// TextMeshPro creates a mesh per character. We modify those meshes.
    /// </summary>
    /// 
    void Start()
    {
        //Determine the location of all the spaces in the word and use that to calculate the word indexes and the word lengths
        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textToAnimate.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }

        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);

    }

    // Update is called once per frame
    void Update()
    {
        velocity += gravity * Time.deltaTime;

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

            
            Vector3 originalVert;

            //We now modify the vertices to create the effect
            //Loop through each vertex in the character
            switch (howToAnimate)
            {
                case TextAnimationTypes.NoAnimation:
                    //Do nothing
                    break;

                case TextAnimationTypes.SineWave:
                    for (int vert = 0; vert < 4; vert++)
                    {
                        originalVert = vertices[characterInfo.vertexIndex + vert];
                        vertices[characterInfo.vertexIndex + vert] = originalVert + new Vector3(0, Mathf.Sin(Time.time * 2.0f + originalVert.x * 0.01f) * 10.0f * intensity, 0);
                    }
                    break;

                case TextAnimationTypes.Falling:
                    for (int vert = 0; vert < 4; vert++)
                    {
                        if (velocity!=0 && !hasJumped)
                        {
                            velocity = 20;
                            hasJumped = true;
                        }
                        
                        if(velocity <= 0)
                        {
                            velocity = 0;
                        }

                        Vector3 offset = new Vector3(0,velocity, 0) * (Time.deltaTime + i +1);
                        vertices[characterInfo.vertexIndex + vert] += offset;
                    }
                    break;

                case TextAnimationTypes.PerlinNoiseVertical:
                    for (int vert = 0; vert < 4; vert++)
                    {
                        originalVert = vertices[characterInfo.vertexIndex + vert];
                        vertices[characterInfo.vertexIndex + vert] = originalVert + new Vector3(0, Mathf.PerlinNoise(originalVert.x * Time.time, originalVert.y * Time.time) * 5.0f * intensity, 0);
                    }
                    break;

                case TextAnimationTypes.Jiggle:
                    for (int vert = 0; vert < 4; vert++)
                    {
                        originalVert = vertices[characterInfo.vertexIndex + vert];
                        vertices[characterInfo.vertexIndex + vert] = originalVert + new Vector3(Mathf.PerlinNoise(originalVert.x * Time.time / 2, originalVert.y * Time.time / 2) * 5.0f * intensity, Mathf.PerlinNoise(originalVert.x * Time.time / 2, originalVert.y * Time.time / 2) * 7.0f * intensity, 0);
                    }
                    break;

                case TextAnimationTypes.Wobble:
                    for (int vert = 0; vert < 4; vert++) {
                        Vector3 offset = Wobble(i);
                        vertices[characterInfo.vertexIndex + vert] += offset;
                    }
                    break;

                case TextAnimationTypes.WordWobble:
                    for (int w = 0; w < wordIndexes.Count; w++)
                    {
                        int wordIndex = wordIndexes[w];
                        Vector3 offset = Wobble(w);

                        for (int n = 0; n< wordLengths[w]; n++)
                        {
                            TMP_CharacterInfo c = textInfo.characterInfo[wordIndex + n];
                            int index = c.vertexIndex;
                            vertices[index] += offset;
                            vertices[index + 1] += offset;
                            vertices[index + 2] += offset;
                            vertices[index + 3] += offset;
                        }
                    }
                    break;

            }

            if (rainbowText)
            {
                colors = textToAnimate.mesh.colors;

                for (int w = 0; w < wordIndexes.Count; w++)
                {
                    int wordIndex = wordIndexes[w];
                    Vector3 offset = Wobble(w);

                    for (int n = 0; n < wordLengths[w]; n++)
                    {
                        TMP_CharacterInfo c = textInfo.characterInfo[wordIndex + n];
                        int index = c.vertexIndex;
                        colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
                        colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
                        colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
                        colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));
                    }
                }
            }
            else
            {
                colors = textToAnimate.mesh.colors;
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
            textToAnimate.mesh.colors = colors;
            textToAnimate.UpdateGeometry(meshInfo.mesh, finalCharVerts);//Setting the final vertex geometry positions
        }
    }

    Vector3 Wobble(int i)
    {
        return new Vector3(Mathf.Sin((Time.time + i) * 1.1f * intensity), Mathf.Cos((Time.time + i) * 0.8f * intensity), 0);
    }
}

