   M S B A M L   `   `   `  ����H  DPMApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PMApplicationPMApplication  5          PMApplication.LoginPage   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:PMApplication     5        N� Title$  	LoginPage��5	      .��5      � 
���   � � RowDefinitions 5   
   �� 5      $	��25* �6   6+   �� 5      $	��50* �6   6+   �� 5      $	��25* �6   6+   .+�5   
   �� $��Center=�6   $��Center=�6-   $��50(�6H   $
��Bold"�6V   $��0��6h   
��� ���.��6�   Password Manager6�   � 5   
   $��1��6    5      �� 5      $��33.3* �6    61   �� 5      $��33.3* �6    61   �� 5      $��33.4* �6    61   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $	��Top=�6O   $��0��6g   .+�5      �� $��40(�6   
��� ���.��6:   	Username:6E   �� 5      -   5        $��usernameTextBox��5   l   $��10,0,0,0q�6   $	��200��6,   $��40(�68   
���   �
��� � �6�   5       �� 5"      $ 
Horizontal=�6   $��Center=�62   $��Center=�6O   $��1��6j   .+�5#      �� 
����p��$��30(�62   .��6@   Public6H   �� 5$      -   5        $��publicKeyFileLabel��5$   �   $��15(�6   
�������$	��150��69   
�������$��Center=�6\   $��Center=�6�   6�   �� 5%      -   5        $��publicKeyChooseFileButton��5%   9   $��30(�6   
�������.��6Z   choose file6g   �� 5'      
����p��$��30(�62   $��10,0,0,0q�6@   .��6R   	Private6[   �� 5(      -   5        $��privateKeyFileLabel��5(   �   $��15(�6   
�������$	��150��69   
�������$��Center=�6\   $��Center=�6�   6�   �� 5)      -   5        $ ��privateKeyChooseFileButton��5)   9   $��30(�6   
�������.��6[   choose file6h   5*      �� 5,      $ 
Horizontal=�6   $��Center=�62   $	��Top=�6O   $��2��6g   .+�5-      �� -   5        $��loginButton��5-   h   $
��Auto��6   $
��Auto��6&   $��40(�64   
��� ���
����  �.��6{   Login6�   �� 5.      -   5        $��newUserButton��5.   z   $��50,0,0,0q�6   $
��Auto��6+   $
��Auto��68   $��40(�6F   
��� ���
����  �.��6�   
New User6�   5/      50      �� 53   
   $��2��6   $��30(�6   $��Center=�6+   $��Bottom=�6H   
�������.��6u   64Copyright © 2023 "Niv Schuman" All rights reserved.6�   54      55      