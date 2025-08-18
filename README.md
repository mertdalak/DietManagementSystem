# Diet Management System

Proje Hakkında

Diet Management System, kullanıcıların diyet planlarını ve öğünlerini yönetebileceği bir .NET 8 Web API uygulamasıdır.
Sistem, Swagger/OpenAPI ile dokümante edilmiştir ve veriler Entity Framework Core üzerinden SQL veritabanında tutulur.

# Kurulum
Gereksinimler

.NET 8 SDK

SQL Server 

Visual Studio 2022

# API Dokümantasyonu

Swagger arayüzü şu adreste otomatik olarak açılır: http://localhost:5000/swagger

# Database şema 

# User (Identity):
•	Id (PK, Guid), FullName, Email, Role (enum), Identity fields...

•	1-to-1 with Dietitian (User has DietitianProfile)

# Dietitian:
•	Id (PK), FullName, Specialization, UserId (unique), CreatedAt, UpdatedAt, IsDeleted, DeletedAt

•	Relations: 1-to-1 with User; 1-to-many Clients; 1-to-many DietPlans; 1-to-many ClientProgress

# Client:
•	Id (PK), FullName, InitialWeight, CurrentWeight?, DietitianId (FK), audit fields

•	Relations: many-to-1 Dietitian; 1-to-many DietPlans; 1-to-many ClientProgress

# DietPlan:
•	Id (PK), Title, StartDate, EndDate, InitialWeight, ClientId (FK), DietitianId (FK), audit fields

•	Relations: many-to-1 Client; many-to-1 Dietitian; 1-to-many Meals

# Meal:
•	Id (PK), Title, StartTime, EndTime, Contents, DietPlanId (FK), audit fields

•	Relations: many-to-1 DietPlan

# ClientProgress:
•	Id (PK), RecordDate, Weight, BodyFatPercentage?, MuscleMass?, Notes?, ClientId (FK), DietitianId (FK), audit fields

•	Relations: many-to-1 Client; many-to-1 Dietitian

# RefreshToken:
•	Id (PK), Token, ExpiryDate, IsUsed, IsRevoked, UserId (FK), audit fields


# Endpoint documentation with request/response examples
# Authentication
# Register: POST /api/auth/register
•	Body:
    {
      "fullName": "Admin User",
      "email": "admin@example.com",
      "password": "P@ssw0rd!",
      "confirmPassword": "P@ssw0rd!",
      "role": "Admin"
    }

  •	200:

  { "token": "jwt...", "refreshToken": "..." }

# Login: POST /api/auth/login
•	Body:
  { "email": "admin@example.com", "password": "P@ssw0rd!" }
  •	200:
  { "token": "jwt...", "refreshToken": "..." }
# Dietitian 
•	POST /api/dietitian (Admin)
  •	Body:
  
  { "fullName": "Mert Dalak", "specialization": "Personal Trainer", "userId": "GUID-OF-USER" }
  
  •	200:
  
  { "isSuccess": true, "message": "Dietitian created successfully" }
  
•	PUT /api/dietitian/{id} (Admin)
  •	Body:
  
  { "fullName": "Mert D.", "specialization": "PT", "userId": "GUID-OF-USER" }
  
  •	200:
  
  { "isSuccess": true, "message": "Dietitian updated successfully" }
  
•	DELETE /api/dietitian/{id} (Admin)

  •	200:
  
  { "isSuccess": true, "message": "Dietitian deleted successfully" }
  
•	GET /api/dietitian/{id} (Admin or owner dietitian)

  •	200:
  
  { "id": "GUID", "fullName": "Mert D.", "specialization": "PT", "userId": "GUID", "isDeleted": false }
  


# Yaygın Responselar ve Hatalar

•	Success (command handlers):
•	{ "isSuccess": true, "message": "..." }

•	Not Found:
•	404 with message (e.g., "Diet plan with ID ... not found")

•	Forbidden:
•	403 with message ("You don't have permission to ...")

•	Unauthorized:
•	401 with message ("Invalid user authentication")

•	Bad Request:
•	400 with message (validation errors or domain rules)


# Detaylar
•	Loglama: Serilog "/diet-management-*.txt" şeklinde loglama yapar.

•	Validation: FluentValidation, pipeline behavior aracılığıyla entegre edilmiştir. Doğrulama mesajları, middleware’iniz tarafından 400 hata olarak döndürülür.

•	Roller: Program.cs başlangıçta rolleri tanımlar. Kullanıcıları Auth endpointleri üzerinden oluşturmalı ve kayıt sırasında rolleri atamalısınız.

•	Filtreli Silme: Birçok entity IsDeleted/DeletedAt değerlerini kullanır böylece kayıtlar gerçekten silinmez kullanılmaz olur; sorgular silinmiş kayıtları filtreler.

  

  

  
