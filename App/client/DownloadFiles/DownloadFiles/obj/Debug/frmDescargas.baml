   M S B A M L   `   `   `  ����H  DDownloadFiles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:DownloadFilesDownloadFiles  5   	        DownloadFiles.frmDescargas   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:DownloadFiles     5   	   -   5          � Title$  Descarga de ficheros��5   	   $	��350��6&   $��728.92��63   .��5	      � .+�5
      � $
��Left=�6	   $	��178��6$   $��	23,19,0,0q�61   $	��Top=�6D   $	��677��6\   .+�5      �� -   5         ��Name$ 
lblUrlJson��5      $��	URL Json:��6   $
��Left=�63   $��25��6N   $��8,17,0,0q�6Z   $	��Top=�6l   $��98��6�   �� 5      -   5        $ 
lblDestino��5      $��Carpeta destino:��6   $
��Left=�6:   $��25��6U   $��8,59,0,0q�6a   $	��Top=�6s   $��98��6�   �� 5      -   5         ��Name$ 
txtUrlJson��5      $
��Left=�6!   $��21��6<   $��
118,19,0,0q�6H    �� TextWrapping$
 Wrap=�6\   $�� ��6p   $	��Top=�6x   $	��538��6�   �� 5      -   5        $ 
txtDestino��5      $
��Left=�6!   $��21��6<   $��
118,61,0,0q�6H   $
 Wrap=�6\   $�� ��6p   $	��Top=�6x   $	��498��6�   �� 5      -   5         ��Name$ 	btnInicio��5      $��Inicia la descarga��6   $
��Left=�6<   $��27��6W   $��144,130,0,0q�6c   $	��Top=�6x   $	��179��6�   6�   �� 5      -   5        $ btnRecuperar��5      $��Recuperacion de fallos��6"   $
��Left=�6C   $��27��6^   $��388,128,0,0q�6j   $	��Top=�6   $	��179��6�   {�.  6�   �� 5      -   5         ��Name$ 
chkErrores��5      $D��>Provocar errores (uno de cada diez ficheros no se descargará)��6"   $
��Left=�6j   $��
118,95,0,0q�6�   $	��Top=�6�   5      � 5      $
��Left=�6	   $��79��6$   $��
40,197,0,0q�60   $	��Top=�6D   $	��639��6\   .+�5      � -	   5         �Name$ pbarProgeso��5      $
��Left=�6&   $��12��6A   $��	20,10,0,0q�6M   $	��Top=�6`   $	��601��6x   �� 5      -
   5        $ txtProcesando��5      $
��Left=�6$   $��19��6?   $��	21,45,0,0q�6K   $
 Wrap=�6^   $	��Top=�6r   $	��598��6�   $��11(�6�    �� 
IsReadOnly . 6�   �� 5      -   5        $ lblProcesando��5      $��Descargando...��6"   $
��Left=�6;   $��25��6V   $��	14,22,0,0q�6b   $	��Top=�6u   $��89��6�   $��11(�6�   5      5      5      