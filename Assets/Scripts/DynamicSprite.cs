using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicSprite : MonoBehaviour {
    public Sprite Sprite;

    private Sprite startSprite;
    private SpriteRenderer SpriteRenderer;

    public List<Sprite> spriteSheet;
    private void Awake() => this.SpriteRenderer = this.SpriteRenderer != null ? this.SpriteRenderer : GetComponent<SpriteRenderer>();

    private void Start() {
        LoadSprites();
    }

    private void LateUpdate() {
        if(this.spriteSheet == null || this.startSprite != Sprite) LoadSprites();

        this.SpriteRenderer.sprite = this.spriteSheet.ElementAt(int.Parse(this.SpriteRenderer.sprite.name.Substring(this.SpriteRenderer.sprite.name.LastIndexOf('_') + 1)));

    }
    private void LoadSprites(){
        this.startSprite = Sprite;
        this.spriteSheet = Resources.LoadAll<Sprite>(Sprite.name.Substring(0, Sprite.name.LastIndexOf('_'))).ToList();
    }
}
