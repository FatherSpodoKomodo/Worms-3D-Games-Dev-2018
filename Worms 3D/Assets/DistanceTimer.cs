﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTimer : MonoBehaviour {
    public Transform StartMarker;
    public Transform EndMarker;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartMarker.position, EndMarker.position);
	}
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(StartMarker.position, EndMarker.position, fracJourney);
    }
}
