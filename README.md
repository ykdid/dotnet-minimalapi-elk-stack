# .NET Minimal API with ELK Stack

This project demonstrates how to implement a .NET Minimal API with ELK (Elasticsearch, Logstash, Kibana) Stack integration for logging and monitoring. The application provides a simple endpoint to receive log data and store it in Elasticsearch, which can then be visualized using Kibana.

## Technologies Used

- .NET 9.0
- NEST (Elasticsearch .NET Client)
- Serilog
- Docker
- ELK Stack
  - Elasticsearch 7.17.9
  - Kibana 7.17.9

## Project Structure

- `MinimalApiElk/`: Main API project
  - `Models/`: Contains data models
  - `Program.cs`: Application entry point and API configuration
  - `Dockerfile`: Container configuration for the API
- `docker-compose.yml`: Docker compose configuration for all services

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Getting Started

1. Clone the repository:
```bash
git clone <repository-url>
cd dotnet-minimalapi-elk-stack
```

2. Start the services using Docker Compose:
```bash
docker compose up -d
```

3. Wait for all services to start (this might take a few minutes for Elasticsearch and Kibana)

4. The following services will be available:
- API: http://localhost:5001
- Elasticsearch: http://localhost:9200
- Kibana: http://localhost:5601

## API Endpoints

### POST /api/logs
Accepts log data and stores it in Elasticsearch.

Example request:
```bash
curl -X POST http://localhost:5001/api/logs \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Test log message",
    "level": "Information",
    "additionalData": {
      "category": "test",
      "userId": "123"
    }
  }'
```

## Setting up Kibana

1. Open Kibana at http://localhost:5601
2. Navigate to Stack Management > Index Patterns
3. Create a new index pattern:
   - Pattern: `logs-*`
   - Timestamp field: `@timestamp`
4. Go to Discover to view your logs

## Environment Variables

The following environment variables can be configured in docker-compose.yml:

- `ASPNETCORE_ENVIRONMENT`: Application environment (Development, Production)
- `ElasticConfiguration__Uri`: Elasticsearch connection URL
- `ElasticConfiguration__DefaultIndex`: Default index name for logs

## Development

To run the project locally without Docker:

1. Start Elasticsearch and Kibana using Docker:
```bash
docker compose up elasticsearch kibana -d
```

2. Run the API project:
```bash
cd MinimalApiElk
dotnet run
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.