echo "=================================================="
echo "Running post-start.sh"
echo "=================================================="

echo "Running apt-get update"
apt-get update && \
    apt upgrade -y && \
    apt-get install -y dos2unix libsecret-1-0 xdg-utils && \
    apt clean -y && \
    rm -rf /var/lib/apt/lists/*
echo "Installed dos2unix, libsecret-1-0, xdg-utils"

## Install .NET Aspire workload
echo "Installing .NET Aspire workload"
dotnet workload update 
dotnet workload install aspire
echo "Installed .NET Aspire workload"

# build the project
echo "Building the project [./src/AspireBlazorAIChatBot.AppHost]"
cd ./src/AspireBlazorAIChatBot.AppHost
dotnet restore
dotnet build
echo "Project built [./src/AspireBlazorAIChatBot.AppHost]"

echo "=================================================="
echo "Devcontainer setup complete"
echo "=================================================="