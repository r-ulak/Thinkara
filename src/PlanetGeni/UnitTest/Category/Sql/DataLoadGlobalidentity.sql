SET SQL_SAFE_UPDATES =0;
Delete From AspNetUsers;
Delete From AspNetRoles;
Delete From AspNetUserClaims;
Delete From AspNetUserLogins;
Delete From AspNetUserRoles;

 LOAD DATA LOCAL INFILE '{0}AspNetUsers.tsv' INTO TABLE AspNetUsers FIELDS TERMINATED BY '\t' ;
  UPDATE AspNetUsers SET UserName = REPLACE(REPLACE(UserName, '\r', ''), '\n', '');
