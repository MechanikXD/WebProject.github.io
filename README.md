# Web Project
Simple website to solve Systems of Linear Equations.
Site provides ability to solve systems of linear equations using Gaussian elimination, register/login/logout and store solutions.
Some restrictions were applied to easier the tasks: max of 10 equations can be input and minimum of 2, equations cannot contain NaN or matrix cannot be singular.
Data validations exist both on front end and backend. No data validation for usernames and passwords is currently implemented...
Each user can store up to 100 solutions at a time. Passwords are hashed before storing in database.

# Frontend
Frontend created using simple html/css and JavaScript.
Support Authorization/Authentication using JWT Tokens.
# Backend & Database
All frontend request are sent to Nginx proxy server who works as load balancer round dropping between 2 servers
Servers are the one who accepting and processing requests, and the one who works with Database.
Servers created using Asp.Net and Entity Framework to work with DB.
Database is running on PostgreSQL.
