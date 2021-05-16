using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSprite : MonoBehaviour {
    public string SpriteSheet;

    private Dictionary<string, Sprite> spriteSheet;
    private SpriteRenderer srenderer;

    private void Start() {
        this.srenderer = GetComponentInChildren<SpriteRenderer>();
         LoadSprites();
    }

    private void LateUpdate() {
        this.srenderer.sprite = this.spriteSheet[SpriteSheet + "_" + this.srenderer.sprite.name.Substring(10)];
    }

    private void LoadSprites() {
        var sprites = Resources.LoadAll<Sprite>(SpriteSheet);
        this.spriteSheet = sprites.ToDictionary(x => {
            Debug.Log(SpriteSheet + "_" + x.name.Substring(10));
            return (SpriteSheet + "_" +x.name.Substring(10));

        }, x => x);
    }
}
