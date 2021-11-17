sudo mkdir /mnt/image-histogram
if [ ! -d "/etc/smbcredentials" ]; then
sudo mkdir /etc/smbcredentials
fi
if [ ! -f "/etc/smbcredentials/apicategorizer.cred" ]; then
    sudo bash -c 'echo "username=apicategorizer" >> /etc/smbcredentials/apicategorizer.cred'
    sudo bash -c 'echo "password=ztmcpSoVYexv45VMja0eT95fiZm7f5tPkMot5IDvmGUAhOFcl5U2syNaTCTF2uM9KBoIb88T0Nq4lwnpqm9G2Q==" >> /etc/smbcredentials/apicategorizer.cred'
fi
sudo chmod 600 /etc/smbcredentials/apicategorizer.cred

sudo bash -c 'echo "//apicategorizer.file.core.windows.net/image-histogram /mnt/image-histogram cifs nofail,vers=3.0,credentials=/etc/smbcredentials/apicategorizer.cred,dir_mode=0777,file_mode=0777,serverino" >> /etc/fstab'
sudo mount -t cifs //apicategorizer.file.core.windows.net/image-histogram /mnt/image-histogram -o vers=3.0,credentials=/etc/smbcredentials/apicategorizer.cred,dir_mode=0777,file_mode=0777,serverino