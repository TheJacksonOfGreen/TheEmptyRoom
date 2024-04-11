using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureChanger : MonoBehaviour {
    [Tooltip("List of textures to change between.")]
    public List<Texture> textures;
    private int index;
    private RawImage img;

    // Start is called before the first frame update
    void Start() {
        index = 0;
        img = this.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update() {
        index += 1;
        if (index >= textures.Count) {
            index = 0;
        }
        img.texture = textures[index];
    }
}
