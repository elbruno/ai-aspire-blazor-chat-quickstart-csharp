echo "Running post-create.sh"

echo "Setting up permissions for vscode user"
sudo chown -R vscode /workspaces
/bin/sh -c sudo chown -R vscode:vscode /workspaces 
sudo chmod -R u+rwX /workspaces
echo "Permissions set"

echo "Running apt-get update"
sudo apt-get update && \
    sudo apt upgrade -y && \
    sudo apt-get install -y dos2unix libsecret-1-0 xdg-utils && \
    sudo apt clean -y && \
    sudo rm -rf /var/lib/apt/lists/*
echo "Installed dos2unix, libsecret-1-0, xdg-utils"

## Install .NET Aspire workload
echo "Installing .NET Aspire workload"
sudo dotnet workload update 
sudo dotnet workload install aspire
echo "Installed .NET Aspire workload"

## Install dev certs
echo "Installing dev certs"
dotnet tool update -g linux-dev-certs
dotnet linux-dev-certs install
echo "Installed dev certs"

# build the project
echo "Building the project [./src/AspireBlazorAIChatBot.AppHost]"
cd ./src/AspireBlazorAIChatBot.AppHost
sudo dotnet restore
sudo dotnet build
echo "Project built [./src/AspireBlazorAIChatBot.AppHost]"


echo "Devcontainer setup complete"
