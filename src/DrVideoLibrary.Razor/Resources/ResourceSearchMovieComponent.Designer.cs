﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrVideoLibrary.Razor.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResourceSearchMovieComponent {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceSearchMovieComponent() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DrVideoLibrary.Razor.Resources.ResourceSearchMovieComponent", typeof(ResourceSearchMovieComponent).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search.
        /// </summary>
        internal static string ButtonLabel {
            get {
                return ResourceManager.GetString("ButtonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception. Please try again..
        /// </summary>
        internal static string OnMovieSelectErrorMessage {
            get {
                return ResourceManager.GetString("OnMovieSelectErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Movie not found. Try from original title..
        /// </summary>
        internal static string SearchMoviesErrorMessage {
            get {
                return ResourceManager.GetString("SearchMoviesErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Title are required.
        /// </summary>
        internal static string SearchTitleErrorEmptyMessage {
            get {
                return ResourceManager.GetString("SearchTitleErrorEmptyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Title:.
        /// </summary>
        internal static string TitleLabel {
            get {
                return ResourceManager.GetString("TitleLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search by title....
        /// </summary>
        internal static string TitlePlaceHolder {
            get {
                return ResourceManager.GetString("TitlePlaceHolder", resourceCulture);
            }
        }
    }
}
