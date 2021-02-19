# notejam
This repo creates the infrastructure of 3 tier architecture application

### Solution Apporach
There are multiple possibilities to implemet the solution
1. Lift and shift i.e. replicate the on-premises infrastructure in cloud
2. Modernize the application to make of functionality such as user management, SSO

This design is based on 2nd approach, the infrastructure is created using IaC (Infra as Code) principle 

Assumption - The identity under which script/code run, has enough permissions to create service principal in AAD.
