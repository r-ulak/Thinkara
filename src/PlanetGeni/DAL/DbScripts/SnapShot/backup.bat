if NOT %1 == 0 GOTO CHECKFolder
echo %1 name must be provided. backup.bat <folderName>
GOTO END
:CHECKFolder
IF EXIST %1 GOTO START
mkdir %1
:START
"C:\Program Files\MySQL\MySQL Workbench 6.2 CE\mysql" -h UbuntuServer1 -u root -psqlapps planetgeni < CountryBudget.sql  >  %1\CountryBudget.tsv
"C:\Program Files\MySQL\MySQL Workbench 6.2 CE\mysql" -h UbuntuServer1 -u root -psqlapps planetgeni < CountryBudgetByType.sql  > %1\CountryBudgetByType.tsv
:END

