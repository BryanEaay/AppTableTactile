   M S B A M L   `   `   `  ���� C  ?FluidKit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG1clr-namespace:FluidKit.Controls;assembly=FluidKitFluidKit.Controls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  AppPalaisRois%   AppPalaisRois.AppRegionMainView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Ffluidkit1clr-namespace:FluidKit.Controls;assembly=FluidKit     e aMicrosoft.Surface.Presentation, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e351s)http://schemas.microsoft.com/surface/2008  0sys&clr-namespace:System;assembly=mscorlib  -     �Name$  Region�� � Title$ Region��$
��1920��$
��1080��$��0q�$��0q� � WindowState$ 	Maximized=� � WindowStyle$
 None=� � 	Resources    ListTemplate K� ZIndex  
Background ��Name �� Padding  NoNothingButton �� 
TargetType	 �� Property
 �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   	Direction   Opacity �� 
TargetType  PlaceHoldersList��%�  &	        &	 y     &	 �     &	 L    7  �� .R�� $	��120��$	��120��.+��� $ 1��
��������� $��10q�$ 10��$��Uniform=����� *
Background+�� .R��� $ ElementVisual��$��0��$ 0q�
���   �
�������$��0q�._��� $��Uniform=����� �� " ��.��� 	 �@�OverridesDefaultStyle
 . �� 	 �@�Cursor$	
 Pen���� 	 � ��$
 Center=��� 	 � ��$
 Center=��� 	 � ��
 �	#01000000�� 	 � z�
 V� .~��� �� �� T� �� 	 �@J�Effect
   $ 36Q�$	 275Q�$ 0Q��� 	 � ��
 �� " ��.R��� $ border��#��z��._��� $��Center=�$��Center=��� " ��.��� 	 � ��
 �� .R��� �� 	 � ��
 �� .R��� -   8��  .���� $
��None=����� *
IconPath+.��� -    �Name$ regionPanel��$
��1920��$
��1080��
���   �.+��� -    ��Name$ 	imageFond������  �� Path$ ActualWidth� �� ElementName$ regionPanel������ $ ActualHeight�$ regionPanel���  � ColumnDefinitions �� $��* ��� $
��1490 ��� $��* � � RowDefinitions �� $��* ��� $	��780 ��� $��3* �.+�#   FluidKit.Controls.ElementFlow  -     Name$ 	flowCarte��$��1��$��1��   ElementHeight$	 780Q�   ElementWidth$
 1490Q�#���     Layout !   FluidKit.Controls.CoverFlow     Camera =� -   
camera�� =� FieldOfView$
 1500Q� � LookDirection$ 0,0,-65� � Position$ 0,0,2.651��� $��1��$��1��$��0q� J� RenderTransformOrigin$	 0,0&�#��[ ���� $ ActualHeight�$ ScatterView������ *Selected.PlaceHolders+���� .��� 	 �@��Top
 �� $ Y��� 	 �@��Left
 �� $ X��� -    ��Name$ Quit_button��$��2��$��0��$	��120��$	��120��$��Center=�$��Center=���Z� ��Z� .���� .+��� -   $ returnRegion��$	��100��$	��100�� J� Opacity$	 1.0Q��� $��2��$��1��$
��0,20q�$��Center=�  �� Orientation$  
Horizontal=�.+��� -	   ! ��Name$! listboxMaps��$
��1490��$��Center=�$��Center=�
������ $��0q�#��[  $��Hidden=�" �� SelectionMode$" Single=����� *Maps+���� .R�� # K� IsItemsHost# . 9  3Microsoft.Surface.Presentation.Controls.ScatterView  -
   $  Name$$ ScatterView��$��1��$��1��$��0q�$��0q�$
��Left=�$	��Top=�% J� IsHitTestVisible% . & J� IsManipulationEnabled& . y��� *ScatterViewVisibility+.@�=  7Microsoft.Surface.Presentation.Controls.ScatterViewItem  $��0q�$��1q�G  AMicrosoft.Surface.Presentation.Controls.ScatterContentControlBase'   CanMove' . (   	CanRotate( . )   CanScale) . *   Center$* 745,390&�& .  +   IsTopmostOnActivation+ . ,   Orientation$, 0Q�-   ZIndex$- 1������ $ ActualWidth�$ ScatterView������ $ ActualHeight�$ ScatterView����Z� 