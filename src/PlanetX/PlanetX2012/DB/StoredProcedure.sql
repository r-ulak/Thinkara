DROP PROCEDURE IF EXISTS planetx.GetFinanceContent;
 delimiter //
 CREATE PROCEDURE planetx.GetFinanceContent
    (
         IN  webUserId INT
         
    )
BEGIN
 SELECT 
 (SELECT SUM(AMOUNT) 
    FROM PlanetX.Card 
    WHERE UserId = webUserId and CardType=1) creditTotal
    ,
 
 (SELECT SUM(AMOUNT) 
    FROM PlanetX.Card 
    WHERE UserId = webUserId and CardType=2) debitTotal
    ,
    
 (SELECT SUM(LOANAMOUNT)  
    FROM PlanetX.LoanFromBusiness
    WHERE UserId = webUserId) businessLoanTotal
    ,
 
 (SELECT SUM(LOANAMOUNT) 
    FROM PlanetX.LoanFromPerson
    WHERE UserId = webUserId) personLoanTotal;
    
 END;//
 
DROP PROCEDURE IF EXISTS planetx.GetProfileSummary;

 CREATE PROCEDURE planetx.GetProfileSummary
    (
         IN  webUserId INT
         
    )
 BEGIN
 SELECT 
 (SELECT count(*) 
    FROM planetx.Friend 
    WHERE UserId = webUserId ) FriendCount
    ,
 (SELECT count(*) 
    FROM planetx.Profile 
    WHERE UserId = webUserId ) ProfileCount;

    
 END;//
 
DROP PROCEDURE IF EXISTS planetx.GetFriends;

 CREATE PROCEDURE planetx.GetFriends
    (
         IN  webUserId INT         
    )
 BEGIN
 SELECT FriendUserId 
    FROM planetx.Friend 
    WHERE UserId = webUserId; 
 END;//