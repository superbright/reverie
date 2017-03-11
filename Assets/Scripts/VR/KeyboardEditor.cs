using UnityEngine;
using System.Collections;
using Valve.VR;

public class KeyboardEditor : MonoBehaviour
{
	public UnityEngine.UI.InputField textEntry;
	public bool minimalMode;
 
    static bool keyboardShowing;
	string text = "";
	static KeyboardEditor activeKeyboard = null;




	void OnEnable()
	{
       // SteamVR_Events.Event.Listen("KeyboardCharInput", OnKeyboard);
       // SteamVR_Events.Event.Listen("KeyboardClosed", OnKeyboardClosed);
	}

	private void OnKeyboard(object[] args)
	{
		if (activeKeyboard != this)
			return;
		Valve.VR.VREvent_t ev = (Valve.VR.VREvent_t)args[0];
		VREvent_Keyboard_t keyboard = ev.data.keyboard;
		byte[] inputBytes = new byte[] { keyboard.cNewInput0, keyboard.cNewInput1, keyboard.cNewInput2, keyboard.cNewInput3, keyboard.cNewInput4, keyboard.cNewInput5, keyboard.cNewInput6, keyboard.cNewInput7 };
		int len = 0;
		for (; inputBytes[len] != 0 && len < 7; len++) ;
		string input = System.Text.Encoding.UTF8.GetString(inputBytes, 0, len);

		if (minimalMode)
		{
			if (input == "\b")
			{
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			else if (input == "\x1b")
			{
				// Close the keyboard
				var vr = SteamVR.instance;
				vr.overlay.HideKeyboard();
				keyboardShowing = false;
			}
			else
			{
				text += input;
			}
			textEntry.text = text;
		}
		else
		{
			System.Text.StringBuilder textBuilder = new System.Text.StringBuilder(1024);
			uint size = SteamVR.instance.overlay.GetKeyboardText(textBuilder, 1024);
			text = textBuilder.ToString();
            textEntry.text = text;
		}
	}

	private void OnKeyboardClosed(object[] args)
	{
		if (activeKeyboard != this)
			return;
		keyboardShowing = false;
		activeKeyboard = null;
    }

	public void ShowKeyboard(string inputtitle)
	{
		if(!keyboardShowing)
		{
			keyboardShowing = true;
			activeKeyboard = this;
			//SteamVR.instance.overlay.ShowKeyboard(0, 0, inputtitle, 256, text, minimalMode, 0);
            SteamVR.instance.overlay.ShowKeyboardForOverlay(0, 0, 0, inputtitle, 140, "", false, 0);
		}
	}


}
