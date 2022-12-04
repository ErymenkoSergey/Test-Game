using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Morkwa.OtherContent
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private float _timeMoving = 1.5f;
        [SerializeField] private float _startHeight = 0.5f;
        [SerializeField] private float _endHeight = 2f;

        private void Start()
        {
            StartCoroutine(ExitMoving());
        }

        private IEnumerator ExitMoving()
        {
            while (true)
            {
                gameObject.transform.DOMoveY(_startHeight, _timeMoving);
                gameObject.transform.DORotate(new Vector3(0, 360, 0), 0.95f, RotateMode.FastBeyond360);
                yield return new WaitForSeconds(_timeMoving);
                gameObject.transform.DOMoveY(_endHeight, _timeMoving);
                gameObject.transform.DORotate(new Vector3(0, 360, 0), 0.95f, RotateMode.FastBeyond360);
                yield return new WaitForSeconds(_timeMoving);
            }
        }
    }
}