# CourseApi

## Endpoints da API

### Autenticação

#### Registrar usuário

POST /auth/register

Body:

```json
{
  "email": "user@email.com",
  "password": "123456"
}
```

Resposta:

* 200 OK → usuário criado
* 400 Bad Request → erro de validação

---

#### Login

POST /auth/login

Body:

```json
{
  "email": "user@email.com",
  "password": "123456"
}
```

Resposta:

```json
{
  "token": "JWT_TOKEN_AQUI"
}
```

---

### Cursos

#### Listar cursos

GET /courses

Query params:

* pageNumber
* pageSize
* category (opcional)

---

#### Criar curso (Admin/Instructor)

POST /courses

Headers:
Authorization: Bearer {token}

Body:

```json
{
  "title": "Curso de C#",
  "description": "Aprenda C#",
  "category": "Programação",
  "workloadHours": 40
}
```

---

#### Atualizar curso

PUT /courses/{id}

---

#### Remover curso (Admin)
DELETE /courses/{id}

---

### Estudantes

####Criar estudante (Admin)

POST /students

---

#### Listar estudantes (Admin)

GET /students

---

#### Atualizar estudante

PUT /students/{id}

---

### Matrículas

#### Matricular em curso

POST /enrollments

---

#### Listar minhas matrículas

GET /enrollments/me

---

## Autenticação

A API utiliza JWT Bearer Token.

Para acessar endpoints protegidos:

1. Faça login em /auth/login
2. Copie o token retornado
3. Envie no header:

Authorization: Bearer SEU_TOKEN
