# CloudLibrary.Class

This is a technical test for Geeks

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





# Problem statement:

Assessment - Cloud Library 

The purpose of this project is to create a library for a company to create and maintain their cloud infrastructure efficiently without needing to have deep knowledge about different cloud providers. This library will introduce apis and abstractions that developers can use to design and implement cloud agnostic infrastructure. The client wants to start the first phase by supporting only virtual machines and database servers. But they would want more resources (i.e. load balancers, elastic file storage etc) at later stages. They want to be able to create both Windows and Linux instances. For database servers they want support for both MySQL and SQL Server. The client wants to be able to create multiple infrastructures as well, for example they want to create a UAT infrastructure for one project and a Test infrastructure for their internal team. 

The project is about creating a class library that provides interfaces for creating infrastructure resources.

Important:

The library should be .net standard
It should support creating and deleting infrastructures       
The library should have the implementation for a provider called IGS Cloud Services as the initial infrastructure provider but with an option to introduce new providers
The output of creating an infrastructure should be a sub-directory in the provider directory with the same name as the infrastructure with a sub-directory for each resource type. For each resource a file containing the attributes of that resource should be created in the [infrastructure/resource type] directory. example:
IGS/
UAT/
VirtualMachine/
UAT_SERVER.json
content: { property : value } 
The delete api should accept the name of an infrastructure and delete all the associated resources.
The implementation of deleting a resource is to delete the resource file on disk. 
Important: Do not delete the infrastructure folder. Dependency hierarchy should be followed, i.e. a virtual machine first should be deleted before deleting its hard disks.
Note: IGS is an imaginary cloud provider in this assessment.
