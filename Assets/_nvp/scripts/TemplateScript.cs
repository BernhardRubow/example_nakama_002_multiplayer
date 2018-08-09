using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateScript : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();
	}

    void Update () {
		
	}

	void OnDestroy(){
		UnsubscribeFromEvents();
	}




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void SubscribeToEvents()
    {
        throw new NotImplementedException();
    }

    private void UnsubscribeFromEvents()
    {
        throw new NotImplementedException();
    }
}
