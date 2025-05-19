using UnityEngine;
using System;

namespace RuStore.CoreClient {

    public class RuStoreCoreClient {

        public static string PluginVersion = "8.0.0";

        private static RuStoreCoreClient instance = null;
        private AndroidJavaObject clientWrapper { get; }

        public static RuStoreCoreClient Instance {
            get {
                if (instance == null) instance = new RuStoreCoreClient();

                return instance;
            }
        }

        private RuStoreCoreClient() {
            if (!IsPlatformSupported()) return;

            CallbackHandler.InitInstance();
            using (var clientJavaClass = new AndroidJavaClass("ru.rustore.unitysdk.core.RuStoreUnityCoreClient")) {
                clientWrapper = clientJavaClass.GetStatic<AndroidJavaObject>("INSTANCE");
            }
        }

        public bool IsRuStoreInstalled() {
            if (!IsPlatformSupported()) return false;

            return clientWrapper?.Call<bool>("isRuStoreInstalled") ?? false;
        }

        public void openRuStoreDownloadInstruction() {
            if (IsPlatformSupported()) clientWrapper?.Call("openRuStoreDownloadInstruction");
        }

        public void openRuStore() {
            if (IsPlatformSupported()) clientWrapper?.Call("openRuStore");
        }

        public void openRuStoreAuthorization() {
            if (IsPlatformSupported()) clientWrapper?.Call("openRuStoreAuthorization");
        }

        private bool IsPlatformSupported(Action<RuStoreError> onFailure = null) {
            bool isSupported = Application.platform == RuntimePlatform.Android;

            if (!isSupported) {
                onFailure?.Invoke(new RuStoreError() {
                    name = "RuStoreCoreClientError",
                    description = "Unsupported platform"
                });
            }

            return isSupported;
        }
    }
}
