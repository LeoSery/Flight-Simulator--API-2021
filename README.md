# Infra project - Flight simulator API
project made with [@Tractorou24](https://github.com/Tractorou24) and [@Tartoo](https://github.com/Tartoo) for the end of year project during the bachelor year 2 (2021/2022).

## Summary :

- [Presentation](##Presentation-:)
- [How to open the project](##How-to-open-the-project-:)
- [How to run the project](##How-to-run-the-project-:)
- [How to run the Unit tests](##How-to-run-the-Unit-tests-:)
- [How to automatically deploy the solution](##How-to-automatically-deploy-the-solution-:)

## Presentation :

This project is based on [our development project](https://github.com/LeoSery/Flight-Simulator-Unity3D) for our end of bachelor 2 at Ynov.

This project is a solution to set up an API accessible directly from the game, allowing access for example to our list of planes or to the results of the matches made in the game.

The project contains:
 - An **API** accessible by **URL**.
 - A GitHub **pull request** system.
 - A **Linter commit** for GitHub.
 - A **CICD system** integrated into the project.
 - **Unit tests**.
 - An automatic deployment system with **GitHub secrets**.


## How to open the project :

1 - Clone the git repository to your computer with the following command :
```
git clone git@github.com:LeoSery/Flight-Simulator-Unity3D.git
```
or 
```
git clone https://github.com/LeoSery/Flight-Simulator-Unity3D.git
```

2 - Open the solution with **Visual Studio 2022**.

## How to run the project : 

```
dotnet run
```
or click on **"Local Windows Debugger"** button in Visual Studio.

## How to run the Unit tests :

```
dotnet test
```
or use the **"Test Explorer"** in Visual Studio.

## How to automatically deploy the solution :

1. **Fork** the GitHub repository.
2. Setup the **GitHub secrets** :
    - APP_SETTINGS_FILE : appsetting.json > database creditencials
    - REMOTE_HOST : Server IP or Domain name
    - REMOTE_PORT : Server SSH port
    - REMOTE_SSH_KEY : Private Key for server access.
    - REMOTE_TARGET : API files folder
    - REMOTE_USER : SSH User
    
4. Add the "fsapi" **service** to execute `dotnet run FlightSimulator.dll` inside the remote target.
5. Setup Apache **reverse proxy** in **port 5000**.