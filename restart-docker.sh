#!/bin/bash
cd "/home/Coder/Vídeos/Asesstment-2"
echo "Deteniendo contenedores..."
docker compose down
echo "Iniciando contenedores con nuevos puertos..."
docker compose up -d
echo "Verificando estado..."
docker compose ps
