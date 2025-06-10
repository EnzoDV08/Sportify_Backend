![Sportify Header Image](./ReadMEAssets/ReadMeFile.png)
<br>
- - - 
![GitHub repo size](https://img.shields.io/github/repo-size/EnzoDV08/Sportify_Backend?color=%000000)
![GitHub watchers](https://img.shields.io/github/watchers/EnzoDV08/Sportify_Backend?color=%000000)
![GitHub language count](https://img.shields.io/github/languages/count/EnzoDV08/Sportify_Backend?color=%000000)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/EnzoDV08/Sportify_Backend?color=%000000)

<h5 align="center" style="padding:0;margin:0;">Zander Bezuidenhout 221205</h5>
<h5 align="center" style="padding:0;margin:0;">Enzo De Vittorio 231244</h5>
<h5 align="center" style="padding:0;margin:0;">Jaco Mostert 231008</h5><h5 align="center" style="padding:0;margin:0;">Joshua De Klerk 231207</h5>
<h6 align="center">DV 300 2025</h6>
</br>
<p align="center">
  <h3 align="center">Sportify Backend</h3>

  <p align="center">
    Backend for a sport event management app enabling users to create, join, and manage sport activities.
   <br />
   <br />
    <a href="https://github.com/EnzoDV08/Sportify_Backend/issues">Report Bug</a>
    ·
    <a href="https://github.comEnzoDV08/Sportify_Backend/issues">Request Feature</a>
</p>

## Table of Contents

* [About the Project](#about-the-project)
  * [Project Description](#project-description)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [How to install](#how-to-install)
* [Features and Functionality](#features-and-functionality)
* [Swagger Live Link](#swagger-live-link)
* [Development Process](#development-process)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)
* [Acknowledgements](#acknowledgements)

## About the Project

### Project Description

This is the backend for Sportify, a sports event platform that lets users create events, manage participants, invite friends and track achievements.

### Built With

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Google Auth](https://img.shields.io/badge/Google_Auth-4285F4?style=for-the-badge&logo=google&logoColor=white)](https://developers.google.com/identity)



## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)

### How to install

Since the backend is already deployed and running live, there's no need to run it locally.

However, to use the project (e.g., frontend or other clients), you **must install all dependencies**:

1. Clone the repository:
   ```sh
   git clone https://github.com/EnzoDV08/Sportify_Backend
2. Navigate into the frontend directory (or root if applicable):
   ```sh
   cd SportifyApi
3. Install all dependencies:
   ```sh
   npm install
4. Create a .env file in the root of your project and add the following environment variables:
   ```sh
   AIVEN_HOST=sportifyapi-sportifyapi.f.aivencloud.com
   AIVEN_PORT=24496
   AIVEN_DATABASE=defaultdb
   AIVEN_USERNAME=avnadmin
   AIVEN_PASSWORD=your_aiven_password_here
   AIVEN_SSLMODE=Require
   Note: The actual AIVEN_PASSWORD is not included for security reasons.
   Please contact the project maintainer to request access credentials.

⚠️ Keep your .env file private — do not commit it to version control.
## Features and Functionality

- **AuthController** – Authentication with optional 2FA  
- **UsersController** – User account management  
- **ProfilesController** – Editable profiles with interests, sports, and bio  
- **EventsController** – Create, update, view, and delete events  
- **EventParticipantsController** – Join events, approve/reject requests  
- **FriendsController** – Send, accept, and remove friend requests  
- **OrganizationsController** – Manage organizational accounts  
- **UploadController** – Upload and retrieve images  
- **UserAchievementsController** – Award achievements to users  

## Swagger Live Link 
 <a>
    <img src="../Sportify_Backend/ReadmeAssets/Screenshot 2025-06-10 143459.png" >
  </a>

[Swagger Live Link](sportify-backend-znri.onrender.com/swagger/index.html)
## Development Process

### Implementation Process

- Clean API structure using ASP.NET MVC  
- Dockerized environment for consistent setup  
- PostgreSQL for data persistence  
- Image upload via controller endpoints  

#### Highlights

- Achievement system
- Working invitation system  
- Full CRUD for events and profiles  

#### Challenges

- Complex participant approval logic  
- Image upload and path resolution  


## Reviews & Testing

### Unit Tests

- Controller tests for events, user, profiles, achievement and participants  
- Authentication flow tests including 2FA  

---

## Future Implementation

- Implement for organizations to make events
- Chat features between friends
- Mobile app integration  


## Contributing

1. Fork the repository  
2. Create your feature branch  
3. Commit your changes  
4. Push to GitHub  
5. Open a pull request  

---

## Authors

- [Zander Bezuidenhout](https://github.com/ZanderBez)
- [Enzo De Vittorio](https://github.com/EnzoDV08)
- [Jaco Mostert](https://github.com/321008Jaco)
- [Joshua De Klerk](https://github.com/JoshuaDeKlerk)

---

## License

Distributed under the MIT License.

---

## Acknowledgements

- [ASP.NET Docs](https://learn.microsoft.com/en-us/aspnet/core/)  
- [Docker Docs](https://docs.docker.com/)  
- [PostgreSQL Docs](https://www.postgresql.org/docs/)  
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
   