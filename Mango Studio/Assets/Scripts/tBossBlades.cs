﻿using UnityEngine;
using System.Collections;
namespace Tutorial{
public class tBossBlades : MonoBehaviour {

	private tBossBladesModel model;
	private tBoss m;
	private float clock;

	// Use this for initialization
	public void init (tBoss owner) {
		this.name = "BossBlade";
		m = owner;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<tBossBladesModel>();						// Add a marbleModel script to control visuals of the gem.
		model.init(this);

		this.transform.position = new Vector3 (m.transform.position.x, m.transform.position.y, m.transform.position.z);

		BoxCollider2D playerbody = gameObject.AddComponent<BoxCollider2D> ();
		playerbody.isTrigger = true;
		transform.localScale = new Vector3 (1f, 4.7f, 1);

		this.m.m.bulletsFolder.Add (this.gameObject);

	}
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (m.transform.position.x, m.transform.position.y, m.transform.position.z);
		this.transform.eulerAngles = new Vector3(0,0,90*clock);
		clock = clock + (Time.deltaTime*3);
	}

	public void Retract(){
	Destroy (this.gameObject);
	}
}
}