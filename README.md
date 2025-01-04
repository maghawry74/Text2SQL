# Text2SQL

## Introduction

Are you tired of writing SQL queries? Do you want to generate SQL queries from natural language? If yes, 
then you are at the right place. This project is about generating SQL queries from natural language.

## The Prompt Workflow
1. The user will provide a natural language query.
2. There is a tool that will retrieve the database schema info to enrich model knowledge context.
3. The model will generate the SQL query from the natural language query.
4. The generated SQL query will be executed on the database.
5. The result will be shown to the user.
6. The user can provide feedback on the result.