# Generar certificado autofirmado para localhost
$cert = New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\CurrentUser\My" -FriendlyName "localhost for Development" -NotAfter (Get-Date).AddYears(1)

# Exportar el certificado a un archivo .pfx
$certPath = "D:\Repos\DrVideoLibrary\localhost.pfx"
Export-PfxCertificate -Cert $cert -FilePath $certPath -Password (ConvertTo-SecureString -String "password" -Force -AsPlainText)

# Instalar el certificado en el almacén de certificados raíz confiables
$rootCert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($certPath, "password")
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store("Root", "CurrentUser")
$store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
$store.Add($rootCert)
$store.Close()

Write-Host "Certificado generado e instalado exitosamente"

# Listar todos los certificados locales
Get-ChildItem -Path "Cert:\CurrentUser\Root" | Where-Object { $_.Subject -like "*localhost*" }