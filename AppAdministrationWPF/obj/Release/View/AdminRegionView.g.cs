﻿#pragma checksum "..\..\..\View\AdminRegionView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6969B6AEF30E5EF5DCF9A6BFA93E2C6EE24617B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AppAdministrationWPF.View {
    
    
    /// <summary>
    /// AdminRegionView
    /// </summary>
    public partial class AdminRegionView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 76 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridMap;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement Carte;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer Map;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl mapBackground;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid1;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btModifierCarte;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btAddPlace;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btEditPlace;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\View\AdminRegionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btDeletePlace;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AppAdministrationWPF;component/view/adminregionview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\AdminRegionView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            this.GridMap = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.Carte = ((System.Windows.Controls.MediaElement)(target));
            return;
            case 5:
            this.Map = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.mapBackground = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 7:
            this.grid1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.btModifierCarte = ((System.Windows.Controls.Button)(target));
            
            #line 117 "..\..\..\View\AdminRegionView.xaml"
            this.btModifierCarte.Click += new System.Windows.RoutedEventHandler(this.btModifierCarte_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btAddPlace = ((System.Windows.Controls.Button)(target));
            
            #line 119 "..\..\..\View\AdminRegionView.xaml"
            this.btAddPlace.Click += new System.Windows.RoutedEventHandler(this.btAddPlace_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btEditPlace = ((System.Windows.Controls.Button)(target));
            
            #line 121 "..\..\..\View\AdminRegionView.xaml"
            this.btEditPlace.Click += new System.Windows.RoutedEventHandler(this.miEditPH_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btDeletePlace = ((System.Windows.Controls.Button)(target));
            
            #line 123 "..\..\..\View\AdminRegionView.xaml"
            this.btDeletePlace.Click += new System.Windows.RoutedEventHandler(this.miDeletePH_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 1:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Primitives.ButtonBase.ClickEvent;
            
            #line 21 "..\..\..\View\AdminRegionView.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.selectOne);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonUpEvent;
            
            #line 71 "..\..\..\View\AdminRegionView.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Map_MouseLeftButtonUp);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

