using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsPoolModel : Model
{
	public List<GameObject>		poolingList		{ get { return _poolingList; } }

	private List<GameObject> 	_poolingList 	= new List<GameObject>();

}

