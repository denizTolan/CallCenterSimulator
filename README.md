# CallCenterSimulator

- Used Tecnology 
  - Entity Framework Core 
  - MediatR pattern 
  - RabbitMQ 
  - asp.net core 
  - Bidirectional Grpc Communication
  - Memory Cache

The project consists of 3 layers.

- Agent Layer
  - CallCenterSimulator.Agent.Api
  - CallCenterSimulator.Agent.Application
  - CallCenterSimulator.Agent.Data
  - CallCenterSimulator.Agent.Domain
- Client Layer
  - CallCenterSimulator.Client
  - CallCenterSimulator.Client.Api
  - CallCenterSimulator.Client.Domain
- Domain Layer
  - CallCenterSimulator.Domain.GrpcClient.Core
  - CallCenterSimulator.Domain.GrpcServer.Core
  - CallCenterSimulator.Domain.Core

Each layer have own business login and library.
  
  - Client Layer
    - I dint use poly between client and Call Center Client Api.I used Bidirectional GRPC communication tecnology between client and Call Center Client Api. 
  Because it would have a bad performance impact as the poly triggers the api every time.However  Grpc communication create bidirectional connection between client and client api.
  Because of that it would much performance than poly.
    - I used the Rabbitmq queue structure to transfer the user information to the agent service and I used the mediaR pattern to transfer the data.
  - Agent Layer
    - Agent Api consume Rabbitmq queue and get first user data from queue 
    - After get first data from queue , Agent service gets agent and team data from database
    - And Calculate available agent after then it send asinged data to client api with Rabbitmq queue
