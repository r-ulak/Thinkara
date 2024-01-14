if NOT %1 == 0 GOTO COPYFOLDER
echo %1 name must be provided. backup.bat <folderName>
GOTO END
:COPYFOLDER
DEL *.tsv /Q /F
copy %1\*.tsv /Y
"C:\Program Files\MySQL\MySQL Workbench 6.2 CE\mysql" -h UbuntuServer1 -u root -psqlapps planetgeni < restore.sql 
:END