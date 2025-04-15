docker-build:
	docker build -t osz2-service:latest .

docker-run:
	docker compose up osz2-service

docker-run-bg:
	docker compose up -d osz2-service