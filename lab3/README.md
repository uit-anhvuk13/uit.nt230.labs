# Lab 3

## Demo
- [Youtube](https://youtu.be/EfaWbFAD90s)

## B.1
- Exploit MS17-010 via Metasploit

## B.2
- Exploit MS17-010 without Metasploit (using [d4t4s3c/Win7Blue](https://github.com/d4t4s3c/Win7Blue))

(Write [install.sh](https://github.com/anhvuk13/uit.nt230/blob/master/lab3/install.sh) that installs dependencies for Win7Blue to work with Kali 2021)

## B.3

### B.3.1
- Manually exploit MS17-010 without executing [Win7Blue.sh](https://github.com/d4t4s3c/Win7Blue/blob/master/Win7Blue.sh)

### B.3.2 (**Not allow to install anything in victim machines**)
- Open port 4444 and 4445
- Manipulate victim 1' shell through MS17-010 (connect back to 4444)
- Use victim 1' shell to exploit MS17-010 on victim 2 (connect back to 4445)

(Use [povlteksttv/Eternalblue](https://github.com/povlteksttv/Eternalblue) and replace its payload with Metasploit's one)
