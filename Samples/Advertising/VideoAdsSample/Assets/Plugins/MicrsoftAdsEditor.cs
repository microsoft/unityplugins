#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 

namespace Microsoft.UnityPlugins
{
    public class EditorAdFactory : IInterstitialAdFactory
    {
        public IInterstittialAd CreateAd()
        {
            return new EditorInterstitialAd();
        }

        public IInterstittialAd CreateAd( Action<object> readyCallback,
            Action<object> completedCallback, Action<object> cancelledCallback, Action<object> errorCallback)
        {
            EditorInterstitialAd ad = new EditorInterstitialAd( readyCallback, completedCallback, cancelledCallback, errorCallback );
            return ad; 
        }
    }

    public class EditorInterstitialAd : IInterstittialAd
    {
        private Action<object> cbReady, cbCompleted, cbError, cbCancelled;
        private bool hasPendingRequests = false;
        private bool playNextReady = false;
        private InterstitialAdState state = InterstitialAdState.NotReady;
        public MonoBehaviour syncMonoBehaviour;
        public UnityEngine.Canvas canvas;
        public bool fireError = false; 

        

        public EditorInterstitialAd()
        {
            
        }

        public EditorInterstitialAd(Action<object> readyCallback,
            Action<object> completedCallback, Action<object> cancelledCallback, Action<object> errorCallback)
        {
            this.cbCompleted = completedCallback;
            this.cbReady = readyCallback;
            this.cbError = errorCallback;
            this.cbCancelled = cancelledCallback; 
        }

        public InterstitialAdState State
        {
            get { return state; }
        }

        private void StartCoroutine(IEnumerator e)
        {
            if (syncMonoBehaviour != null)
                syncMonoBehaviour.StartCoroutine(e);
        }


        public void AddCallback(AdCallback type, Action<object> cb)
        {
            switch (type)
            {
                case AdCallback.Ready:
                    cbReady = cb;
                    break;
                case AdCallback.Completed:
                    cbCompleted = cb;
                    break;
                case AdCallback.Cancelled:
                    cbCancelled = cb;
                    break;
                case AdCallback.Error:
                    cbError = cb;
                    break;
            }
        }

        public void ClearCallback(AdCallback type)
        {
            Action<object> cb = null;
            switch (type)
            {
                case AdCallback.Ready:
                    cbReady = cb;
                    break;
                case AdCallback.Completed:
                    cbCompleted = cb;
                    break;
                case AdCallback.Cancelled:
                    cbCancelled = cb;
                    break;
                case AdCallback.Error:
                    cbError = cb;
                    break;
            }
        }

        public void Request(string appId, string adUnitId, AdType type)
        {
            hasPendingRequests = true;
            if( fireError )
                StartCoroutine(PrepareError());
            else 
                StartCoroutine(PrepareRequest());
        }
        
        private IEnumerator PrepareError()
        {
            
            fireError = false; 
            yield return new WaitForSeconds(1f);

            ErrorCode randomError = (ErrorCode)UnityEngine.Random.Range( (int)ErrorCode.Unknown, (int) ErrorCode.ValidationFailure);
            AdErrorEventArgs arg = new AdErrorEventArgs (randomError, randomError.ToString()); 
            Fire(AdCallback.Error, arg ); 
        } 
        

        private IEnumerator PrepareRequest()
        {
            yield return new WaitForSeconds(1f);
            hasPendingRequests = false;
            state = InterstitialAdState.Ready;
            Fire(AdCallback.Ready);
            if (playNextReady)
            {
                playNextReady = false;
                Show();
            }
        }


        private void Fire(AdCallback cb, object passthrough = null )
        {
            switch (cb)
            {
                case AdCallback.Ready:
                    state = InterstitialAdState.Ready;
                    if (cbReady != null)
                        cbReady(passthrough);
                    break;
                case AdCallback.Completed:
                    state = InterstitialAdState.Closed;
                    if (cbCompleted != null)
                        cbCompleted(passthrough);

                    break;
                case AdCallback.Cancelled:
                    state = InterstitialAdState.Closed;
                    if (cbCancelled != null)
                        cbCancelled(passthrough);
                    break;
                case AdCallback.Error:
                    state = InterstitialAdState.Closed;
                    if (cbError != null)
                        cbError(passthrough);
                    break;
            }
        }



        public void RequestAndShow(string appId, string adUnitId)
        {
            playNextReady = true;
            Request(appId, adUnitId, AdType.Video);
        }
         

        public void Show()
        {
            if (fireError)
            {
                StartCoroutine(PrepareError());
                return; 
            }

            if (state == InterstitialAdState.Ready )
            {
                state = InterstitialAdState.Showing;
                StartCoroutine(DoShow(10f));
            }
            else
                throw new InvalidOperationException("Not Ready. You must request ad first");
        }

        private IEnumerator DoShow(float delay)
        {
            if (canvas != null)
            {
                GameObject go = new GameObject();
                go.AddComponent<RectTransform>();
                var image = go.AddComponent<Image>();
                image.color = Color.red;
                RectTransform rt = image.rectTransform;
                image.transform.SetParent(canvas.transform);
                image.sprite = Resources.Load<Sprite>("EmptyImage"); 
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
                rt.localPosition = Vector3.zero;

                go = new GameObject();
                go.AddComponent<RectTransform>();
                var text = go.AddComponent<Text>();
                text.color = Color.white;
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 48;
                text.transform.SetParent(canvas.transform);
                rt = text.rectTransform;
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
                rt.localPosition = Vector3.zero;
                text.text = "This ugly ad will be replaced by nice full screen video ad when not in editor";
                yield return new WaitForSeconds(delay);
                GameObject.Destroy(image);
                GameObject.Destroy(text);
            }
            else
                yield return new WaitForSeconds(delay);


            Fire(AdCallback.Completed);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ClearCallback(AdCallback.Cancelled);
                    ClearCallback(AdCallback.Error);
                    ClearCallback(AdCallback.Completed);
                    ClearCallback(AdCallback.Ready);                     
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EditorInterstitialAd() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
} 

#endif 
