# Planning Poker Backend
Next information applies to **server-side** of **Planning Poker** project. Information about **client-side** application you can find in [Poker Planning Android repository](https://github.com/bardss/TSD-PokerPlanning-Android).
## Tools
List of tools, needed to develop the application:
 - [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/windows)
 - [Visual Studio 2017](https://www.visualstudio.com/ru/downloads/)
 - [IIS Express 10.0](https://www.microsoft.com/en-us/download/details.aspx?id=48264)
 - [SQL Express 2016](https://www.microsoft.com/en-us/download/details.aspx?id=54284)
 - [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017)
 - [Postman](https://www.getpostman.com) (optional)
 - Windows 7 or later / Server 2012 or later

## Project configuration
**Connection string configuration**
 1. In the root directory find *PlanningPokerBackend.sln* and open it using Visual Studio.
 2. Open *PlanningPockerBackend project > Startup.cs*. 
 3. In `ConfigureServices` method find next:

> services.AddDbContext<PlanningPokerDbContext>(opt => opt.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PlanningPokerDb;Trusted_Connection=True;"));

 4. Change connection string to yours.

## Running Tests
Tests are located in *PlanningPokerBackend.Tests* project.

1. Open Test Explorer (go to **Tests > Windows > Test Explorer**)

![enter image description here](https://lh3.googleusercontent.com/meQysFmz7SWFgC__n_JTwxGYdKRjRogrzJdctNp4gGXqNn54WMGB0JBZ0ZqgDNDCW4cQBMFZWMsG)

2. Build solution (`F6`).
3. Click **Run All**

![enter image description here](https://lh3.googleusercontent.com/t2oMrNOrAcAByyEUKcJ06ORYdVXKa9yLWrJxjNoNJdcUcJKLlV-IMHHmYWOdLeDdwped_TXqh0Tn)

Also you can run specific tests by selecting them from list, right clicking on them and selecting **Run Selected Tests** from context menu.

## Deployment
1. Firstly, make sure you have installed all the listed tools.
2. Clone repository to your computer from **production** branch. Unzip the project.
3. Depending on what Windows do you have, follow steps, described [here](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.0&tabs=aspnetcore2x#iis-configuration) (IIS configuration section), to configure your OS.
4. Install .NET Core Hosting Bundle

    1. [Download](https://www.microsoft.com/net/download/thank-you/dotnet-runtime-2.1.0-rc1-windows-hosting-bundle-installer) and install .NET Core Hosting Bundle.

    2. Open command prompt and execute:
    
    > **net stop was /y**
    >
    > **net start w3svc**

5. Create site and configure IIS

    1. Open **IIS Manager**.
    2. Add website
    
    ![enter image description here](https://lh3.googleusercontent.com/PFbeanuablBMSFT_H8OJ5moBgBBuOVfO9dF2dqvSqh39xXeRGT3UnEmIycdfQsa9K3uc2ZIPdk8R)  
    
    3. Specify **Site name**.
    4. Create folder, where you want to publish your website. Specify path to this folder in **Physical path**.
    ![enter image description here](https://lh3.googleusercontent.com/qsMl2DuIVu-pQSNqs9NJUu9SIE9xJrjQ5YH71lE-lxh2dwuAM_HMm91QS9hBmyl5BBtubl2ovn0e)
    5. Specify **IP Address** and **Port**. If port 80 is used, try to use 8080.
    6. Click **Ok**.
    6. In **Application Pools** node select your website and change **.NET CLR version** to **No Managed Code** and click **Ok**
    ![enter image description here](https://lh3.googleusercontent.com/buiKoVt-_uTkOYUxxX-jOnGAblztgHosxRW_POTKQTiCp2WIE3CKRCLUda_EBj8L3583QI9l9GoF)

6. Create SQL Login
    1. Open **SQL Server Management Studio**. Connect to SQL Express server using **Windows Authentication**.
    2. Go to **Object Explorer > (your_localdb)\SQLEXPRESS > Security > Logins**. Find **sa** user. Right click mouse **> Properties**.
    3. In tab **General** specify your password.
    4. In tab **Status** set **Login** - **Enabled**.

    ![enter image description here](https://lh3.googleusercontent.com/v8jH7mlUs6lEo4bFIbR5e4pThK3pGdJ-m_CGV-MwnIT1S0Crw-D-4zZQnoz9gbth263Dvz0dj5fD)

    5. Click **Ok**.
    6. Open properties of SQL Express Server

    ![enter image description here](https://lh3.googleusercontent.com/N2QgMc57ZDU4OlFMn95hp4t7zNN5vxqXWXLg5rkPNAIbT9PS9L_MCeN7BJp4botFsuLSNg1vWjzi)

    7. Open **Security** tab. Change **Server authentication** to **SQL Server and Windows Authentication mode**.
    8. Click **Ok**.

    ![enter image description here](https://lh3.googleusercontent.com/GALj3aaaHi-ZiCptxXfMe-1HBdpjyXNuuQwjNjtC2X_zVz30aykd6p_skqkx88fS9-YXiMTstc87)

    9. Restart your SQL server instance

    ![enter image description here](https://lh3.googleusercontent.com/VOrvA98F0D1uLBtfM8jTyGTsScgD6QjlnBiOPihBnpymHePiJayTZcgYumfCzdGNW19jqqc5f1EH)

7. Open project in Visual Studio. Change connection string for `PlanningPokerDbContext` to:

> Server=.\SQLEXPRESS;Database=PlanningPokerDb;User Id=sa;Password=**(your_password)**;

8. Deploy project from Visual Studio
    1. Open project with Visual Studio
    2. Change build mode to **Release**

    ![enter image description here](https://lh3.googleusercontent.com/I_-x7bivDInfH1PUWyjGZSrY00QC1anmCTS_xxEvO3a0I-IL-JMMMFl5ne_W5QmnLxr14u3FiQpl)

    3. Build project (press `F6`).
    4. Click **Publish...**. Click **Create new profile**.

    ![enter image description here](https://lh3.googleusercontent.com/aPwnvOpOCOCIObUBOkTAOgBTKcpyxXEVw4AkQHk3dw62l13LkIFVs3I_AHXeUsNbuX--epPmMc6x)

    5. Select **Folder** and specify folder of your website

    ![enter image description here](https://lh3.googleusercontent.com/8IhB1K7-AzvvV3NYFfGWntQdYso2KIkGAuIFZxkRYHonhsAW-MlUV0XIIfV3LHtHywBl4xLp2lBq)

    6. Click **Publish**.
9.  Open inbound port
    1. Go to **Control panel > Windows Defender Firewall > Advanced settings (on left side)**. Open tab **Inbound rules**.
    2. Click **New Rule... (on right side)**
    3. Select **Port**

    ![enter image description here](https://lh3.googleusercontent.com/39qwaj4t9tNBdH1ZneFluvMdG67Qrq9SzY86Wl3aLX2tQNsqZ8UC75adNj3wTj7JbVJpQm9nAC2C)

    4. In **Specific local ports:** specify port, which uses your website. Click **Next**. 
    5. Select **Allow the connection**. Click **Next**. 
    6. Check **Domain, Private and Public**. Click **Next**.
    7. Specify some name for rule and **Save**.
    8. Restart OS.

In case of some troubles, you can try to find solution on [Troubleshoot ASP.NET Core on IIS](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/troubleshoot?view=aspnetcore-2.0) page.
