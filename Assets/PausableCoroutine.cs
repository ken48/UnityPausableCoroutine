using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableCoroutine : IEnumerator
{
    public object Current => _isPaused ? null : GetCurrentEnumerator().Current;

    readonly MonoBehaviour _coHolder;
    readonly Stack<IEnumerator> _coStack;
    Coroutine _unityCo;
    bool _isPaused;

    public PausableCoroutine(MonoBehaviour coHolder, IEnumerator co)
    {
        _coHolder = coHolder;
        _coStack = new Stack<IEnumerator>();
        _coStack.Push(co);

        _unityCo = _coHolder.StartCoroutine(this);
    }

    public void Stop()
    {
        if (_unityCo == null)
            return;

        _coHolder.StopCoroutine(_unityCo);
        _unityCo = null;
    }

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;
    }

    public bool MoveNext()
    {
        if (_isPaused)
            return true;

        bool moveNext = GetCurrentEnumerator().MoveNext();
        if (moveNext)
        {
            var current = GetCurrentEnumerator().Current;
            if (current is IEnumerator curCo && curCo != GetCurrentEnumerator())
                _coStack.Push(curCo);
        }
        else
        {
            _coStack.Pop();
            moveNext = _coStack.Count > 0;
        }

        return moveNext;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    IEnumerator GetCurrentEnumerator()
    {
        return _coStack.Peek();
    }
}
