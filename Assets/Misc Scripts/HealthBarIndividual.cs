using UnityEngine;
 
using System.Collections;

public class HealthBarIndividual {

	public enum State
	{
		OFF,
		ON
	}
	public GameObject go = new GameObject();
	public SpriteRenderer renderer;
	public Transform transform;
	public Sprite offTex = Resources.Load<Sprite>("EnergyBarEmpty") as Sprite;
	public Sprite onTex	= Resources.Load<Sprite>("EnergyBarFilled") as Sprite;
	public Sprite offSprite;
	public Sprite onSprite;


	// Use this for initialization
	public HealthBarIndividual(HealthBarIndividual.State initialState){
		offSprite = offTex;
		onSprite = onTex;
		renderer = go.AddComponent<SpriteRenderer> () as SpriteRenderer;
		transform = go.GetComponent<Transform> ();
		renderer.sortingOrder = 300;
		RenderState (initialState);
	}

	public void RenderState(State state){
		switch (state) {
		case State.OFF:
			renderer.sprite = offSprite;
			break;
		case State.ON:
			renderer.sprite = onSprite;
			break;
		}
	}
}
