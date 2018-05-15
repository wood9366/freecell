using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoSingleton<Timer> {
    public int setTimeOut(float time, System.Action cb) {
        return addTimer(true, time, cb);
    }

    public void clearTimeOut(int uid) {
        TimerData timer;

        if (_timers.TryGetValue(uid, out timer)) {
            timer.uid = -1;
        }
    }

    public int setInterval(float time, System.Action cb) {
        return addTimer(false, time, cb);
    }

    void Update() {
        foreach (var uid in _deletedTimerUids) {
            _timers.Remove(uid);
        }

        _deletedTimerUids.Clear();

        foreach (var timer in _timers) {
            if (timer.Value.uid < 0) {
                continue;
            }

            timer.Value.time += Time.deltaTime;

            if (timer.Value.time >= timer.Value.duration) {
                if (timer.Value.cb != null) {
                    timer.Value.cb();

                    if (timer.Value.once) {
                        _deletedTimerUids.Add(timer.Value.uid);
                    } else {
                        timer.Value.time -= timer.Value.duration;
                    }
                }
            }
        }
    }

    List<int> _deletedTimerUids = new List<int>();

    int addTimer(bool once, float time, System.Action cb) {
        if (cb != null) {
            TimerData data = new TimerData();

            data.once = once;
            data.uid = TimerUid;
            data.time = 0;
            data.duration = time;
            data.cb = cb;

            _timers.Add(data.uid, data);

            return data.uid;
        }

        return -1;
    }

    static int TimerUid { get { return sUid++; } }
    static int sUid = 0;

    class TimerData {
        public int uid;
        public bool once;
        public float time;
        public float duration;
        public System.Action cb;
    }

    Dictionary<int, TimerData> _timers = new Dictionary<int, TimerData>();
}
