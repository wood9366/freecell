using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoSingleton<Config> {
    public Vector3 _CardStackInitial = new Vector3(0, 0, -0.1f);
	public Vector3 _CardStackOffset = new Vector3(0, -50, -0.1f);

    public float _CardFlySpeed = 1.0f;
    public float _DealCardInterval = 0.1f;

    public Vector3 CardStackInitial { get { return _CardStackInitial; } }
    public Vector3 CardStackOffset { get { return _CardStackOffset; } }

    public float CardFlySpeed { get { return _CardFlySpeed; } }
    public float DealCardInterval { get { return _DealCardInterval; } }
}
