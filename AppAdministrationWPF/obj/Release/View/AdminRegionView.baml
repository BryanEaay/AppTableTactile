   M S B A M L   `   `   `  ����   AppAdministrationWPF/    )AppAdministrationWPF.View.AdminRegionView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $
��1665��$
��1000��$��0q�$��Center=�$	��Top=�
���   �
���   �$��1q�$��1����Z�   � 	Resources     BaseTemplate  NoNothingButton �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   	Direction   Opacity �� 
TargetType  PlaceHoldersList �� Path��%F  &	        &	 #     &	 �    7  �� .R��� ���� *
IconPath+�� " ��.���  �@�OverridesDefaultStyle . ��  � ��$ 0q���  �@�Cursor$	 Pen����  � ��$ Center=���  � ��$ Center=�-   ��  � �� �	#01000000��  � z� V� .~��� �� �� T� ��  �@J�Effect   $ 36Q�$	 275Q�$ 0Q���  � �� �� " ��.R��� $��border��#��z��._��� $��Center=�$��Center=��� " ��.���  � �� �� .R��� $��0q���  � �� �� .R��� 8��  ���� $
��None=����� $ IconPath�.��� -   	 �Name$	 GridMap��$
��1000��$	��Top=�
���   �
 � RowDefinitions
 �� $	��820 ��� $��1* � � ColumnDefinitions �� $
��1665 �.+��� -    ��Name$ canvasCarte��$
��1490��$	��780��.+��� -    ��Name$ Carte��$��0q�$��Uniform=����� $ ActualWidth� �� ElementName$ canvasCarte������ $ ActualHeight�$ canvasCarte���� -    ��Name$ mapBackground��$
��Left=�$	��Top=�
������ $
��Auto=�#��[ ���� $ ActualWidth�$ canvasCarte������ $ ActualHeight�$ canvasCarte������ *SelectedMap.PlaceHolders+���� .���  �@��Top �� $ Y���  �@��Left �� $ X��� $��1��$��10,10q�
�������$��2q� �� CornerRadius$ 20��._�� -   $	 grid1��}�.  �� $��* ��� $��3* ��� $��80 ��� $	��300 �
 �� $��* ��� $��* �.+��� $��0��$��1��$��Center=�$	��Top=�$��16(�
�������.��Modification des Cartes :�� $��0��$��1��$��10q�$��Center=�$��Bottom=� �� Orientation$ 
Horizontal=�.+��� -    ��Name$ btAddMap��$	��150��$��35��$��Center=�$��Center=�#��� ��$��Ajouter carte����Z� �� -   $ btModifierCarte��$	��150��$��35��$
��50,0q�$��Center=�$��Center=�#��� ��$��Modifier carte����Z� �� -	   $ btDeleteMap��$	��150��$��35��$��Center=�$��Center=�#��� ��$��Supprimer carte����Z� �� $��1��$��1��$��Center=�$	��Top=�$��16(�
�������.��Modification des balises :�� $��1��$��1��$��10q�$��Center=�$��Bottom=�$ 
Horizontal=�.+��� -
   $ 
btAddPlace��$	��150��$��35��$��Center=�$��Center=�#��� ��$��Ajouter lieu����Z� �� -   $ btEditPlace��$	��150��$��35��$
��50,0q�$��Center=�$��Center=�#��� ��$��Modifier lieu����Z� �� -   $ btDeletePlace��$	��150��$��35��$��Center=�$��Center=�#��� ��$��Supprimer lieu����Z� �� $��2��$��0��$��Center=�$ 
Horizontal=�.+��� -   $ 
btImporter��$��1��$	��100��$��35��$��25q�$��Center=�$��Center=�#��� ��$��Importer����Z� �� -   $ 
btExporter��$��0��$	��100��$��35��$��25q�$��Center=�$��Center=�#��� ��$��Exporter����Z� �� -   $ buttonUp��$��0��$��2��$��50��$��60��#��� ��$��^��$��24(��� -   $ 
buttonDown��$��1��$��2��$��50��$��60��#��� ��$��v��$��24(��� -    ��Name$ listboxMaps��$��2��$��3��$
��10,5q� �� DisplayMemberPath$
 Name��$��16(� �� SelectionMode$ Single=����� *Maps+