   M S B A M L   `   `   `  ���� H  DPMApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PMApplicationPMApplication      PMApplication.LoginPage   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:PMApplication       N� Title$  	LoginPage��.��� 
���   � � RowDefinitions �� $	��25* ��� $	��50* ��� $	��25* �.+��� $��Center=�$��Center=�$��50(�$
��Bold"�$��0��
��� ���.��Password Manager� $��1�� �� $��33.3* ��� $��33.3* ��� $��33.4* �.+���  �� Orientation$ 
Horizontal=�$��Center=�$	��Top=�$��0��.+��� $��40(�
��� ���.��	Username:�� -   $��usernameTextBox��$��10,0,0,0q�$	��200��$��40(�
���   �
��� � ��� $ 
Horizontal=�$��Center=�$��Center=�$��1��.+��� 
����p��$��30(�.��Public�� -   $��publicKeyFileLabel��$��15(�
�������$	��150��
�������$��Center=�$��Center=��� -   $��publicKeyChooseFileButton��$��30(�
�������.��choose file�� 
����p��$��30(�$��10,0,0,0q�.��	Private�� -   $��privateKeyFileLabel��$��15(�
�������$	��150��
�������$��Center=�$��Center=��� -   $ ��privateKeyChooseFileButton��$��30(�
�������.��choose file�� $ 
Horizontal=�$��Center=�$	��Top=�$��2��.+��� -   $��loginButton��$
��Auto��$
��Auto��$��40(�
��� ���
����  �.��Login�� -   $��newUserButton��$��50,0,0,0q�$
��Auto��$
��Auto��$��40(�
��� ���
����  �.��
New User�� $��2��$��30(�$��Center=�$��Bottom=�
�������.��64Copyright © 2023 "Niv Schuman" All rights reserved.