using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIModel : Model
{
	public UIGameModel		UIGameModel				{ get { return _UIGameModel 		= SearchLocal<UIGameModel>(			_UIGameModel,		"UIGameModel");	} }
	public UIMenuModel		UIMenuModel				{ get { return _UIMenuModel			= SearchLocal<UIMenuModel>(			_UIMenuModel,		"UIMenuModel");	} }

	private UIGameModel		_UIGameModel;
	private UIMenuModel		_UIMenuModel;
}

