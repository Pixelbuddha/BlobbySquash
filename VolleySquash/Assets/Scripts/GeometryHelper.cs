using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GeometryHelper {

	public static float DistanceSegmentSegment (Vector3 lineA1, Vector3 lineA2, Vector3 lineB1, Vector3 lineB2) {

		double EPS = 0.00000001;

		Vector3 directionA = lineA2 - lineA1;
		Vector3 directionB = lineB2 - lineB1;
		Vector3 directionAB = lineA1 - lineB1;

		double a = directionA.sqrMagnitude;
		double b = Vector3.Dot(directionA, directionB);
		double c = directionB.sqrMagnitude;
		double d = Vector3.Dot(directionA, directionAB); 
		double e = Vector3.Dot(directionB, directionAB);
		double D = a * c - b * b;

		double sc, sN, sD = D;
		double tc, tN, tD = D;

		if (D < EPS) {
			sN = 0.0;
			sD = 1.0;
			tN = e;
			tD = c;
		}
		else {
			sN = (b * e - c * d);
			tN = (a * e - b * d);
			if (sN < 0.0) {
				sN = 0.0;
				tN = e;
				tD = c;
			}
			else if (sN > sD) {
				sN = sD;
				tN = e + b;
				tD = c;
			}
		}

		if (tN < 0.0) {
			tN = 0.0;

			if (-d < 0.0)
				sN = 0.0;
			else if (-d > a)
				sN = sD;
			else {
				sN = -d;
				sD = a;
			}
		}
		else if (tN > tD) {
			tN = tD;
			if ((-d + b) < 0.0)
				sN = 0;
			else if ((-d + b) > a)
				sN = sD;
			else {
				sN = (-d + b);
				sD = a;
			}
		}

		if (Mathf.Abs((float)sN) < EPS) sc = 0.0;
		else sc = sN / sD;
		if (Mathf.Abs((float)tN) < EPS) tc = 0.0;
		else tc = tN / tD;

		Vector3 dP;
		dP = directionAB + ((float)sc * directionA) - ((float)tc * directionB);

		return Mathf.Sqrt(Vector3.Dot(dP, dP));
	}
}
