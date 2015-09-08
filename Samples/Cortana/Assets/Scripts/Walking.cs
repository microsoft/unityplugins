using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Microsoft.UnityPlugins;

public class Walking : MonoBehaviour {

    private Animator animator;
    public Text speechText;

    void Start () {
        animator = GetComponent<Animator>();
        Speech.ListenForCommands(new string[] { "walk", "dance", "stop", "jump", "run" }, (result) =>
        {
            if (result != null)
            {
                if (result.Status == SpeechResultStatus.Command)
                {
                    speechText.text = result.Text;
                    switch (result.Text)
                    {
                        case "walk":
                            animator.SetTrigger("walk");
                            break;
                        case "dance":
                            animator.SetTrigger("dance");
                            break;
                        case "stop":
                            animator.SetTrigger("stop");
                            break;
                        case "jump":
                            animator.SetTrigger("jump");
                            break;
                        case "run":
                            animator.SetTrigger("run");
                            break;
                    }
                }
                else if(result.Status == SpeechResultStatus.Dictation
                || result.Status == SpeechResultStatus.Hypothesis
                )
                {
                    speechText.text = result.Text;
                }
            }
        });
	}
	
	void Update () {
	
		#region Manual input

		if (Input.GetKey(KeyCode.UpArrow))
		{
			animator.SetTrigger("dance");
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			animator.SetTrigger("stop");
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			animator.SetTrigger("walk");
		}

		#endregion
	}

}
