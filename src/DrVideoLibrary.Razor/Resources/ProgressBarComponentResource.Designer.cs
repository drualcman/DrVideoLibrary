namespace DrVideoLibrary.Razor.Resources {
    using System;
    using System.Resources;
    using System.Globalization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ProgressBarComponentResource {
        
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal ProgressBarComponentResource() {
        }

        internal static ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("DrVideoLibrary.Razor.Resources.ProgressBarComponentResource", typeof(ProgressBarComponentResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string TotalTimeLabel {
            get {
                return ResourceManager.GetString("TotalTimeLabel", resourceCulture);
            }
        }

    }
}