   M S B A M L   `   `   `  ����H  DPMApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PMApplicationPMApplication  5          PMApplication.UserPage   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:PMApplication     5        N� Title$  UserPage��5	      .��5      � 
���   � � RowDefinitions 5   
   �� 5      $	��20* �6   6+   �� 5      $	��60* �6   6+   �� 5      $	��10* �6   6+   �� 5      $	��10* �6   6+   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Stretch=�6;   .+�5      �� -   5        $��
HelloLabel��5      $��40(�6&   $
��Left=�64   $��Center=�6O   
�������.��6}   Hello User!6�   �� 5      -   5        $��25(�5   +   $��Right=�69   $	��Top=�6U   $��
400,10,0,0q�6m   
���  ��
�������.��6�   Delete Account6�   5      �� 5   
   $��1��6   $��Stretch=�6$   $��Stretch=�6@   $
��Auto=�6^   .��5      &   System.Windows.Controls.DataGrid  -   5        $��PasswordsDataGrid��5   �      AutoGenerateColumns$ False��6   $
��Left=�63   
�������   CanUserReorderColumns$ False��6`      CanUserAddRows$ False��6~      CanUserDeleteRows$ False��6�      CanUserResizeColumns$ False��6�      CanUserSortColumns$ False��6�   	 � ContextMenu	 5      �� 5      .@�5      m� 
 m� Command$

 Copy��6#   5       � 	Resources 5!      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value:  4System.Windows.Controls.Primitives.DataGridRowHeader��%T  '        '  �     �� 5"      "  6   .�5#      ��  � ��
 ��ح�6K   �� 5$       � ��$ SemiBold"�67   6J   �� 5%       � ��$ 0,0,1,2q�6<   6N   �� 5&       � ��
 �   �6H   �� 5'       � ��$ 20(�65   6B   5(      �� 5*      "  6   .�5+      ��  � ��
 ���� 6M   5,      5-         RowStyle 5/      �� 50      )  #System.Windows.Controls.DataGridRow"  6   .�51      ��  � ��
 � ���6H   52         	CellStyle 55      �� 56      *  $System.Windows.Controls.DataGridCell"  6    �� Triggers 57      P� 58       P� Property �@ 
IsSelected P� Value$
 True��6I   . �59   "   ��  � �� 5:   &   �� 5;   *    �� Color$ Transparent��6:   5=   #   �� 5>   "    � ��# � ��5?      �� 5A   "    � �� 5B   &   �� 5C   *   $ Transparent��6:   5E   #   5F      5H         Columns 5K      0  *System.Windows.Controls.DataGridTextColumn  5L      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 238 6)      
IsReadOnly$
 True��65      Header$ Source��6G   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 6W   �� *Source+6t     5M      $	 238 6)   $
 True��65   $ Password��6G    6Y   �� *
Password+6x   4
  .System.Windows.Controls.DataGridTemplateColumn
  5N      $	 238 6-   $ Show Password��69    
  CellTemplate 5O      �� 5P      .R�5Q   "   �� -   5        
��� � �.��5Q   Q   Show6W   5R      5T      
  5U      $ 65 6-   $ Delete��68    5V      �� 5W      .R�5X   "   �� -   5        $��Center=�5X   @   $��Center=�6b   
���  ��.��6�   X6�   5Y      5[      5]      5^      �� 5a   
   $��2��6   $��42��6"   $ 
Horizontal=�6.   .+�5b      �� -   5        $��50��5b   %   $��30(�60   
�������$��Stretch=�6Q   .��6m   ⟳6p   �� 5c      -   5        
�������$	��200��5c   <   $��20(�6H   $
��Left=�6V   $��Stretch=�6q   .��6�   Add New Password6�   5d      �� 5g   
   $��3��6   $��20(�6   $��Center=�6+   $��Bottom=�6H   
�������.��6u   64Copyright © 2023 "Niv Schuman" All rights reserved.6�   5h      5i      