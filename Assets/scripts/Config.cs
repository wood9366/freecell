﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoSingleton<Config> {
	public Vector3 _CardStackOffset = new Vector3(0, -50, -0.1f);

    public Vector3 CardStackOffset { get { return _CardStackOffset; } }
}
