Can just build in VisualStudio, run the project or tests as seen appropriate.

If no Visual Studio:

Navigate to the TransPerfectWeb solution in a command line prompt:

To build: 
dotnet build TransPerfect.sln

To test:
dotnet test TransPerfect.sln

To run:
dotnet run TransPerfect.sln
	You should be listening in on some port like https://localhost:5001 or https://localhost:5000
	From here you can just open a browser and access the commands here:
	https://localhost:5001/api/QRReader/GetQRImage
	https://localhost:5001/api/QRReader/GetQRMessage
