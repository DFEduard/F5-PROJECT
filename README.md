# F5-PROJECT
F5-PROJECT-BACKEND

Backend build. 
MS SQL Server, C#, Web API, REST Services

# APIS
- Get access token and refresh token:
  - POST: {{base_url}}/token/
  - Payload: {
    "email": "{{user email}}",
    "password": {{user password}}
  }
  - - Return: access token (lifetime 5 minutes) refresh token (used to get a new access token)
  
- Get new access token
  - POST: {{base_url}}/token/refresh/
  - Payload: {
    "refresh": "{{refresh token here}}"
  }
  - Return: new access token (lifetime 24 hours)
  
  
- Create user: 
  - POST: {{base_url}}/api/users
  - Payload: {
    "email": "{{email here}}",
    "password": "{{password here}}",
    "name": "{{name here}}",
    "title": "{{job title here}}",
    "country": "{{country here}}"
}
  - Return: user data

- Get users: 
  - GET: {{base_url}}/api/users
  - Return: list of users
  
- Get user: 
  - GET: {{base_url}}/api/users/{{user_id}}/
  - Returns: user
  
- Update user: 
  - PUT: {{base_url}}/api/users/{{user_id}}/
  - Returns: updated user
  
- Delete user: 
  - DELETE: {{base_url}}/api/users/{{user_id}}/
  - Returns: 200
 
