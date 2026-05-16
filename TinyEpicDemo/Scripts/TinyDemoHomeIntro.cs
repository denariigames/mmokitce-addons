using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Delete this script if you copying ThemeUIPanel-ServerList to your own project.
/// </summary>
public class TinyDemoHomeIntro : MonoBehaviour
{
	[SerializeField] private Toggle introToggle;
	[SerializeField] private GameObject[] introContent;
	[SerializeField] private GameObject[] regularContent;

	private const string PREF_KEY = "OptOut_ShowTinyDemoIntro";  

	void Awake()
	{
		if (introToggle == null || introContent == null)
		{
			Debug.LogWarning("OptOutPrompt is missing Toggle or Container reference!", this);
			return;
		}
		ToggleContent();
	}

	public void OnCloseButtonPressed()
	{
		// Only save opt-out if they actually checked it
		if (introToggle.isOn)
		{
			PlayerPrefs.SetInt(PREF_KEY, 1);
			PlayerPrefs.Save();
		}

		ToggleContent(true);
	}

	private void ToggleContent(bool introOff = false)
	{
		if (introOff || PlayerPrefs.GetInt(PREF_KEY, 0) == 1)
		{
			foreach (GameObject go in introContent)
			{
				go.SetActive(false);
			}
			foreach (GameObject go in regularContent)
			{
				go.SetActive(true);
			}
		}
		else
		{
			introToggle.isOn = false;
			foreach (GameObject go in introContent)
			{
				go.SetActive(true);
			}
			foreach (GameObject go in regularContent)
			{
				go.SetActive(false);
			}
		}
	}

	// Reset for testing (call from inspector or debug menu)
	[ContextMenu("Reset Opt-Out (for testing)")]
	private void ResetOptOut()
	{
		PlayerPrefs.DeleteKey(PREF_KEY);
		PlayerPrefs.Save();
		Debug.Log("Opt-out reset. Will show prompt again next play.");
	}
}
