using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PoolingObjectType
{
	OBSTACLE
}

public class PoolingObject
{
	public PoolingObjectType 	poolingType;
	public Object 				poolingObject;
}

public class ObjectsPoolModel : Model
{
	public Queue<PoolingObject>		poolingQueue		{ get { return _poolingList; } }

	private Queue<PoolingObject> 	_poolingList 	= new Queue<PoolingObject>();

}

