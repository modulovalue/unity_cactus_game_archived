using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVPlayerMidiController : MonoBehaviour {
	public List<ListOfKnobSettingsContainer> knobSettingList = new List<ListOfKnobSettingsContainer>();
}

[System.Serializable]
public class ListOfKnobSettingsContainer {
	public string description;
	public List<KnobSettingsContainer> list;
}