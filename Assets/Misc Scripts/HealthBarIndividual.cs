using UnityEngine;
using UnityEditor;
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
	public Sprite offTex = Object.Instantiate(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/EnergyBarEmpty.png")) as Sprite;
	public Sprite onTex	= Object.Instantiate(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/EnergyBarFilled.png")) as Sprite;
	public Sprite offSprite;
	public Sprite onSprite;


	// Use this for initialization
	public HealthBarIndividual(HealthBarIndividual.State initialState){
		offSprite = offTex;
		onSprite = onTex;
		renderer = go.AddComponent<SpriteRenderer> () as SpriteRenderer;
		transform = go.GetComponent<Transform> ();
		renderer.sortingOrder = 30;
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
