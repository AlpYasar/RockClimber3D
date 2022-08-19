using System;
using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Controllers
{
    public class RockSelector : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private LayerMask rockLayer;
        [SerializeField] private GameObjectEvent onRockTargetSelected;
        [SerializeField] private BoolVariable isGameActive;
        #endregion

        #region Private Fields
        private IEnumerator m_HitCheckerCoroutine;
        private Camera m_Camera;
        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Camera = Camera.main;
            OnGameStart();
        }

        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && isGameActive.Value)
            {
                var hit = Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out var hitInfo, 100, rockLayer);
                if (hit) 
                {
                    Debug.Log("Hit");
                    onRockTargetSelected.Raise(hitInfo.collider.gameObject);
                }
            }
        }
        

        #endregion
        
        #region Public Methods



        public void OnGameStart()
        {
            isGameActive.Value = true;
            //m_HitCheckerCoroutine = HitChecker();
            //StartCoroutine(m_HitCheckerCoroutine);
        }
    
        public void OnGameEnd()
        {
            //StopCoroutine(m_HitCheckerCoroutine);
            isGameActive.Value = false;
        }
    
        #endregion

        #region Private Methods
        private IEnumerator HitChecker()
        {
            //var wait = new WaitForSeconds(Time.fixedDeltaTime);
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out var hitInfo, 100, rockLayer);
                    if (hit) 
                    {
                        Debug.Log("Hit");
                        onRockTargetSelected.Raise(hitInfo.collider.gameObject);
                    }
                }
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            // ReSharper disable once IteratorNeverReturns
        }
        #endregion

    }
}
