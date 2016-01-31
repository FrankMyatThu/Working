1.Using microsoft sql server, create database named as "testscript".
2.Run below two script files which also included in zip file.
	-1.ToRun.TestScript.CreateTables.sql
	-2.ToRun.TestScript.TestData.sql
3.Using visual studio, open the solution file named "TestScript.sln".
4.Open web.config file which is under project named "TestScript.Service". And update the connection string accordingly.
5.Open app.config file which is under project named "TestScript.Model". And update the connection string accordingly.
6.Press start button from visual studio or press f5 to start the whole solution. Normally, below two projects will run simultaneously.
	-TestScript.Client (https://localhost:44310/Order/List)
	-TestScript.Service (https://localhost:44309/)