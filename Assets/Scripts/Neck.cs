using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neck : MonoBehaviour {

	public GameObject neckPiecePrefab;
	public List<GameObject> neckPieces;
	public int neckPieceCount;

	public Head head;
	public GameObject root;
	public Vector3 middle;

	public float neckRaiseFactor;

	public float scalePerSection;  // make neck sections a little bigger as they go from root to head

	// Use this for initialization
	void Start () {

		// create the neck pieces
		for( int i = 0; i < neckPieceCount; i++){
			GameObject piece = Instantiate( neckPiecePrefab ) as GameObject;
			piece.transform.localScale += Vector3.one * i * scalePerSection;
			piece.transform.parent = Hydra.instance.transform;
			neckPieces.Add( piece );

		}

	}

	// Update is called once per frame
	void Update (){

		if (Hydra.instance.state == Hydra.HydraState.ALIVE) {

			float distance = Vector3.Distance (head.transform.position, root.transform.position);
			Vector3 middle = (head.transform.position + root.transform.position) / 2;
			middle += new Vector3 (0, ((head.maxDistance / distance) - 1) * neckRaiseFactor, 0);

			for (int i = 0; i < neckPieces.Count; i++) {
				neckPieces [i].transform.position = Bezier2 (head.transform.position, middle, root.transform.position, (1.0f * i / neckPieces.Count));
			}
		}

	}


	//gotten from: http://answers.unity3d.com/questions/12689/moving-an-object-along-a-bezier-curve.html
	protected Vector3 Bezier2(Vector3 start,Vector3 control, Vector3 end, float t)
	{
		return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}
}
