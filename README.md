# notejam
## Problem Statement

Rehost and modernize notejam application in Azure cloud.

1. Must have requirements

    - Application must be highly available
    - Application must sustain variable and peak load
    - Support for backup and restore
    - Monitor application logs, metrics
    - Deploy DTAP with minimal efforts

## Current situation

![Current Situation](/Resources/currentsituation.png)

### Pain Points

- Self managed infrasturcture e.g servers, firewall
- User mangement
- Arrangements for disaster recovery
- Additional hardware/software requirements for monitoring/logging

## Solution

### Solution Apporach

There are multiple possibilities to implement the solution

1. Lift and shift i.e. replicate the on-premises infrastructure in cloud
2. Modernize the application to make use of functionality such as user management, independent database servers

This design is based on **2nd approach**.

### Design Principles

- SaaS over PaaS over IaaS
- Design for high availability
- Design for failover
- Least privileged access
- Appliction layer security (OSI Level 7)
- Infrastructure as a Code
- Focus on Cost, use the least Sku of the Azure resources

### Solution Overview

For non-azure audience here is the overview of the solution

![General Solution](/Resources/laymanview.PNG)

Here is the detailed technical diagram from MS Azure perspective

![Azure Solution](/Resources/technicaldiagram.png)

### Design Considertaion

1. **High Availability** - The application components such as web application and sql server are designed to be highly available i.e. the *web apps are created in 2 separate regions* such as north europe and west europe. The *SQL server is created with failover configuration*. The failover partner of sql server runs in different region than the primary sql server. This configuration handles the scenario in case one of the Azure region / data center fails.

2. **Scalability** - Current design makes use of all PaaS (Platform as a Service) resources. The PaaS *resource are fully managed by Microsoft*, those are *highly elastic* and can be scaled-up/down, scaled-out/in based on usage load. The web app can be scaled out upto 3 instances in case the CPU utilization is 70% for 10 minutes. These settings are highly flexible and can be adjusted as required. During the non-business hours or weekend the scale-down operation can be scheduled so that *infrasturcture becomes cost effecitve*

3. **Bakcup/Distaster Recovery** - As mentioned above, the PaaS resources are fully managed which means Microsoft takes *frequent backups of database* as well. Currently, the *backups are scheduled daily, weekly and monhtly*. The *retention period is set to 3 years* which makes it possible to *restore the 3 years old note* in case of accidental deletion of records or other disasters.

4. **Security** - The web applications are protected by *OSI Layer 7* security resource i.e. *Front Door*. Front door is configured with *WAF functionality* which helps to prevent various attacks like SQL injection, malicious BOT etc. Front door also act as an *global load balancer*. It enables *intelligent routing*. Intelligent meaning, it knows the preferred backend and can route maximum traffic (in this case 85%) to web app in primary region. In case of backend failure, all the traffic is routed to next available backend in this case failover web app. In addition to that, firewall rules on database server allow traffic originating from web application and effectively blocking any other access. Similary, web application has security configuration to allow traffic originating from Frontdoor endpoint and disabling the default host name.

5. **Monitoring** - Application telemetry, traces, exceptions, logs are fully consumed in *App Insights* which can also be connected to *Log Analytics workspace*. App Insights can send various *alerts in case of exceptions or application availability issues*. SQL servers are connected to Log Analytics workspace which advertise various *metrics* information.The logs can be easily fetched by writing simple queries against log analytics

6. **User Management** - This design makes use of *Azure Active Directory* for use management. With the use of AAD, application code is *free of handling user authentication and authorization*. All the users/groups from the organization can access the application with addtional overhead. The AAD comes with additional features such Single Sign-on, MFA.

7. **Infrastructure as Code** - The deployment is *fully automated*. It consists of *yaml pipeline (CI/CD)* which can be run from *Azure DevOps* to provision the resources in Azure. The resources are grouped in a container called as *Resource Group*. The deployment can be rolled out to *D/T/A/P environment* without making any code change. The code can be executed locally in case there is no Azure DevOps environment. (Please exevcute main.ps1). The prerequisite is - Azure subscription.

8. **Administration** - At the resource group level, certain *tags are defined* which helps to *send the billing invoices to cost center*, to identify the application name for which the resources are provisioned and application owner.

Check the Solution design presentation located at [Design Document](/Documentation/notejam.pptx)

## How do I deploy?

There are two ways

1. Impor the source code and yaml pipeline in Azure DevOps repo. Create the service connection from Azure DevOps to your Azure subscription. Execute the pipeline.
2. The source code can be executed as a stand alone trigger. Execute the main.ps1 file. Either use default parameters or use your own.
