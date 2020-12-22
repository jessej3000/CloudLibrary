# CloudLibrary.Class
Theoretical implementation of cloud management.

# Description

Main project: CloudLibrary project - A .NET standard Class Library consist of interrelated objects to be used to manage the structure of a cloud.
Supporting app: CloudManager project - A console application that consumes and demonstrate CloudLibrary APIs.

# Setup

1. Clone the whole solution
2. Make sure to to have CloudLibrary project as a dependency in CloudManager project.
3. You may need Visual Studio to open the solution (MAC or Windows) will do.

# Notes

You may see the folders and files created in the bin directory of the application. In this case CloudManager.
Everytime you create a provider, virtual machine, or database. A related folder or file will be created.
The code can still be improved if I still have time. Probably using dependency injections and generics.
But for now it works as expected.

