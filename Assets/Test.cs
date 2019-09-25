using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PausableCoroutine _p;
    bool _isPaused;

    // Start is called before the first frame update
    void Start()
    {
        _p = new PausableCoroutine(this, Kuku());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPaused = !_isPaused;
            _p.SetPaused(_isPaused);
        }
    }

    IEnumerator Kuku()
    {
        int x = 1;

        while (true)
        {
            Debug.Log("-------- " + x++);

            yield return new WaitForSeconds(0.5f);
            yield return Foo();
        }
    }

    IEnumerator Foo()
    {
        int x = 0;

        while (x < 5)
        {
            Debug.Log("*** " + (x++));

            yield return new WaitForSeconds(0.5f);
            yield return new TestYield(0.5f);
        }
    }
}

class TestYield : CustomYieldInstruction
{
    readonly float _duration;
    readonly Dictionary<int, float> _frameTimes;

    public TestYield(float duration)
    {
        _duration = duration;
        _frameTimes = new Dictionary<int, float>();
    }

    public override bool keepWaiting
    {
      get
      {
          _frameTimes[Time.frameCount] = Time.deltaTime;

          Debug.Log(Time.frameCount + "TEST");

          return GetTotalTime() < _duration;
      }
    }

    float GetTotalTime()
    {
        float result = 0f;
        foreach (float frameTime in _frameTimes.Values)
            result += frameTime;
        return result;
    }
}
