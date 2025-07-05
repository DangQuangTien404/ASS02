#!/bin/bash
set -e

# Restore & build the full solution
dotnet restore DangQuangTien_Se171443_A02.sln
dotnet build DangQuangTien_Se171443_A02.sln --no-restore

# Optional: run tests (if you have a test project)
# dotnet test DangQuangTien_RazorPages/DangQuangTien_RazorPages.csproj --no-build
