   M S B A M L   `   `   `  ���� H  DPMApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PMApplicationPMApplication      PMApplication.UserPage   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:PMApplication       N� Title$  UserPage��.��� 
���   � � RowDefinitions �� $	��20* ��� $	��60* ��� $	��10* ��� $	��10* �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Stretch=�.+��� -   $��
HelloLabel��$��40(�$
��Left=�$��Center=�
�������.��Hello User!�� -   $��25(�$��Right=�$	��Top=�$��
400,10,0,0q�
���  ��
�������.��Delete Account�� $��1��$��Stretch=�$��Stretch=�$
��Auto=�.��&   System.Windows.Controls.DataGrid  -   $��PasswordsDataGrid��   AutoGenerateColumns$ False��$
��Left=�
�������   CanUserReorderColumns$ False��   CanUserAddRows$ False��   CanUserDeleteRows$ False��   CanUserResizeColumns$ False��   CanUserSortColumns$ False��	 � ContextMenu	 �� .@�m� 
 m� Command$

 Copy�� � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value:  4System.Windows.Controls.Primitives.DataGridRowHeader��%�   '        '  �     �� "  .���  � ��
 ��ح���  � ��$ SemiBold"���  � ��$ 0,0,1,2q���  � ��
 �   ���  � ��$ 20(��� "  .���  � ��
 ����    RowStyle �� )  #System.Windows.Controls.DataGridRow"  .���  � ��
 � ���   	CellStyle �� *  $System.Windows.Controls.DataGridCell"   �� Triggers P�  P� Property �@ 
IsSelected P� Value$
 True��. ���  � �� ��  �� Color$ Transparent����  � ��# � ����  � �� �� $ Transparent��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 238    
IsReadOnly$
 True��   Header$ Source��1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *Source+  $	 238 $
 True��$ Password�� �� *
Password+4
  .System.Windows.Controls.DataGridTemplateColumn
  $	 238 $ Show Password�� 
  CellTemplate �� .R��� -   
��� � �.��Show
  $ 65 $ Delete�� �� .R��� -   $��Center=�$��Center=�
���  ��.��X�� $��2��$��42��$ 
Horizontal=�.+��� -   $��50��$��30(�
�������$��Stretch=�.��⟳�� -   
�������$	��200��$��20(�$
��Left=�$��Stretch=�.��Add New Password�� $��3��$��20(�$��Center=�$��Bottom=�
�������.��64Copyright © 2023 "Niv Schuman" All rights reserved.