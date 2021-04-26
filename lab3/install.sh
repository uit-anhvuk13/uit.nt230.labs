#!/bin/bash

apt install -y xterm netcat rlwrap

cd /home/kali && mkdir deps && cd deps

curl -O https://files.pythonhosted.org/packages/39/80/06eb7b32dd6c3a5c09e78b039b0b8d1d7a490a2c08b29324464cb97c8446/impacket-0.9.22.tar.gz
curl -O https://files.pythonhosted.org/packages/a4/db/fffec68299e6d7bad3d504147f9094830b704527a7fc098b721d38cc7fa7/pyasn1-0.4.8.tar.gz
curl -O https://files.pythonhosted.org/packages/88/7f/740b99ffb8173ba9d20eb890cc05187677df90219649645aca7e44eb8ff4/pycryptodome-3.10.1.tar.gz
curl -O https://files.pythonhosted.org/packages/82/e2/a0f9f5452a59bafaa3420585f22b58a8566c4717a88c139af2276bb5695d/pycryptodomex-3.10.1.tar.gz

for f in *.tar.gz
do
	tar -xf "$f"
done

for d in */
do
	cd /home/kali/deps/"$d" && python setup.py install
done
