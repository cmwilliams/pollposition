docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up
docker-compose -f docker-compose-integration-local.yml up --abort-on-container-exit