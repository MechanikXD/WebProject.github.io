# WebProject
Simple website to solve Systems of Linear Equations.
Site provides ability to solve systems of linear equations using gaussian elimination, rigister/login/logout and store solutions.
Some restrictions were apallied to easer the task: max of 10 equations can be inputed and minimum of 2, equations cannot contain NaN or matrix cannot be singular.
Data validations exists both on front end and backend. No data validation for usernames and passwords are currently inplemented...
Each user can store up to 100 solutions at a time. Passwords are hashed before storing in database.

# Frontend
Frontend created using simple html/css and JavaScript.
Support Authorization/Authentication using JWT Tokens.
# BackEnd & DataBase
All frontend request are send to NGinx proxy server who works as load balancer round dropping between 2 servers
Servers are the one who accepting and processing requests, and the one who works with DataBase.
Servers created using Asp.Net and EntityFramework to work with DB.
DataBase is running on PostGreSQL.
