# 📚 Course API

API RESTful para gerenciamento de cursos, estudantes e matrículas.

---

## 🚀 Tecnologias

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* ASP.NET Identity
* JWT Authentication
* Swagger

---

## ▶ Como rodar

```bash
dotnet build
dotnet ef database update
dotnet run
```

Acesse:

[https://localhost:xxxx/swagger](https://localhost:xxxx/swagger)

---

## 🔐 Autenticação

1. Faça login em:
   POST /auth/login

2. Copie o token retornado

3. No Swagger:
   Clique em "Authorize"

Digite:
Bearer SEU_TOKEN

---

## 📡 Endpoints principais

### Courses

* GET /courses → público (com paginação/filtro)
* POST /courses → Admin/Instructor
* PUT /courses/{id} → Admin/Instructor
* DELETE /courses/{id} → Admin

---

### Students

* GET /students → Admin
* GET /students/{id} → Admin ou dono
* GET /me → usuário autenticado
* POST /students → Admin

---

### Enrollments

* POST /enrollments → autenticado
* GET /students/{id}/enrollments → Admin ou dono
* DELETE /enrollments/{id} → cancelar

---

## 📊 Paginação

Exemplo:

GET /courses?page=1&pageSize=10

---

## ⚠️ Erros

A API retorna erros no padrão ProblemDetails:

```json
{
  "title": "Curso não encontrado",
  "status": 404
}
```

---

## 🔑 Roles

* Admin
* Instructor
* Student

---

## 🧪 Testes

Use Swagger ou Postman.
