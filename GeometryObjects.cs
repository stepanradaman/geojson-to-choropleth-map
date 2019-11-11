using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GeoJSON {

	[System.Serializable]
	public class GeometryObject : GeoJSONObject {

		public GeometryObject() : base() {
		}

		public GeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}

		/*
		 * Returns all PositionObjects in the Geometry as a single list
		 */
		virtual public List<PositionObject> AllPositions() {
			return null;
		}

		/*
		 * Returns first PositionObject in the Geometry
		 */
		virtual public PositionObject FirstPosition() {
			return null;
		}

		/*
		 * Returns the number of all PositionObjects in the Geometry
		 */
		
		
		virtual public int PositionCountMultiPolygon() {
			return 0;
		}
		/**/
		
		virtual public int PositionCount() {
			return 0;
		}

		virtual public int PositionCountPolygon() {
			return 0;
		}
		
		virtual public List<int> PositionCountPolygonMulti() {
			return null;
		}
		
		virtual public List<int> PositionCountPolygonArray() {
			return null;
		}

		virtual public List<List<int>> PositionCountMultiPolygonArray() {
			return null;
		}

		virtual public List<List<List<PositionObject>>> MultiAllPositions() {
			return null;
		}

		override protected void SerializeContent(JSONObject rootObject) {
			JSONObject coordinateObject = SerializeGeometry ();
			rootObject.AddField ("coordinates", coordinateObject);
		}

		virtual protected JSONObject SerializeGeometry() { return null; }
	}

	[System.Serializable]
	public class SingleGeometryObject : GeometryObject {
		public PositionObject coordinates;

		public SingleGeometryObject() : base() {
			type = "Point";
			coordinates = new PositionObject ();
		}

		public SingleGeometryObject(float longitude, float latitude) : base() {
			type = "Point";
			coordinates = new PositionObject (longitude, latitude);
		}

		public SingleGeometryObject(JSONObject jsonObject) : base(jsonObject) {
			coordinates = new PositionObject (jsonObject["coordinates"]);
		}

		override public List<PositionObject> AllPositions() {
			List<PositionObject> list = new List<PositionObject> ();
			list.Add (coordinates);
			return list;
		}

		override public PositionObject FirstPosition() {
			return coordinates;
		}

		override public int PositionCount() {
			return 1;
		}

		override protected JSONObject SerializeGeometry() {
			return coordinates.Serialize();
		}
	}
	[System.Serializable]
	public class ArrayGeometryObject : GeometryObject {
		public List<PositionObject> coordinates;

		public ArrayGeometryObject(JSONObject jsonObject) : base(jsonObject) {
			coordinates = new List<PositionObject>();
			foreach(JSONObject j in jsonObject["coordinates"].list){
				coordinates.Add(new PositionObject (j));
			}
		}

		override public List<PositionObject> AllPositions() {
			return coordinates;
		}

		override public PositionObject FirstPosition() {
			if(coordinates.Count > 0)
				return coordinates[0];

			return null;
		}

		override public int PositionCount() {
			return coordinates.Count;
		}

		override protected JSONObject SerializeGeometry() {

			JSONObject coordinateArray = new JSONObject (JSONObject.Type.ARRAY);
			foreach (PositionObject position in coordinates) {
				coordinateArray.Add (position.Serialize());
			}

			return coordinateArray;
		}
	}
	[System.Serializable]
	public class ArrayArrayGeometryObject : GeometryObject {
		public List<List<PositionObject>> coordinates;

		public ArrayArrayGeometryObject(JSONObject jsonObject) : base(jsonObject) {

			coordinates = new List<List<PositionObject>> ();
			foreach (JSONObject l in jsonObject["coordinates"].list) {
				List<PositionObject> polygon = new List<PositionObject>();
				coordinates.Add (polygon);
				foreach (JSONObject l2 in l.list) {
					polygon.Add (new PositionObject (l2));
				}
			}
		}

		override public List<PositionObject> AllPositions() {
			List<PositionObject> list = new List<PositionObject> ();
			foreach (List<PositionObject> l in coordinates) {
				foreach (PositionObject pos in l) {
					list.Add (pos);
				}
			}
			return list;
		}

		override public PositionObject FirstPosition() {
			if(coordinates.Count > 0 && coordinates[0].Count > 0)
				return coordinates[0][0];

			return null;
		}

		override public int PositionCountPolygon() {
			return coordinates.Count;
		}

			
		override public int PositionCount() {
		
			int totalPositions = 0;
			
			for (int i = 0; i < coordinates.Count; i++) {
				totalPositions += coordinates[i].Count;
			}	
			
			return totalPositions;
		}

		
		override public List<int> PositionCountPolygonArray() {
			List<int> arraypolylist = new List<int> ();
			
			for (int i = 0; i < coordinates.Count; i++) {
				arraypolylist.Add (coordinates[i].Count);

			}	
			
			return arraypolylist;
		}
		

		override protected JSONObject SerializeGeometry() {

			JSONObject coordinateArrayArray = new JSONObject (JSONObject.Type.ARRAY);

			foreach (List<PositionObject> l in coordinates) {
				JSONObject coordinateArray = new JSONObject (JSONObject.Type.ARRAY);
				foreach (PositionObject pos in l) {
					coordinateArray.Add (pos.Serialize());
				}
				coordinateArrayArray.Add (coordinateArray);
			}

			return coordinateArrayArray;
		}
	}

	

	[System.Serializable]
	public class ArrayArrayArrayGeometryObject : GeometryObject {
		public List<List<List<PositionObject>>> coordinates;
		
		
		public ArrayArrayArrayGeometryObject(JSONObject jsonObject) : base(jsonObject) {

			coordinates = new List<List<List<PositionObject>>> ();
			
			foreach (JSONObject l in jsonObject["coordinates"].list) {
				List<List<PositionObject>> polygon = new List<List<PositionObject>>();
				coordinates.Add (polygon);
				
				foreach (JSONObject l2 in l)
				{
					List<PositionObject> polygon1 = new List<PositionObject>();
					polygon.Add (polygon1);
					foreach (JSONObject l3 in l2) {
						polygon1.Add (new PositionObject (l3));
					}
				}
			}
		}
		
		
		override public List<List<List<PositionObject>>> MultiAllPositions() {
			List<List<List<PositionObject>>> arraypolylist = new List<List<List<PositionObject>>> ();
			
			for (int i = 0; i < coordinates.Count; i++)
			{
				List<List<PositionObject>> arraypolylist1 = new List<List<PositionObject>> ();				
				for (int j = 0; j < coordinates[i].Count; j++)
				{
					List<PositionObject> arraypolylist2 = new List<PositionObject> ();
					for (int k = 0; k < coordinates[i][j].Count; k++)
					{
						arraypolylist2.Add(coordinates[i][j][k]);
					}						
					arraypolylist1.Add(arraypolylist2);
				}
				
				arraypolylist.Add(arraypolylist1);
				
			}
			return arraypolylist;
		}


		
		override public PositionObject FirstPosition() {
			if(coordinates.Count > 0 && coordinates[0].Count > 0 && coordinates[0][0].Count > 0)
				return coordinates[0][0][0];

			return null;
		}
		
		
		override public int PositionCountMultiPolygon() {
			return coordinates.Count;
		}		

		
		override public List<int> PositionCountPolygonMulti() {
			List<int> arraypolylist = new List<int> ();

			for (int i = 0; i < coordinates.Count; i++) {
				arraypolylist.Add (coordinates[i].Count);
	
			}

			return arraypolylist;
		}		
		
		
		
		
		override public int PositionCount() {
			
			int totalPositions = 0;
			for (int i = 0; i < coordinates.Count; i++) {
				for (int j = 0; j < coordinates[i].Count; j++) {
					totalPositions += coordinates[i][j].Count;
				}	
			}
			
			return totalPositions;
		}

		override public List<List<int>> PositionCountMultiPolygonArray() {
			List<List<int>> arraypolylist = new List<List<int>> ();
			
			for (int i = 0; i < coordinates.Count; i++)
			{
				List<int> arraypolylist1 = new List<int> ();				
				for (int j = 0; j < coordinates[i].Count; j++)
				{
					arraypolylist1.Add(coordinates[i][j].Count);
				}
				
				arraypolylist.Add(arraypolylist1);
				
			}
			return arraypolylist;
		}		
		

		
		override protected JSONObject SerializeGeometry() {

			JSONObject coordinateArrayArrayArray = new JSONObject (JSONObject.Type.ARRAY);
			
			foreach (List<List<PositionObject>> l in coordinates)
			{
				
				foreach (List<PositionObject> l2 in l)
				{
					JSONObject coordinateArray = new JSONObject (JSONObject.Type.ARRAY);
					foreach (PositionObject pos in l2) {
						coordinateArray.Add (pos.Serialize());
					}
					coordinateArrayArrayArray.Add (coordinateArray);
				}
				
			}

			return coordinateArrayArrayArray;
		}
		
	}

	

	[System.Serializable]
	public class PointGeometryObject : SingleGeometryObject {
		public PointGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
		public PointGeometryObject(float longitude, float latitude) : base(longitude, latitude) {
		}
	}
	[System.Serializable]
	public class MultiPointGeometryObject : ArrayGeometryObject {
		public MultiPointGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
	}

	[System.Serializable]
	public class LineStringGeometryObject : ArrayGeometryObject {
		public LineStringGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
	}
	[System.Serializable]
	public class MultiLineStringGeometryObject : ArrayArrayGeometryObject {
		public MultiLineStringGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
	}

	[System.Serializable]
	public class PolygonGeometryObject : ArrayArrayGeometryObject {
		public PolygonGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
	}
	[System.Serializable]
	public class MultiPolygonGeometryObject : ArrayArrayArrayGeometryObject {
		public MultiPolygonGeometryObject(JSONObject jsonObject) : base(jsonObject) {
		}
	}
}