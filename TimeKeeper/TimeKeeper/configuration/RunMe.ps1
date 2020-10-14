# Promp for user login
az login 

# Create a resource group called CentriqAzureDemo
#   We'll use this only for our demo so we can easily clean up
Write-Host "==============================================================="
Write-Host "Creating Resource Group"
Write-Host "---------------------------------------------------------------"
az group create -n CentriqAzureDemo -l eastus --output none

# Create the Redis Cache (About $16.50 per month)
Write-Host "Creating Azure Cache for Redis"
az redis create -n CentriqRedisDemo -g CentriqAzureDemo -l eastus --sku Basic --vm-size c0 --output none

# Wait for a bit for the Redis cache creation to finish up.
#  If we try to get the keys too soon, it will fail\
Write-Host "Waiting for Redis Cache creation to complete"
Start-Sleep -Seconds 45

# Query the Keys for this new Redis CAche and place it in a json file under our
#  configuration directory to be used when we run 
Write-Host "Fetching Keys"
az redis list-keys -n CentriqRedisDemo -g CentriqAzureDemo > Config.json

# Create a Service Bus Namespace called TimeKeeperServiceBus
Write-Host "---------------------------------------------------------------"
Write-Host "Creating Service Bus Namespace for Queue Demo portion"
az servicebus namespace create -g CentriqAzureDemo -n TimekeeperServiceBus --output none

# Add a queue to the bus that we will use to store a massive influx of requests
Write-Host "Adding Queue"
az servicebus queue create -g CentriqAzureDemo -n ChargeEntries --namespace-name TimeKeeperServiceBus --output none

# Establish Access levels and create a key for each one
#  One for our Client to be able to SEND messages, but not read them
#  One for our Service to be able to LISTEN for messages
Write-Host "Setting Queue authorization rules"
az servicebus queue authorization-rule create -g CentriqAzureDemo --namespace-name TimeKeeperServiceBus --queue-name ChargeEntries -n TimeKeeperSender --rights Send --output none
az servicebus queue authorization-rule create -g CentriqAzureDemo --namespace-name TimeKeeperServiceBus --queue-name ChargeEntries -n TimeKeeperSvc --rights Listen --output none

#  Now each of these keys need to be saved to a json file that we can be read in by our applications
Write-Host "Fetching Keys"
az servicebus queue authorization-rule keys list -g CentriqAzureDemo --namespace-name TimeKeeperServiceBus --queue-name ChargeEntries -n TimeKeeperSender > SBSenderConfig.json
az servicebus queue authorization-rule keys list -g CentriqAzureDemo --namespace-name TimeKeeperServiceBus --queue-name ChargeEntries -n TimeKeeperSvc > SBSvcConfig.json

Write-Host "---------------------------------------------------------------"
Write-Host "Creating Sql Database Server"
az sql server create -n TimeKeeperSqlServer -g CentriqAzureDemo -u adminuser -p Pa55w.rd! --output none


Write-Host "Adding Database"
az sql db create -n TimeKeeper -s TimeKeeperSqlServer -g CentriqAzureDemo -e Basic -c 5 --output none
Write-Host "==============================================================="
Write-Host "Ready to use your Redis cache, Service Bus and Sql Database..."
Write-Host "You can now set RedisCacheCore as your start up project and run"