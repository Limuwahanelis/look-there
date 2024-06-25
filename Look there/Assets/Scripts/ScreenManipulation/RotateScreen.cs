using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RotateScreen : MonoBehaviour
{
    [SerializeField] Transform _cameraPivot;
    [SerializeField] float _rotationSpeed;
    [SerializeField] GameObject _normalHelperCanvas;
    [SerializeField] GameObject _rotatedHelperCanvas;
    private float _totalRotation=0;
    private bool _isRotating;
    private bool _shouldSwapCanvases=true;
    bool _isRotatedVertically=false;
    bool _isRotatedHorizontally = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RotateCam()
    {
        _totalRotation = 0;
        _shouldSwapCanvases = true;
        StartCoroutine(rotate());
    }
    private void RotateCamHorizontally()
    {
        _totalRotation = 0;
        _shouldSwapCanvases = true;
        StartCoroutine(rotateHori());
    }
    IEnumerator rotate()
    {
        if(_isRotating) yield break;
        _isRotating = true;
        while (_totalRotation < 180)
        {
            _totalRotation += _rotationSpeed * Time.deltaTime;
            _cameraPivot.Rotate(_cameraPivot.up, _rotationSpeed * Time.deltaTime);
            if(_totalRotation>90)
            {
                if(_shouldSwapCanvases)
                {
                    if(!_isRotatedVertically)
                    {
                        _normalHelperCanvas.SetActive(false);
                        _rotatedHelperCanvas.SetActive(true);
                    }
                    else
                    {
                        _normalHelperCanvas.SetActive(true);
                        _rotatedHelperCanvas.SetActive(false);
                    }
                    _shouldSwapCanvases = false;
                }
            }
            yield return null;
        }
        Quaternion tmp = _cameraPivot.rotation;
        tmp.eulerAngles = new Vector3(_isRotatedHorizontally ? 180 : 0, _isRotatedVertically ? 0 : 180, 0);
        //_cameraPivot.rotation = Quaternion.Euler(_cameraPivot.rotation.x, _firstRot ? 180 : 0, _cameraPivot.rotation.z);
        _cameraPivot.rotation = tmp;
        _isRotating = false;
        _isRotatedVertically = !_isRotatedVertically;
    }
    IEnumerator rotateHori()
    {
        if (_isRotating) yield break;
        _isRotating = true;
        while (_totalRotation < 180)
        {
            _totalRotation += _rotationSpeed * Time.deltaTime;
            _cameraPivot.Rotate(_cameraPivot.right, _rotationSpeed * Time.deltaTime);
            //if (_totalRotation > 90)
            //{
            //    if (_shouldSwapCanvases)
            //    {
            //        if (_firstRot)
            //        {
            //            _normalHelperCanvas.SetActive(false);
            //            _rotatedHelperCanvas.SetActive(true);
            //        }
            //        else
            //        {
            //            _normalHelperCanvas.SetActive(true);
            //            _rotatedHelperCanvas.SetActive(false);
            //        }
            //        _shouldSwapCanvases = false;
            //    }
            //}
            yield return null;
        }
        Quaternion tmp = _cameraPivot.rotation;
        tmp.eulerAngles = new Vector3(_isRotatedHorizontally ? 0 : 180, _isRotatedVertically ? 180 : 0, 0);
        //_cameraPivot.rotation = Quaternion.Euler(_firstRot ? 180 : 0, _cameraPivot.rotation.y, _cameraPivot.rotation.z);
        _isRotatedHorizontally = !_isRotatedHorizontally;
        _cameraPivot.rotation = tmp;
        _isRotating = false;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RotateScreen))]
    public class RotateScreenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Rotate"))
            {
                (target as RotateScreen).RotateCam();
            }
            if (GUILayout.Button("RotateHori"))
            {
                (target as RotateScreen).RotateCamHorizontally();
            }
        }
    }
#endif
}


