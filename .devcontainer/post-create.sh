echo "Running post-create.sh"
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

echo "Devcontainer setup complete"
