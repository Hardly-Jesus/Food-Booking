#!/bin/bash

# Script para agregar "Prueba" al final de todos los archivos

find . -type f \( -name "*.cs" -o -name "*.yml" -o -name "*.yaml" -o -name "*.json" -o -name "*.html" -o -name "*.js" -o -name "*.ts" -o -name "*.tsx" -o -name "*.jsx" -o -name "*.md" -o -name "*.xml" \) ! -path "./.git/*" ! -path "./.*" | while read file; do
  # Obtener extensión
  ext="${file##*.}"
  
  # Determinar comentario según el tipo de archivo
  case $ext in
    cs|js|ts|tsx|jsx)
      comment="// Prueba"
      ;;
    yml|yaml)
      comment="# Prueba"
      ;;
    json)
      # JSON no soporta comentarios, saltar
      continue
      ;;
    html|xml)
      comment="<!-- Prueba -->"
      ;;
    md)
      comment="<!-- Prueba -->"
      ;;
    *)
      comment="# Prueba"
      ;;
  esac
  
  # Agregar el comentario al final del archivo
  echo "" >> "$file"
  echo "$comment" >> "$file"
done

echo "✅ Comentario 'Prueba' agregado a todos los archivos"
