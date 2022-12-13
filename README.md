# GuideBook

REQUIREMENTS:

* .NET CORE
* RABBITMQ
* MONGODB

You must run four projects at the same time(
	GuideBook.ContactApi, 
	GuideBook.PersonApi, 
	GuideBook.ReportApi, 
	ReportService Worker)

4 projects must be startup projects. RabbitMQ must be queue name "Report".  

The first report request is created with the url below

http://localhost:22634/CreateReportByLocation/Ayd%C4%B1n%20/adasd.com


Person Api

https://localhost:7006/swagger/index.html

You must run four projects at the same time(Person Api, Contact Api, Report Api ve ReportService Worker)

Person Api

![image](https://user-images.githubusercontent.com/119305359/205001677-2d9bf047-654e-477c-accc-5378500a1383.png)

Contact Api

http://localhost:5048/swagger/index.html

![image](https://user-images.githubusercontent.com/119305359/205001535-cc2b7b0e-be63-4383-bb1d-a353a62c5920.png)

Report Api

http://localhost:22634/swagger/index.html

![image](https://user-images.githubusercontent.com/119305359/205001843-37779128-b3cc-430e-a50d-07d520cd2656.png)


