   M S B A M L   `   `   `  ����  MyDentDiagnostics1    +MyDentDiagnostics.ManageProgressNotesWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6?   -   5          �Name$  Window��5       � Title$% Administrar Notas de evolución��5      $��528.06��5      $��363.343��6    � Icon$1 +/MyDentDiagnostics;component/Images/app.ico��6"    � WindowStartupLocation$ CenterScreen=�6U   .��5
      � -   5         �Name$ 
LayoutRoot��5
      .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgProgressNotes��5      $��4��6,   $��10,10,10,44q�69      
IsReadOnly .    CanUserAddRows .     CanUserSortColumns . 	   CanUserDeleteRows	 .  
   AutoGenerateColumns
 .  ��6N   �� *ObservableData+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Número de folio��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.2* 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *ProgressNoteId+6a     5      $ Tipo de nota��6%   $ 0.25* 6T    6;   �� *Type+6T     5      $ Fecha y hora de creación��6%   $
 0.3* 6g    6G   �� *CreatedDate+6g     5      $ Usuario��6%   $
 0.2* 66    5      `� 5       �� StringFormat$  {0} {1}��6'   .1�5      ��  �� Path$ User.FirstName�6&   �� 5      $ User.LastName�6&   5      5      5      �� 5   
   -   5         ��Name$ btnAdd��5      $��Agregar��6!    � Cursor$
 Hand��63   $��6��6A   $��29��6N   $��Bottom=�6Z   $��Right=�6u   $��91��6�   $��	0,0,10,10q�6�   �� 5   
   -   5        $ btnExportToPDF��5      $��Exportar a PDF��6)   $��	10,0,0,10q�6B   $
 Hand��6U   $��5��6c   $��29��6p   $��Bottom=�6|   $
��Left=�6�   $	��111��6�   6�   5      5      