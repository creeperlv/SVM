.data:
text0 "Hello, World!\n"
file_name "example.txt"
.code:
sd.int32 $4 123
write_file:
sd.int32 $20 -4
sd.int32 $10 file_name
sd.int32 $11 11
sd.int32 $12 1
sys 5
sd.int32 $11 text0
sd.int32 $12 14
sys 4
return
sd.int32 $10 100
sys 1
bmath add Int32 $4 $5 $6
