   M S B A M L   `   `   `  ����L  HMyDentDiagnostics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:MyDentDiagnosticsMyDentDiagnostics  5   	   #    MyDentDiagnostics.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:MyDentDiagnostics     5   	     � Title$  Inicio de sesión��5      $��308.179��5	      $��152.582��6    � Icon$ Images/app.ico��6#    � WindowStartupLocation$ CenterScreen=�69    � 
ResizeMode$ CanMinimize=�6^   .��5      � -   5         �Name$ 
LayoutRoot��5      .+�5   
   �� -   5         ��Name$ 	lblUserId��5      $��Número de empleado:��6#   $
��Left=�6A   $��27��6\   $��13.507,11.441,0,0q�6h   $	��Top=�6�   $	��132��6�   �� 5   
   -   5        $ lblPassword��5      $��Contraseña:��6%   $
��Left=�6;   $��27��6V   $��71.507,43.441,0,0q�6b   $	��Top=�6}   $��74��6�   G� 5   
   -   5         G�Name$ 
pbPassword��5      $
��Left=�6*   $��27��6E   $��150.507,43.441,0,0q�6Q   $	��Top=�6m   $	��130��6�   $��1��6�   
���   �$��Center=�6�   �� 5   
   -   5         ��Name$ 	txtUserId��5      $
��Left=�6%   $��27��6@   $��150.507,11.441,0,0q�6L    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��130��6�   $��0��6�   
���   �$��Center=�6�   �� 5   
   -   5        	 ��Name$	 btnLogin��5      $��Iniciar sesión��6#   $
��Left=�6<   $��24.97��6W   $��95.507,83.441,0,0q�6f   $	��Top=�6�   $��90��6�   
 � Cursor$

 Hand��6�   $��2��6�   �� 5   
   -   5        $	 btnClose��5      $��Cerrar��6#   $
��Left=�64   $��24.97��6O   $��190.507,83.441,0,0q�6^   $	��Top=�6z   $��90��6�   $

 Hand��6�   $��3��6�   �� 5   
   -   5        $ lblLoginStatus��5      $��Iniciando sesión...��6(   $
��Left=�6F   $��10,98.582,0,0q�6a   $��175.507��6x   $
��Bold"�6�   $��Italic$�6�   $��27��6�   $	��Top=�6�   $y�Hidden=�6�   5      5      