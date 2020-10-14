# Promp for user login
az login 

# clean up
az group delete -n CentriqAzureDemo 

Remove-Item Config.json
Remove-Item SBSenderConfig.json
Remove-Item SBSvcConfig.json
